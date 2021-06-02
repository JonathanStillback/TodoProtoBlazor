using System;
using System.Collections.Generic;
using Models;

namespace Interfaces
{
    public interface ITodoRepository
    {
        Todo Create(Todo todo);
        Todo Get(int id);
        List<Todo> GetAll();
        Todo Update(Todo todo);
        Todo Delete(Todo todo);
    }
}
