using Salary.DataAccess.Implementation;
using System.Collections.Generic;

namespace Salary.DataAccess.InMemory
{
    public class InMemoryStorage<T> : IStorage<T>
    {
        public void Dispose()
        {

        }

        public Dictionary<int, T> Entities { get; } = new Dictionary<int, T>();
    }
}