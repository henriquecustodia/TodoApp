using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Todo
{
    [Route("api/todoes")]
    [ApiController]
    public class TodoesController : ControllerBase
    {
        private readonly TodoContext context;
        private readonly TodoesRepository todoesRepository;

        public TodoesController(TodoContext context, TodoesRepository todoesRepository)
        {
            this.context = context;
            this.todoesRepository = todoesRepository;
        }

        [HttpGet]
        public async Task<ActionResult<Page<TodoModel>>> GetAll(int page, int pageSize)
        {
            return await todoesRepository.GetTodoes(page, pageSize);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoModel>> GetTodoById(int id)
        {
            var todo = await todoesRepository.GetTodoById(id);

            if (todo == null)
            {
                return NotFound();
            }

            return todo;
        }

        [HttpPost]
        public async Task<ActionResult<TodoModel>> Create(TodoPayloadModel payload, [FromServices] LoggedUserService loggedUserService)
        {
            var validatorResult = await new TodoPayloadValidator().ValidateAsync(payload);

            if (!validatorResult.IsValid)
            {
                return BadRequest(ValidationResponse.CreateFieldsErrors(validatorResult.Errors));
            }

            var todo = await todoesRepository.CreateTodo(payload);

            return CreatedAtAction(nameof(GetTodoById), new { id = todo.Id }, payload);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var exists = await todoesRepository.Exists(id);

            if (!exists)
            {
                return NotFound();
            }

            await todoesRepository.RemoveTodo(id);

            return NoContent();
        }
    }
}
