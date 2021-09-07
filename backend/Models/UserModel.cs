using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Todo
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password{ get; set; }
        public List<TodoModel> Todos { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
