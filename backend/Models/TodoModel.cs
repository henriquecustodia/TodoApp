using System;
using System.Text.Json.Serialization;

namespace Todo
{
    public class TodoModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDone { get; set; }
        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        public UserModel User { get; set; }
    }
}
