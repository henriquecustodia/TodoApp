using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Todo
{
    public class UsersRepository
    {
        private readonly TodoContext context;

        public UsersRepository(TodoContext context)
        {
            this.context = context;
        }

        public async Task<bool> UserExists(string email)
        {
            return await context.User.AnyAsync(x => x.Email == email);
        }

        public async Task<UserModel> CreateUser(UserPayloadModel payload)
        {
            var user = new UserModel
            {
                Email = payload.Email,
                Password = payload.Password
            };

            await context.User.AddAsync(user);
            await context.SaveChangesAsync();

            return user;
        }

        public async Task<UserModel> GetUserByEmail(string email)
        {
            return await context.User.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<bool> canAuthenticateUser(UserPayloadModel payload)
        {
            var user = await GetUserByEmail(payload.Email);
               
            if (user == null)
            {
                return false;
            }

            return user.Password == payload.Password;
        }
    }
}
