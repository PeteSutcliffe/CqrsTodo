using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Domain.Events;

namespace Todo.Domain
{
    public class TodoList : EntityBase
    {
        private string _name;
        private Guid _id;

        public void CreateNewList(Guid id, string name)
        {
            RaiseEvent(new TodoListCreated{ListId = id, Name = name});
        }

        protected override void RegisterEvents()
        {
            RegisterEvent<TodoListCreated>(ApplyChange);
        }

        private void ApplyChange(TodoListCreated ev)
        {
            _id = ev.ListId;
            _name = ev.Name;
        }
    }
}
