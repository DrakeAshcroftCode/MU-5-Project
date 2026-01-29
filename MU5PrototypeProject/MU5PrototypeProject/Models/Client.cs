namespace MU5PrototypeProject.Models
{
    public class Client
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string Phone {  get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<Session> Sessions { get; set; } = new HashSet<Session>();
    }
}
