namespace MU5PrototypeProject.Models
{
    public class Session
    {

        public int ID { get; set; }
        public DateTime SessionDate { get; set; }
        public DateTime CreatedAt { get; set; }

        //NOTE:(kaiden) Check against data model
        //Do we need to keep these between different sessions?
        //Do we need multiple of these per session?

        //SessionNotes
        public string HealthMedicalHistory { get; set; } //Replace with PDF attachment later I believe
        public string GeneralComments { get; set; }
        public string SubjectiveReport { get; set; }
        public string ObjectiveReport { get; set; }
        public string Plan { get; set; } //Make more descriptive?


        public ICollection<SessionExercise> Exercises { get; set; } = new HashSet<SessionExercise>();

        public int TrainerID { get; set; }
        public Trainer? Trainer { get; set; }
        public int ClientID {  get; set; }
        public Client? Client { get; set; }

    }
}
