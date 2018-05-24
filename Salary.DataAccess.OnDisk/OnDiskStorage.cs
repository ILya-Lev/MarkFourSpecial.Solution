using Newtonsoft.Json;
using Salary.DataAccess.Implementation;
using Salary.Models;
using Salary.Models.Errors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Salary.DataAccess.OnDisk
{
    public class OnDiskStorage<T> : IStorage<T> where T : IEntityWithId
    {
        private readonly string _root;
        public Dictionary<int, T> Entities { get; private set; }

        public OnDiskStorage(string root = null)
        {
            _root = root ?? Path.Combine(Directory.GetCurrentDirectory(), "storage");
            var path = Path.Combine(_root, NameForType());
            LoadEntities(path);
        }

        public void Dispose()
        {
            var path = Path.Combine(_root, NameForType());
            StoreOnDisk(path);
        }

        private static string NameForType()
        {
            var nameSelector = new Dictionary<Type, string>
            {
                [typeof(Employee)] = "Employee.json",
                [typeof(EntityForEmployee)] = "EntityForEmployee.json",
            };

            return nameSelector[typeof(T)];
        }

        private void LoadEntities(string fullPath)
        {
            if (!File.Exists(fullPath))
            {
                Entities = new Dictionary<int, T>();
                return;
            }
            Entities = File.ReadLines(fullPath)
                .Select(DeserializeObject)
                .OfType<T>()
                .ToDictionary(item => item.Id);
        }

        private static IEntityWithId DeserializeObject(string line)
        {
            var result = JsonConvert.DeserializeObject<T>(line);
            if (result.Id != 0)
                return result;

            if (typeof(T) == typeof(Employee))
            {
                throw new RepositoryException($"Employee storage corrupted")
                {
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }

            return TryDeserializeEntityWithId<TimeCard>(line)
                ?? TryDeserializeEntityWithId<SalaryPayment>(line)
                ?? TryDeserializeEntityWithId<SalesReceipt>(line)
                ?? TryDeserializeEntityWithId<ServiceCharge>(line);
        }

        private static IEntityWithId TryDeserializeEntityWithId<TValue>(string line) where TValue : IEntityWithId
        {
            var entity = JsonConvert.DeserializeObject<TValue>(line);
            if (entity.Id != 0)
                return entity;
            return null;
        }

        private void StoreOnDisk(string fullPath)
        {
            if (!Directory.Exists(_root))
                Directory.CreateDirectory(_root);

            File.WriteAllLines(fullPath, Entities.Select(e => JsonConvert.SerializeObject(e.Value, Formatting.None)));
        }
    }
}
