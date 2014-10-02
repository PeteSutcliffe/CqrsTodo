using System;
using System.Collections.Generic;

namespace Todo.Web.Models
{    
    public class TodoList
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Todo> Todos { get; set; }
    }

    public class Todo
    {
        public Guid Id { get; set; }
        public string Action { get; set; }
        public bool Complete { get; set; }
        public Guid ListId { get; set; }
    }
}