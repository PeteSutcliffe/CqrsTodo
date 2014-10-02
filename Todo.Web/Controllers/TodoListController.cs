using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Todo.Command;
using Todo.Command.Infrastructure;
using Todo.ReadModel;

namespace Todo.Web.Controllers
{
    public class TodoListController : ApiController
    {
        private readonly ICommandBus _commandBus;

        //todo: inject this
        TodoContext _readModel = new TodoContext();

        public TodoListController(ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }

        public IEnumerable<Models.TodoList> Get()
        {
            var todoLists = _readModel.TodoLists
                .Include("Todos")
                .Select(l => new Models.TodoList
                {
                    Id = l.Id, 
                    Name = l.Name, 
                    Todos = l.Todos.Select(t => new Models.Todo
                    {
                        Action = t.Action,
                        Complete = t.Complete,
                        Id = t.Id,
                        ListId = l.Id
                    }).ToList()
                })
                .ToList();
            return todoLists;
        }

        public async Task<TodoList> Post([FromBody]TodoList list)
        {
            var id = Guid.NewGuid();

            var command = new CreateTodoList(id, list.Name);
            await _commandBus.SendAsync(command);

            list.Id = id;

            return list;
        }
    }
}
