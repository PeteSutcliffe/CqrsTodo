using System;
using System.Runtime.Serialization;

namespace Todo.Command
{    
    [Serializable]
    public class CreateTodoList : ICommand
    {
        public CreateTodoList(Guid id, string name)
        {
            Name = name;
            Id = id;
        }

        public CreateTodoList()
        {
            
        }

        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
