using System;
using System.Collections.Generic;
using Models;

namespace Interfaces
{
    public interface IDBProvider<T>
    {
        long Create(T item);
        T Get(int id);
				IEnumerable<T> Get(string where, object param);
        IEnumerable<T> GetAll();
        T Update(T item);
        T Delete(T item);
    }
}
