using System;
using System.Collections.Generic;

namespace Salary.DataAccess.Implementation
{
    public interface IStorage<T> : IDisposable
    {
        Dictionary<int, T> Entities { get; }
    }
}