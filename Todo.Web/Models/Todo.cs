using System;

namespace Todo.Web.Models
{    
    public class TodoList
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class Todo
    {
        public Guid Id { get; set; }
        public string Action { get; set; }
        public bool Done { get; set; }
    }
}