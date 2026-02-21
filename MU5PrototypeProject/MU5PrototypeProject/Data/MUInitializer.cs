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
                    {
                        context.Database.Migrate();
                    }
                    else
                    {
                        context.Database.EnsureCreated();
                    }
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

            if (!seedSampleData)
            {
                return;
            }

            // Required data (trainers, exercises) if empty
            if (!context.Trainers.Any())
            {
                context.Trainers.AddRange(
                    new Trainer { FirstName = "Alex",   LastName = "Green", Email = "alex@mu.com",   Role = "Trainer", IsActive = true },
                    new Trainer { FirstName = "Jordan", LastName = "Lee",   Email = "jordan@mu.com", Role = "Admin",   IsActive = true },
                    new Trainer { FirstName = "Morgan", LastName = "Park",  Email = "morgan@mu.com", Role = "Physio",  IsActive = true },
                    new Trainer { FirstName = "Taylor", LastName = "Kim",   Email = "taylor@mu.com", Role = "Trainer", IsActive = true }
                );
                context.SaveChanges();
            }

            if (!context.Apparatuses.Any())
            {
                context.Apparatuses.AddRange(
                    new Apparatus { ApparatusName = "Reformer" },
                    new Apparatus { ApparatusName = "Mat"      },
                    new Apparatus { ApparatusName = "Chair"    },
                    new Apparatus { ApparatusName = "Cadillac" }
                );
                context.SaveChanges();
            }

            if (!context.Exercises.Any())
            {
                // Load apparatus IDs after seeding
                int reformerID = context.Apparatuses.First(a => a.ApparatusName == "Reformer").ID;
                int matID      = context.Apparatuses.First(a => a.ApparatusName == "Mat").ID;

                context.Exercises.AddRange(
                    new Exercise { ExerciseName = "Reformer Footwork", ApparatusID = reformerID },
                    new Exercise { ExerciseName = "Hundred",            ApparatusID = reformerID },
                    new Exercise { ExerciseName = "Wall Squat",         ApparatusID = matID      },
                    new Exercise { ExerciseName = "Bridge",             ApparatusID = matID      },
                    new Exercise { ExerciseName = "Spine Stretch",      ApparatusID = matID      },
                    new Exercise { ExerciseName = "Rowing Front",       ApparatusID = reformerID },
                    new Exercise { ExerciseName = "Cat-Cow",            ApparatusID = matID      },
                    new Exercise { ExerciseName = "Lunge",              ApparatusID = reformerID },
                    new Exercise { ExerciseName = "Single Leg Stand",   ApparatusID = matID      },
                    new Exercise { ExerciseName = "Swan",               ApparatusID = matID      },
                    new Exercise { ExerciseName = "Side Splits",        ApparatusID = reformerID },
                    new Exercise { ExerciseName = "Plank",              ApparatusID = matID      }
                );
                context.SaveChanges();
            }

            // Sample clients (~60) if empty
            if (!context.Clients.Any())
            {
                var clients = new List<Client>
                {
                    // === Existing 20 specific clients ===
                    new Client
                    {
                        FirstName = "Ava",
                        LastName = "Reed",
                        DOB = new DateTime(1990,5,12),
                        Phone = "2265550101",
                        Email = "ava.reed@example.com",
                        ClientFolderUrl = "https://files.movementunlimited.com/clients/ava-reed",
                        IsArchived = false
                    },
                    new Client
                    {
                        FirstName = "Noah",
                        LastName = "Hughes",
                        DOB = new DateTime(1985,8,23),
                        Phone = "2895550102",
                        Email = "noah.hughes@example.com",
                        ClientFolderUrl = "https://files.movementunlimited.com/clients/noah-hughes",
                        IsArchived = false
                    },
                    new Client
                    {
                        FirstName = "Mia",
                        LastName = "Turner",
                        DOB = new DateTime(1992,2,14),
                        Phone = "3435550103",
                        Email = "mia.turner@example.com",
                        ClientFolderUrl = "https://files.movementunlimited.com/clients/mia-turner",
                        IsArchived = true
                    },
                    new Client
                    {
                        FirstName = "Liam",
                        LastName = "Baker",
                        DOB = new DateTime(1988,11,2),
                        Phone = "3655550104",
                        Email = "liam.baker@example.com",
                        ClientFolderUrl = "https://files.movementunlimited.com/clients/liam-baker",
                        IsArchived = false
                    },
                    new Client
                    {
                        FirstName = "Emma",
                        LastName = "Cole",
                        DOB = new DateTime(1995,7,7),
                        Phone = "4165550105",
                        Email = "emma.cole@example.com",
                        ClientFolderUrl = "https://files.movementunlimited.com/clients/emma-cole",
                        IsArchived = false
                    },
                    new Client
                    {
                        FirstName = "Oliver",
                        LastName = "Shaw",
                        DOB = new DateTime(1983,6,9),
                        Phone = "4375550106",
                        Email = "oliver.shaw@example.com",
                        ClientFolderUrl = "https://files.movementunlimited.com/clients/oliver-shaw",
                        IsArchived = false
                    },
                    new Client
                    {
                        FirstName = "Sophia",
                        LastName = "Wong",
                        DOB = new DateTime(1991,3,29),
                        Phone = "5195550107",
                        Email = "sophia.wong@example.com",
                        ClientFolderUrl = "https://files.movementunlimited.com/clients/sophia-wong",
                        IsArchived = false
                    },
                    new Client
                    {
                        FirstName = "Lucas",
                        LastName = "Nguyen",
                        DOB = new DateTime(1996,1,17),
                        Phone = "5485550108",
                        Email = "lucas.nguyen@example.com",
                        ClientFolderUrl = "https://files.movementunlimited.com/clients/lucas-nguyen",
                        IsArchived = false
                    },
                    new Client
                    {
                        FirstName = "Isabella",
                        LastName = "Khan",
                        DOB = new DateTime(1993,12,4),
                        Phone = "5795550109",
                        Email = "isabella.khan@example.com",
                        ClientFolderUrl = "https://files.movementunlimited.com/clients/isabella-khan",
                        IsArchived = false
                    },
                    new Client
                    {
                        FirstName = "Ethan",
                        LastName = "Moore",
                        DOB = new DateTime(1987,9,1),
                        Phone = "5815550110",
                        Email = "ethan.moore@example.com",
                        ClientFolderUrl = "https://files.movementunlimited.com/clients/ethan-moore",
                        IsArchived = false
                    },
                    new Client
                    {
                        FirstName = "Amelia",
                        LastName = "Young",
                        DOB = new DateTime(1994,4,26),
                        Phone = "5875550111",
                        Email = "amelia.young@example.com",
                        ClientFolderUrl = "https://files.movementunlimited.com/clients/amelia-young",
                        IsArchived = true
                    },
                    new Client
                    {
                        FirstName = "James",
                        LastName = "Diaz",
                        DOB = new DateTime(1989,10,20),
                        Phone = "6045550112",
                        Email = "james.diaz@example.com",
                        ClientFolderUrl = "https://files.movementunlimited.com/clients/james-diaz",
                        IsArchived = true
                    },
                    new Client
                    {
                        FirstName = "Charlotte",
                        LastName = "Singh",
                        DOB = new DateTime(1997,3,6),
                        Phone = "6135550113",
                        Email = "charlotte.singh@example.com",
                        ClientFolderUrl = "https://files.movementunlimited.com/clients/charlotte-singh",
                        IsArchived = false
                    },
                    new Client
                    {
                        FirstName = "Benjamin",
                        LastName = "Chen",
                        DOB = new DateTime(1990,8,8),
                        Phone = "6225550114",
                        Email = "benjamin.chen@example.com",
                        ClientFolderUrl = "https://files.movementunlimited.com/clients/benjamin-chen",
                        IsArchived = false
                    },
                    new Client
                    {
                        FirstName = "Harper",
                        LastName = "Ali",
                        DOB = new DateTime(1992,5,18),
                        Phone = "6475550115",
                        Email = "harper.ali@example.com",
                        ClientFolderUrl = "https://files.movementunlimited.com/clients/harper-ali",
                        IsArchived = false
                    },
                    new Client
                    {
                        FirstName = "Henry",
                        LastName = "Brown",
                        DOB = new DateTime(1986,2,9),
                        Phone = "6725550116",
                        Email = "henry.brown@example.com",
                        ClientFolderUrl = "https://files.movementunlimited.com/clients/henry-brown",
                        IsArchived = false
                    },
                    new Client
                    {
                        FirstName = "Evelyn",
                        LastName = "Patel",
                        DOB = new DateTime(1993,7,30),
                        Phone = "7055550117",
                        Email = "evelyn.patel@example.com",
                        ClientFolderUrl = "https://files.movementunlimited.com/clients/evelyn-patel",
                        IsArchived = false
                    },
                    new Client
                    {
                        FirstName = "Daniel",
                        LastName = "Gomez",
                        DOB = new DateTime(1984,6,12),
                        Phone = "7425550118",
                        Email = "daniel.gomez@example.com",
                        ClientFolderUrl = "https://files.movementunlimited.com/clients/daniel-gomez",
                        IsArchived = false
                    },
                    new Client
                    {
                        FirstName = "Abigail",
                        LastName = "Foster",
                        DOB = new DateTime(1998,1,5),
                        Phone = "7785550119",
                        Email = "abigail.foster@example.com",
                        ClientFolderUrl = "https://files.movementunlimited.com/clients/abigail-foster",
                        IsArchived = true
                    },
                    new Client
                    {
                        FirstName = "Jack",
                        LastName = "Murphy",
                        DOB = new DateTime(1982,11,28),
                        Phone = "8075550120",
                        Email = "jack.murphy@example.com",
                        ClientFolderUrl = "https://files.movementunlimited.com/clients/jack-murphy",
                        IsArchived = false
                    }
                };

                // === Generate ~40 additional clients ===
                var rnd = new Random();
                string[] firstNames = { "Alex", "Jordan", "Taylor", "Riley", "Casey", "Morgan", "Quinn", "Sydney", "Rowan", "Parker" };
                string[] lastNames = { "Green", "Lee", "Park", "Kim", "Nguyen", "Patel", "Singh", "Brown", "Hughes", "Shaw" };

                int basePhone = 9000; // just to keep phones unique-ish
                int extraCount = 40;

                for (int i = 0; i < extraCount; i++)
                {
                    var fn = firstNames[rnd.Next(firstNames.Length)];
                    var ln = lastNames[rnd.Next(lastNames.Length)];

                    var phoneNumber = $"905555{(basePhone + i):D4}";
                    var email = $"{fn.ToLower()}.{ln.ToLower()}{i}@example.com";

                    var extraClient = new Client
                    {
                        FirstName = fn,
                        LastName = ln,
                        DOB = DateTime.Today.AddYears(-rnd.Next(18, 75)).AddDays(-rnd.Next(365)),
                        Phone = phoneNumber,
                        Email = email,
                        ClientFolderUrl = $"https://files.movementunlimited.com/clients/{fn.ToLower()}-{ln.ToLower()}-{i}",
                        // About 20% of extra clients start archived
                        IsArchived = rnd.NextDouble() < 0.2
                    };

                    clients.Add(extraClient);
                }

                context.Clients.AddRange(clients);
                context.SaveChanges();
            }

            // Sessions if empty
            if (!context.Sessions.Any())
            {
                // Seed a core set of example sessions
                var sessions = new List<Session>
                {
                    new Session { SessionDate = new DateTime(2025,2,1),  SessionsPerWeekRecommended = 2, TrainerID = 1, ClientID = 1,  IsArchived = false },
                    new Session { SessionDate = new DateTime(2025,2,2),  SessionsPerWeekRecommended = 2, TrainerID = 2, ClientID = 2,  IsArchived = false },
                    new Session { SessionDate = new DateTime(2025,2,3),  SessionsPerWeekRecommended = 3, TrainerID = 3, ClientID = 3,  IsArchived = false },
                    new Session { SessionDate = new DateTime(2025,2,4),  SessionsPerWeekRecommended = 1, TrainerID = 1, ClientID = 4,  IsArchived = false },
                    new Session { SessionDate = new DateTime(2025,2,5),  SessionsPerWeekRecommended = 2, TrainerID = 2, ClientID = 5,  IsArchived = false },
                    new Session { SessionDate = new DateTime(2025,2,6),  SessionsPerWeekRecommended = 1, TrainerID = 3, ClientID = 6,  IsArchived = false },
                    new Session { SessionDate = new DateTime(2025,2,7),  SessionsPerWeekRecommended = 2, TrainerID = 1, ClientID = 7,  IsArchived = false },
                    new Session { SessionDate = new DateTime(2025,2,8),  SessionsPerWeekRecommended = 2, TrainerID = 2, ClientID = 8,  IsArchived = false },
                    new Session { SessionDate = new DateTime(2025,2,9),  SessionsPerWeekRecommended = 3, TrainerID = 3, ClientID = 9,  IsArchived = false },
                    new Session { SessionDate = new DateTime(2025,2,10), SessionsPerWeekRecommended = 1, TrainerID = 1, ClientID = 10, IsArchived = false },
                    new Session { SessionDate = new DateTime(2025,2,11), SessionsPerWeekRecommended = 2, TrainerID = 2, ClientID = 11, IsArchived = false },
                    new Session { SessionDate = new DateTime(2025,2,12), SessionsPerWeekRecommended = 2, TrainerID = 3, ClientID = 12, IsArchived = false }
                };

                // Additional sessions provide more realistic volume across clients/trainers
                var rnd = new Random();
                var allClientIds = context.Clients.Select(c => c.ID).ToArray();
                var allTrainerIds = context.Trainers.Select(t => t.ID).ToArray();

                // Generate about 24 more sessions on different dates
                for (int i = 0; i < 24; i++)
                {
                    var clientId = allClientIds[rnd.Next(allClientIds.Length)];
                    var trainerId = allTrainerIds[rnd.Next(allTrainerIds.Length)];

                    var session = new Session
                    {
                        SessionDate = DateTime.Today.AddDays(rnd.Next(-30, 60)),
                        SessionsPerWeekRecommended = rnd.Next(1, 4),
                        TrainerID = trainerId,
                        ClientID = clientId,
                        // Small percentage start archived to exercise archive logic
                        IsArchived = rnd.NextDouble() < 0.1
                    };

                    sessions.Add(session);
                }

                context.Sessions.AddRange(sessions);
                context.SaveChanges();
            }

            // SessionNotes
            if (!context.SessionNotes.Any())
            {
                context.SessionNotes.AddRange(
                    new SessionNotes { SessionID = 1, GeneralComments = "New client; explain apparatus safety.", SubjectiveReports = "3/10 shoulder pain.", ObjectiveFindings = "Limited ROM on left shoulder.", Plan = "Mobility + core stability." },
                    new SessionNotes { SessionID = 2, GeneralComments = "Prefers mornings.", SubjectiveReports = "Knee stable.", ObjectiveFindings = "Good squat mechanics.", Plan = "Progress load carefully." },
                    new SessionNotes { SessionID = 3, GeneralComments = "Asthma managed.", SubjectiveReports = "Breathing ok.", ObjectiveFindings = "Normal vitals.", Plan = "Endurance and posture." },
                    new SessionNotes { SessionID = 4, GeneralComments = "Desk job; back stiffness.", SubjectiveReports = "Tight lower back.", ObjectiveFindings = "Tight hamstrings.", Plan = "Hip hinge patterning." },
                    new SessionNotes { SessionID = 5, GeneralComments = "Very motivated.", SubjectiveReports = "Wants strength.", ObjectiveFindings = "Baseline strength ok.", Plan = "Progressive overload." },
                    new SessionNotes { SessionID = 6, GeneralComments = string.Empty, SubjectiveReports = "Occasional ankle soreness.", ObjectiveFindings = "Slight instability.", Plan = "Balance + ankle stability." },
                    new SessionNotes { SessionID = 7, GeneralComments = string.Empty, SubjectiveReports = "Sleep improved.", ObjectiveFindings = "Better thoracic mobility.", Plan = "Introduce loaded carries." },
                    new SessionNotes { SessionID = 8, GeneralComments = "Hypertension controlled.", SubjectiveReports = "Energy low.", ObjectiveFindings = "BP safe range.", Plan = "Longer warm-up; intervals." },
                    new SessionNotes { SessionID = 9, GeneralComments = string.Empty, SubjectiveReports = "No pain today.", ObjectiveFindings = "Asymmetry reduced.", Plan = "Eccentric hamstring work." },
                    new SessionNotes { SessionID = 10, GeneralComments = string.Empty, SubjectiveReports = "Stressed week.", ObjectiveFindings = "Tension in traps.", Plan = "Downregulate, breathing." },
                    new SessionNotes { SessionID = 11, GeneralComments = string.Empty, SubjectiveReports = "Ready for test.", ObjectiveFindings = "Strong core activation.", Plan = "Advance series." },
                    new SessionNotes { SessionID = 12, GeneralComments = string.Empty, SubjectiveReports = "Mild DOMS.", ObjectiveFindings = "Good recovery.", Plan = "Maintain volume." }
                );
                context.SaveChanges();
            }

            // AdminStatus
            if (!context.AdminStatuses.Any())
            {
                context.AdminStatuses.AddRange(
                    new AdminStatus { SessionID = 1, IsPaid = false, AdminNotes = "Intake complete.", AdminInitials = "AG", NextAppointmentBooked = true, CommunicatedProgress = false, ReadyToProgress = false, CourseCorrectionNeeded = false, TeamConsult = false, ReferredExternally = false, ReferredTo = null },
                    new AdminStatus { SessionID = 2, IsPaid = true, AdminNotes = "Completed.", AdminInitials = "JL", NextAppointmentBooked = true, CommunicatedProgress = true, ReadyToProgress = true, CourseCorrectionNeeded = false, TeamConsult = false, ReferredExternally = false, ReferredTo = null },
                    new AdminStatus { SessionID = 3, IsPaid = true, AdminNotes = "Completed.", AdminInitials = "MP", NextAppointmentBooked = true, CommunicatedProgress = true, ReadyToProgress = true, CourseCorrectionNeeded = false, TeamConsult = false, ReferredExternally = false, ReferredTo = null },
                    new AdminStatus { SessionID = 4, IsPaid = false, AdminNotes = "Cancelled.", AdminInitials = "AG", NextAppointmentBooked = false, CommunicatedProgress = false, ReadyToProgress = false, CourseCorrectionNeeded = false, TeamConsult = false, ReferredExternally = false, ReferredTo = null },
                    new AdminStatus { SessionID = 5, IsPaid = false, AdminNotes = "Scheduled.", AdminInitials = "JL", NextAppointmentBooked = true, CommunicatedProgress = false, ReadyToProgress = false, CourseCorrectionNeeded = false, TeamConsult = false, ReferredExternally = false, ReferredTo = null },
                    new AdminStatus { SessionID = 6, IsPaid = false, AdminNotes = "No-show invoiced.", AdminInitials = "MP", NextAppointmentBooked = false, CommunicatedProgress = false, ReadyToProgress = false, CourseCorrectionNeeded = false, TeamConsult = false, ReferredExternally = false, ReferredTo = null },
                    new AdminStatus { SessionID = 7, IsPaid = true, AdminNotes = "Completed.", AdminInitials = "AG", NextAppointmentBooked = true, CommunicatedProgress = true, ReadyToProgress = true, CourseCorrectionNeeded = false, TeamConsult = false, ReferredExternally = false, ReferredTo = null },
                    new AdminStatus { SessionID = 8, IsPaid = false, AdminNotes = "Scheduled.", AdminInitials = "JL", NextAppointmentBooked = true, CommunicatedProgress = false, ReadyToProgress = false, CourseCorrectionNeeded = false, TeamConsult = false, ReferredExternally = false, ReferredTo = null },
                    new AdminStatus { SessionID = 9, IsPaid = true, AdminNotes = "Completed.", AdminInitials = "MP", NextAppointmentBooked = true, CommunicatedProgress = true, ReadyToProgress = true, CourseCorrectionNeeded = false, TeamConsult = false, ReferredExternally = false, ReferredTo = null },
                    new AdminStatus { SessionID = 10, IsPaid = true, AdminNotes = "Completed.", AdminInitials = "AG", NextAppointmentBooked = true, CommunicatedProgress = true, ReadyToProgress = true, CourseCorrectionNeeded = false, TeamConsult = false, ReferredExternally = false, ReferredTo = null },
                    new AdminStatus { SessionID = 11, IsPaid = false, AdminNotes = "Scheduled.", AdminInitials = "JL", NextAppointmentBooked = true, CommunicatedProgress = false, ReadyToProgress = false, CourseCorrectionNeeded = false, TeamConsult = false, ReferredExternally = false, ReferredTo = null },
                    new AdminStatus { SessionID = 12, IsPaid = false, AdminNotes = "Scheduled.", AdminInitials = "MP", NextAppointmentBooked = true, CommunicatedProgress = false, ReadyToProgress = false, CourseCorrectionNeeded = false, TeamConsult = false, ReferredExternally = false, ReferredTo = null }
                );
                context.SaveChanges();
            }

            // SessionExercises (2 per session)
            if (!context.SessionExercises.Any())
            {
                context.SessionExercises.AddRange(
                    new SessionExercise { SessionID = 1, ExerciseID = 1, Springs = "3R", Props = string.Empty, Notes = "Neutral pelvis focus." },
                    new SessionExercise { SessionID = 1, ExerciseID = 2, Springs = "2R", Props = "Straps", Notes = "Breathing emphasis." },
                    new SessionExercise { SessionID = 2, ExerciseID = 3, Springs = string.Empty, Props = "Ball", Notes = "Knee alignment." },
                    new SessionExercise { SessionID = 2, ExerciseID = 4, Springs = string.Empty, Props = string.Empty, Notes = "Glute activation." },
                    new SessionExercise { SessionID = 3, ExerciseID = 5, Springs = string.Empty, Props = string.Empty, Notes = string.Empty },
                    new SessionExercise { SessionID = 3, ExerciseID = 6, Springs = "1R", Props = "Straps", Notes = string.Empty },
                    new SessionExercise { SessionID = 4, ExerciseID = 7, Springs = string.Empty, Props = string.Empty, Notes = "Lumbar mobility." },
                    new SessionExercise { SessionID = 5, ExerciseID = 8, Springs = "1R", Props = "Box", Notes = "Control tempo." },
                    new SessionExercise { SessionID = 6, ExerciseID = 9, Springs = string.Empty, Props = "Pad", Notes = "Ankle stability." },
                    new SessionExercise { SessionID = 7, ExerciseID = 10, Springs = string.Empty, Props = string.Empty, Notes = string.Empty },
                    new SessionExercise { SessionID = 8, ExerciseID = 11, Springs = "1R", Props = string.Empty, Notes = string.Empty },
                    new SessionExercise { SessionID = 9, ExerciseID = 12, Springs = string.Empty, Props = string.Empty, Notes = string.Empty },
                    new SessionExercise { SessionID = 10, ExerciseID = 1, Springs = "3R", Props = string.Empty, Notes = string.Empty },
                    new SessionExercise { SessionID = 10, ExerciseID = 2, Springs = "2R", Props = "Straps", Notes = string.Empty },
                    new SessionExercise { SessionID = 11, ExerciseID = 3, Springs = string.Empty, Props = "Ball", Notes = string.Empty },
                    new SessionExercise { SessionID = 11, ExerciseID = 4, Springs = string.Empty, Props = string.Empty, Notes = string.Empty },
                    new SessionExercise { SessionID = 12, ExerciseID = 5, Springs = string.Empty, Props = string.Empty, Notes = string.Empty },
                    new SessionExercise { SessionID = 12, ExerciseID = 6, Springs = "1R", Props = "Straps", Notes = string.Empty },
                    new SessionExercise { SessionID = 8, ExerciseID = 1, Springs = "3R", Props = string.Empty, Notes = string.Empty },
                    new SessionExercise { SessionID = 9, ExerciseID = 2, Springs = "2R", Props = "Straps", Notes = string.Empty },
                    new SessionExercise { SessionID = 6, ExerciseID = 3, Springs = string.Empty, Props = "Ball", Notes = string.Empty },
                    new SessionExercise { SessionID = 7, ExerciseID = 4, Springs = string.Empty, Props = string.Empty, Notes = string.Empty },
                    new SessionExercise { SessionID = 5, ExerciseID = 5, Springs = string.Empty, Props = string.Empty, Notes = string.Empty },
                    new SessionExercise { SessionID = 4, ExerciseID = 6, Springs = "1R", Props = "Straps", Notes = string.Empty }
                );
                context.SaveChanges();
            }
        }
    }
}