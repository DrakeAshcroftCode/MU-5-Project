using Microsoft.EntityFrameworkCore;
using MU5PrototypeProject.Models;
using System.Diagnostics;

namespace MU5PrototypeProject.Data
{
    public static class MUInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider,
            bool deleteDatabase = false, bool useMigrations = true, bool seedSampleData = true)
        {
            using var context = new MUContext(
                serviceProvider.GetRequiredService<DbContextOptions<MUContext>>());
            try
            {
                // Prepare the database (delete optional, then create/migrate)
                if (deleteDatabase || !context.Database.CanConnect())
                {
                    if (useMigrations)
                        context.Database.Migrate();
                    else
                        context.Database.EnsureCreated();
                }
                else if (useMigrations)
                {
                    context.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.GetBaseException().Message);
            }

            if (!seedSampleData) return;

            // Required data (trainers, exercises) if empty
            if (!context.Trainers.Any())
            {
                context.Trainers.AddRange(
                    new Trainer { ID = 1, FirstName = "Alex",   LastName = "Green" },
                    new Trainer { ID = 2, FirstName = "Jordan", LastName = "Lee" },
                    new Trainer { ID = 3, FirstName = "Morgan", LastName = "Park" },
                    new Trainer { ID = 4, FirstName = "Taylor", LastName = "Kim" }
                );
                context.SaveChanges();
            }

            if (!context.Exercises.Any())
            {
                context.Exercises.AddRange(
                    new Exercise { ID = 1,  ExerciseName = "Reformer Footwork", Apparatus = "Reformer" },
                    new Exercise { ID = 2,  ExerciseName = "Hundred",            Apparatus = "Reformer" },
                    new Exercise { ID = 3,  ExerciseName = "Wall Squat",         Apparatus = "Mat"      },
                    new Exercise { ID = 4,  ExerciseName = "Bridge",             Apparatus = "Mat"      },
                    new Exercise { ID = 5,  ExerciseName = "Spine Stretch",      Apparatus = "Mat"      },
                    new Exercise { ID = 6,  ExerciseName = "Rowing Front",       Apparatus = "Reformer" },
                    new Exercise { ID = 7,  ExerciseName = "Cat-Cow",            Apparatus = "Mat"      },
                    new Exercise { ID = 8,  ExerciseName = "Lunge",              Apparatus = "Reformer" },
                    new Exercise { ID = 9,  ExerciseName = "Single Leg Stand",   Apparatus = "Mat"      },
                    new Exercise { ID = 10, ExerciseName = "Swan",               Apparatus = "Mat"      },
                    new Exercise { ID = 11, ExerciseName = "Side Splits",        Apparatus = "Reformer" },
                    new Exercise { ID = 12, ExerciseName = "Plank",              Apparatus = "Mat"      }
                );
                context.SaveChanges();
            }

            // Sample clients (20) if empty
            if (!context.Clients.Any())
            {
                var clients = new List<Client>
                {
                    new Client { ID = 1,  FirstName = "Ava",      LastName = "Reed",     DOB = new DateTime(1990,5,12), Phone = "2265550101", Email = "ava.reed@example.com",      IsActive = true, CreatedAt = new DateTime(2025,1,3)  },
                    new Client { ID = 2,  FirstName = "Noah",     LastName = "Hughes",   DOB = new DateTime(1985,8,23), Phone = "2895550102", Email = "noah.hughes@example.com",    IsActive = true, CreatedAt = new DateTime(2025,1,4)  },
                    new Client { ID = 3,  FirstName = "Mia",      LastName = "Turner",   DOB = new DateTime(1992,2,14), Phone = "3435550103", Email = "mia.turner@example.com",     IsActive = true, CreatedAt = new DateTime(2025,1,5)  },
                    new Client { ID = 4,  FirstName = "Liam",     LastName = "Baker",    DOB = new DateTime(1988,11,2), Phone = "3655550104", Email = "liam.baker@example.com",     IsActive = true, CreatedAt = new DateTime(2025,1,6)  },
                    new Client { ID = 5,  FirstName = "Emma",     LastName = "Cole",     DOB = new DateTime(1995,7,7),  Phone = "4165550105", Email = "emma.cole@example.com",      IsActive = true, CreatedAt = new DateTime(2025,1,7)  },
                    new Client { ID = 6,  FirstName = "Oliver",   LastName = "Shaw",     DOB = new DateTime(1983,6,9),  Phone = "4375550106", Email = "oliver.shaw@example.com",    IsActive = true, CreatedAt = new DateTime(2025,1,8)  },
                    new Client { ID = 7,  FirstName = "Sophia",   LastName = "Wong",     DOB = new DateTime(1991,3,29), Phone = "5195550107", Email = "sophia.wong@example.com",    IsActive = true, CreatedAt = new DateTime(2025,1,9)  },
                    new Client { ID = 8,  FirstName = "Lucas",    LastName = "Nguyen",   DOB = new DateTime(1996,1,17), Phone = "5485550108", Email = "lucas.nguyen@example.com",   IsActive = true, CreatedAt = new DateTime(2025,1,10) },
                    new Client { ID = 9,  FirstName = "Isabella", LastName = "Khan",     DOB = new DateTime(1993,12,4), Phone = "5795550109", Email = "isabella.khan@example.com",  IsActive = true, CreatedAt = new DateTime(2025,1,11) },
                    new Client { ID = 10, FirstName = "Ethan",    LastName = "Moore",    DOB = new DateTime(1987,9,1),  Phone = "5815550110", Email = "ethan.moore@example.com",    IsActive = true, CreatedAt = new DateTime(2025,1,12) },
                    new Client { ID = 11, FirstName = "Amelia",   LastName = "Young",    DOB = new DateTime(1994,4,26), Phone = "5875550111", Email = "amelia.young@example.com",   IsActive = true, CreatedAt = new DateTime(2025,1,13) },
                    new Client { ID = 12, FirstName = "James",    LastName = "Diaz",     DOB = new DateTime(1989,10,20),Phone = "6045550112", Email = "james.diaz@example.com",     IsActive = true, CreatedAt = new DateTime(2025,1,14) },
                    new Client { ID = 13, FirstName = "Charlotte",LastName = "Singh",    DOB = new DateTime(1997,3,6),  Phone = "6135550113", Email = "charlotte.singh@example.com",IsActive = true, CreatedAt = new DateTime(2025,1,15) },
                    new Client { ID = 14, FirstName = "Benjamin", LastName = "Chen",     DOB = new DateTime(1990,8,8),  Phone = "6225550114", Email = "benjamin.chen@example.com",  IsActive = true, CreatedAt = new DateTime(2025,1,16) },
                    new Client { ID = 15, FirstName = "Harper",   LastName = "Ali",      DOB = new DateTime(1992,5,18), Phone = "6475550115", Email = "harper.ali@example.com",     IsActive = true, CreatedAt = new DateTime(2025,1,17) },
                    new Client { ID = 16, FirstName = "Henry",    LastName = "Brown",    DOB = new DateTime(1986,2,9),  Phone = "6725550116", Email = "henry.brown@example.com",    IsActive = true, CreatedAt = new DateTime(2025,1,18) },
                    new Client { ID = 17, FirstName = "Evelyn",   LastName = "Patel",    DOB = new DateTime(1993,7,30), Phone = "7055550117", Email = "evelyn.patel@example.com",   IsActive = true, CreatedAt = new DateTime(2025,1,19) },
                    new Client { ID = 18, FirstName = "Daniel",   LastName = "Gomez",    DOB = new DateTime(1984,6,12), Phone = "7425550118", Email = "daniel.gomez@example.com",   IsActive = true, CreatedAt = new DateTime(2025,1,20) },
                    new Client { ID = 19, FirstName = "Abigail",  LastName = "Foster",   DOB = new DateTime(1998,1,5),  Phone = "7785550119", Email = "abigail.foster@example.com", IsActive = true, CreatedAt = new DateTime(2025,1,21) },
                    new Client { ID = 20, FirstName = "Jack",     LastName = "Murphy",   DOB = new DateTime(1982,11,28),Phone = "8075550120", Email = "jack.murphy@example.com",    IsActive = true, CreatedAt = new DateTime(2025,1,22) }
                };
                context.Clients.AddRange(clients);
                context.SaveChanges();
            }

            // Sessions if empty
            if (!context.Sessions.Any())
            {
                var sessions = new List<Session>
                {
                    new Session { ID = 1,  SessionDate = new DateTime(2025,2,1),  CreatedAt = new DateTime(2025,1,25), SessionsPerWeekRecommended = 2, TrainerID = 1, ClientID = 1 },
                    new Session { ID = 2,  SessionDate = new DateTime(2025,2,2),  CreatedAt = new DateTime(2025,1,25), SessionsPerWeekRecommended = 2, TrainerID = 2, ClientID = 2 },
                    new Session { ID = 3,  SessionDate = new DateTime(2025,2,3),  CreatedAt = new DateTime(2025,1,26), SessionsPerWeekRecommended = 3, TrainerID = 3, ClientID = 3 },
                    new Session { ID = 4,  SessionDate = new DateTime(2025,2,4),  CreatedAt = new DateTime(2025,1,26), SessionsPerWeekRecommended = 1, TrainerID = 1, ClientID = 4 },
                    new Session { ID = 5,  SessionDate = new DateTime(2025,2,5),  CreatedAt = new DateTime(2025,1,27), SessionsPerWeekRecommended = 2, TrainerID = 2, ClientID = 5 },
                    new Session { ID = 6,  SessionDate = new DateTime(2025,2,6),  CreatedAt = new DateTime(2025,1,27), SessionsPerWeekRecommended = 1, TrainerID = 3, ClientID = 6 },
                    new Session { ID = 7,  SessionDate = new DateTime(2025,2,7),  CreatedAt = new DateTime(2025,1,28), SessionsPerWeekRecommended = 2, TrainerID = 1, ClientID = 7 },
                    new Session { ID = 8,  SessionDate = new DateTime(2025,2,8),  CreatedAt = new DateTime(2025,1,28), SessionsPerWeekRecommended = 2, TrainerID = 2, ClientID = 8 },
                    new Session { ID = 9,  SessionDate = new DateTime(2025,2,9),  CreatedAt = new DateTime(2025,1,29), SessionsPerWeekRecommended = 3, TrainerID = 3, ClientID = 9 },
                    new Session { ID = 10, SessionDate = new DateTime(2025,2,10), CreatedAt = new DateTime(2025,1,29), SessionsPerWeekRecommended = 1, TrainerID = 1, ClientID = 10 },
                    new Session { ID = 11, SessionDate = new DateTime(2025,2,11), CreatedAt = new DateTime(2025,1,30), SessionsPerWeekRecommended = 2, TrainerID = 2, ClientID = 11 },
                    new Session { ID = 12, SessionDate = new DateTime(2025,2,12), CreatedAt = new DateTime(2025,1,30), SessionsPerWeekRecommended = 2, TrainerID = 3, ClientID = 12 }
                };
                context.Sessions.AddRange(sessions);
                context.SaveChanges();
            }

            // SessionNotes
            if (!context.SessionNotes.Any())
            {
                context.SessionNotes.AddRange(
                    new SessionNotes { ID = 1,  SessionID = 1,  GeneralComments = "New client; explain apparatus safety.", SubjectiveReports = "3/10 shoulder pain.", ObjectiveFindings = "Limited ROM on left shoulder.", Plan = "Mobility + core stability." },
                    new SessionNotes { ID = 2,  SessionID = 2,  GeneralComments = "Prefers mornings.", SubjectiveReports = "Knee stable.", ObjectiveFindings = "Good squat mechanics.", Plan = "Progress load carefully." },
                    new SessionNotes { ID = 3,  SessionID = 3,  GeneralComments = "Asthma managed.", SubjectiveReports = "Breathing ok.", ObjectiveFindings = "Normal vitals.", Plan = "Endurance and posture." },
                    new SessionNotes { ID = 4,  SessionID = 4,  GeneralComments = "Desk job; back stiffness.", SubjectiveReports = "Tight lower back.", ObjectiveFindings = "Tight hamstrings.", Plan = "Hip hinge patterning." },
                    new SessionNotes { ID = 5,  SessionID = 5,  GeneralComments = "Very motivated.", SubjectiveReports = "Wants strength.", ObjectiveFindings = "Baseline strength ok.", Plan = "Progressive overload." },
                    new SessionNotes { ID = 6,  SessionID = 6,  GeneralComments = "", SubjectiveReports = "Occasional ankle soreness.", ObjectiveFindings = "Slight instability.", Plan = "Balance + ankle stability." },
                    new SessionNotes { ID = 7,  SessionID = 7,  GeneralComments = "", SubjectiveReports = "Sleep improved.", ObjectiveFindings = "Better thoracic mobility.", Plan = "Introduce loaded carries." },
                    new SessionNotes { ID = 8,  SessionID = 8,  GeneralComments = "Hypertension controlled.", SubjectiveReports = "Energy low.", ObjectiveFindings = "BP safe range.", Plan = "Longer warm-up; intervals." },
                    new SessionNotes { ID = 9,  SessionID = 9,  GeneralComments = "", SubjectiveReports = "No pain today.", ObjectiveFindings = "Asymmetry reduced.", Plan = "Eccentric hamstring work." },
                    new SessionNotes { ID = 10, SessionID = 10, GeneralComments = "", SubjectiveReports = "Stressed week.", ObjectiveFindings = "Tension in traps.", Plan = "Downregulate, breathing." },
                    new SessionNotes { ID = 11, SessionID = 11, GeneralComments = "", SubjectiveReports = "Ready for test.", ObjectiveFindings = "Strong core activation.", Plan = "Advance series." },
                    new SessionNotes { ID = 12, SessionID = 12, GeneralComments = "", SubjectiveReports = "Mild DOMS.", ObjectiveFindings = "Good recovery.", Plan = "Maintain volume." }
                );
                context.SaveChanges();
            }

            // AdminStatus
            if (!context.AdminStatuses.Any())
            {
                context.AdminStatuses.AddRange(
                    new AdminStatus { ID = 1,  SessionID = 1,  IsPaid = false, AdminNotes = "Intake complete.", AdminInitials = "AG", NextAppointmentBooked = true,  CommunicatedProgress = false, ReadyToProgress = false, CourseCorrectionNeeded = false, TeamConsult = false, ReferredExternally = false, ReferredTo = null },
                    new AdminStatus { ID = 2,  SessionID = 2,  IsPaid = true,  AdminNotes = "Completed.",       AdminInitials = "JL", NextAppointmentBooked = true,  CommunicatedProgress = true,  ReadyToProgress = true,  CourseCorrectionNeeded = false, TeamConsult = false, ReferredExternally = false, ReferredTo = null },
                    new AdminStatus { ID = 3,  SessionID = 3,  IsPaid = true,  AdminNotes = "Completed.",       AdminInitials = "MP", NextAppointmentBooked = true,  CommunicatedProgress = true,  ReadyToProgress = true,  CourseCorrectionNeeded = false, TeamConsult = false, ReferredExternally = false, ReferredTo = null },
                    new AdminStatus { ID = 4,  SessionID = 4,  IsPaid = false, AdminNotes = "Cancelled.",       AdminInitials = "AG", NextAppointmentBooked = false, CommunicatedProgress = false, ReadyToProgress = false, CourseCorrectionNeeded = false, TeamConsult = false, ReferredExternally = false, ReferredTo = null },
                    new AdminStatus { ID = 5,  SessionID = 5,  IsPaid = false, AdminNotes = "Scheduled.",       AdminInitials = "JL", NextAppointmentBooked = true,  CommunicatedProgress = false, ReadyToProgress = false, CourseCorrectionNeeded = false, TeamConsult = false, ReferredExternally = false, ReferredTo = null },
                    new AdminStatus { ID = 6,  SessionID = 6,  IsPaid = false, AdminNotes = "No-show invoiced.",AdminInitials = "MP", NextAppointmentBooked = false, CommunicatedProgress = false, ReadyToProgress = false, CourseCorrectionNeeded = false, TeamConsult = false, ReferredExternally = false, ReferredTo = null },
                    new AdminStatus { ID = 7,  SessionID = 7,  IsPaid = true,  AdminNotes = "Completed.",       AdminInitials = "AG", NextAppointmentBooked = true,  CommunicatedProgress = true,  ReadyToProgress = true,  CourseCorrectionNeeded = false, TeamConsult = false, ReferredExternally = false, ReferredTo = null },
                    new AdminStatus { ID = 8,  SessionID = 8,  IsPaid = false, AdminNotes = "Scheduled.",       AdminInitials = "JL", NextAppointmentBooked = true,  CommunicatedProgress = false, ReadyToProgress = false, CourseCorrectionNeeded = false, TeamConsult = false, ReferredExternally = false, ReferredTo = null },
                    new AdminStatus { ID = 9,  SessionID = 9,  IsPaid = true,  AdminNotes = "Completed.",       AdminInitials = "MP", NextAppointmentBooked = true,  CommunicatedProgress = true,  ReadyToProgress = true,  CourseCorrectionNeeded = false, TeamConsult = false, ReferredExternally = false, ReferredTo = null },
                    new AdminStatus { ID = 10, SessionID = 10, IsPaid = true,  AdminNotes = "Completed.",       AdminInitials = "AG", NextAppointmentBooked = true,  CommunicatedProgress = true,  ReadyToProgress = true,  CourseCorrectionNeeded = false, TeamConsult = false, ReferredExternally = false, ReferredTo = null },
                    new AdminStatus { ID = 11, SessionID = 11, IsPaid = false, AdminNotes = "Scheduled.",       AdminInitials = "JL", NextAppointmentBooked = true,  CommunicatedProgress = false, ReadyToProgress = false, CourseCorrectionNeeded = false, TeamConsult = false, ReferredExternally = false, ReferredTo = null },
                    new AdminStatus { ID = 12, SessionID = 12, IsPaid = false, AdminNotes = "Scheduled.",       AdminInitials = "MP", NextAppointmentBooked = true,  CommunicatedProgress = false, ReadyToProgress = false, CourseCorrectionNeeded = false, TeamConsult = false, ReferredExternally = false, ReferredTo = null }
                );
                context.SaveChanges();
            }

            // SessionExercises (2 per session)
            if (!context.SessionExercises.Any())
            {
                context.SessionExercises.AddRange(
                    new SessionExercise { ID = 1,  SessionID = 1,  ExerciseID = 1,  Springs = "3R", Props = "",       Notes = "Neutral pelvis focus." },
                    new SessionExercise { ID = 2,  SessionID = 1,  ExerciseID = 2,  Springs = "2R", Props = "Straps", Notes = "Breathing emphasis."   },
                    new SessionExercise { ID = 3,  SessionID = 2,  ExerciseID = 3,  Springs = "",   Props = "Ball",   Notes = "Knee alignment."       },
                    new SessionExercise { ID = 4,  SessionID = 2,  ExerciseID = 4,  Springs = "",   Props = "",       Notes = "Glute activation."     },
                    new SessionExercise { ID = 5,  SessionID = 3,  ExerciseID = 5,  Springs = "",   Props = "",       Notes = ""                      },
                    new SessionExercise { ID = 6,  SessionID = 3,  ExerciseID = 6,  Springs = "1R", Props = "Straps", Notes = ""                      },
                    new SessionExercise { ID = 7,  SessionID = 4,  ExerciseID = 7,  Springs = "",   Props = "",       Notes = "Lumbar mobility."      },
                    new SessionExercise { ID = 8,  SessionID = 5,  ExerciseID = 8,  Springs = "1R", Props = "Box",    Notes = "Control tempo."        },
                    new SessionExercise { ID = 9,  SessionID = 6,  ExerciseID = 9,  Springs = "",   Props = "Pad",    Notes = "Ankle stability."      },
                    new SessionExercise { ID = 10, SessionID = 7,  ExerciseID = 10, Springs = "",   Props = "",       Notes = ""                      },
                    new SessionExercise { ID = 11, SessionID = 8,  ExerciseID = 11, Springs = "1R", Props = "",       Notes = ""                      },
                    new SessionExercise { ID = 12, SessionID = 9,  ExerciseID = 12, Springs = "",   Props = "",       Notes = ""                      },
                    new SessionExercise { ID = 13, SessionID = 10, ExerciseID = 1,  Springs = "3R", Props = "",       Notes = ""                      },
                    new SessionExercise { ID = 14, SessionID = 10, ExerciseID = 2,  Springs = "2R", Props = "Straps", Notes = ""                      },
                    new SessionExercise { ID = 15, SessionID = 11, ExerciseID = 3,  Springs = "",   Props = "Ball",   Notes = ""                      },
                    new SessionExercise { ID = 16, SessionID = 11, ExerciseID = 4,  Springs = "",   Props = "",       Notes = ""                      },
                    new SessionExercise { ID = 17, SessionID = 12, ExerciseID = 5,  Springs = "",   Props = "",       Notes = ""                      },
                    new SessionExercise { ID = 18, SessionID = 12, ExerciseID = 6,  Springs = "1R", Props = "Straps", Notes = ""                      },
                    new SessionExercise { ID = 19, SessionID = 8,  ExerciseID = 1,  Springs = "3R", Props = "",       Notes = ""                      },
                    new SessionExercise { ID = 20, SessionID = 9,  ExerciseID = 2,  Springs = "2R", Props = "Straps", Notes = ""                      },
                    new SessionExercise { ID = 21, SessionID = 6,  ExerciseID = 3,  Springs = "",   Props = "Ball",   Notes = ""                      },
                    new SessionExercise { ID = 22, SessionID = 7,  ExerciseID = 4,  Springs = "",   Props = "",       Notes = ""                      },
                    new SessionExercise { ID = 23, SessionID = 5,  ExerciseID = 5,  Springs = "",   Props = "",       Notes = ""                      },
                    new SessionExercise { ID = 24, SessionID = 4,  ExerciseID = 6,  Springs = "1R", Props = "Straps", Notes = ""                      }
                );
                context.SaveChanges();
            }
        }
    }
}