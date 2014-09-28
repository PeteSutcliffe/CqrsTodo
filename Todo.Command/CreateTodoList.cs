using System;
using System.Runtime.Serialization;

namespace Todo.Command
{    
    [DataContract]
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

        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}
