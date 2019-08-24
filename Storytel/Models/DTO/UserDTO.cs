namespace Storytel.Models.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; } = false;
    }
}
