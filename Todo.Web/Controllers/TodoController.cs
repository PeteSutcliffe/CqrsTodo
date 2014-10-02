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
    public class TodoController : ApiController
    {
        private readonly ICommandBus _commandBus;

        //todo: inject this
        TodoContext _readModel = new TodoContext();

        public TodoController(ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }        

        public async Task<Models.Todo> Post([FromBody]Models.Todo todo)
        {
            var id = Guid.NewGuid();

            var command = new CreateTodo(todo.ListId, id, todo.Action);
            await _commandBus.SendAsync(command);

            todo.Id = id;
            return todo;
        }       

        public async void Put(Guid id, [FromBody]Models.Todo todo)
        {            

            var command = new SetCompleted(todo.ListId, id, todo.Complete);
            await _commandBus.SendAsync(command);
        }
    }
}