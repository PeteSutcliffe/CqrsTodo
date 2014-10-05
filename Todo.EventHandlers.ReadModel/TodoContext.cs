using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.SqlClient;

namespace Todo.ReadModel
{
    public class TodoContext : DbContext
    {
        public TodoContext()
            : base(@"Data Source=(LocalDB)\v11.0;Initial Catalog=Todo.EventHandlers.ReadModel.TodoContext;Integrated Security=True")
        {
            
        }

        public DbSet<TodoList> TodoLists { get; set; }
    }

    public class TodoList
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Todo> Todos { get; set; }

        public TodoList()
        {
            Todos = new Collection<Todo>();
        }
    }

    public class Todo
    {
        public Guid Id { get; set; }
        public string Action { get; set; }
        public bool Complete { get; set; }
    }
}