namespace MU5PrototypeProject.Models
{
    public class Trainer
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<Session> Sessions { get; set; } = new HashSet<Session>();
    }
}
