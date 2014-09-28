using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Todo.Command;
using Todo.Command.Infrastructure;
using Todo.Web.Models;

namespace Todo.Web.Controllers
{
    public class TodoListController : ApiController
    {
        private readonly ICommandBus _commandBus;

        public TodoListController(ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }

        public IEnumerable<TodoList> Get()
        {
            return new List<TodoList>
            {
                new TodoList()
                {
                    Id = Guid.NewGuid(),
                    Name = "First"
                },
                new TodoList()
                {
                    Id = Guid.NewGuid(),
                    Name = "2nd"
                }
            };
        }

        public async Task<TodoList> Post([FromBody]TodoList list)
        {
            var command = new CreateTodoList(Guid.NewGuid(), list.Name);
            await _commandBus.SendAsync(command);

            list.Id = Guid.NewGuid();

            return list;
        }
    }
}
