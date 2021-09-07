using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Todo
{
    public class TodoesRepository
    {
        private readonly TodoContext context;
        private readonly LoggedUserService loggedUserService;

        public TodoesRepository(TodoContext context, LoggedUserService loggedUserService)
        {
            this.context = context;
            this.loggedUserService = loggedUserService;
        }

        public async Task<Page<TodoModel>> GetTodoes(int page, int pageSize)
        {
            var query = from t in context.Todo
                        let user = t.User
                        where user.Id == loggedUserService.User.Id
                        select t;

            return await Pagination<TodoModel>.Create(query, page, pageSize);
        }

        public async Task<TodoModel> GetTodoById(int id)
        {
            var query = from t in context.Todo
                        let user = t.User
                        where t.Id == id && user.Id == loggedUserService.User.Id
                        select t;

           return await query.FirstOrDefaultAsync();
        }

        public async Task<TodoModel> CreateTodo(TodoPayloadModel payload)
        {
            var todo = new TodoModel
            {
                Name = payload.Name,
                IsDone = payload.IsDone == true,
                User = loggedUserService.User
            };

            await context.Todo.AddAsync(todo);
            await context.SaveChangesAsync();

            return todo;
        }

        public async Task RemoveTodo(int id)
        {
            var todo = await context.Todo.FindAsync(id);
            context.Todo.Remove(todo);
            await context.SaveChangesAsync();
        }

        public async Task<bool> Exists(int id)
        {
            return await context.Todo.AnyAsync(x => x.Id == id);
        }

    }
}
