using FitnessCenter.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace FitnessCenter.Data
{
    public static class DbInitializer
    {
        private static readonly Random rand = new Random();

        public static void Initialize(ApplicationDbContext db)
        {
            db.Database.EnsureCreated();

            // если нужно перезаполнять БД каждый раз, можно очистить:
            if (db.Clients.Any())
            {
                db.Visits.RemoveRange(db.Visits);
                db.MembershipSales.RemoveRange(db.MembershipSales);
                db.Clients.RemoveRange(db.Clients);
                db.SaveChanges();
            }

            // ---- Тренеры ----
            var trainers = new[]
            {
                new Trainer { FirstName = "Иван", LastName = "Крутов", Specialty = "Crossfit" },
                new Trainer { FirstName = "Ольга", LastName = "Лайт", Specialty = "Yoga" },
                new Trainer { FirstName = "Анна", LastName = "Соколова", Specialty = "Pilates" },
                new Trainer { FirstName = "Денис", LastName = "Марков", Specialty = "Stretching" }
            };
            db.Trainers.AddRange(trainers);
            db.SaveChanges();

            // ---- Абонементы ----
            var plans = new[]
            {
                new MembershipPlan { Name = "Месячный", Kind = MembershipKind.TimeBased, DurationDays = 30, Price = 80 },
                new MembershipPlan { Name = "Квартальный", Kind = MembershipKind.TimeBased, DurationDays = 90, Price = 210 },
                new MembershipPlan { Name = "Годовой", Kind = MembershipKind.TimeBased, DurationDays = 365, Price = 700 },
            };
            db.MembershipPlans.AddRange(plans);
            db.SaveChanges();

            // ---- Списки имён ----
            string[] firstNames = { "Андрей", "Иван", "Дмитрий", "Павел", "Егор", "Роман", "Максим", "Сергей", "Кирилл", "Алексей", "Мария", "Екатерина", "Кристина", "Ольга", "Дарья", "Анна", "Виктория", "Елизавета", "Юлия" };
            string[] lastNames = { "Орлов", "Петров", "Иванов", "Сидоров", "Смирнов", "Белов", "Ершов", "Куликов", "Федоров", "Жуков", "Морозов", "Кузнецов", "Волков" };

            // ---- Генерация клиентов ----
            var clients = new List<Client>();
            for (int i = 0; i < 1000; i++)
            {
                string first = firstNames[rand.Next(firstNames.Length)];
                string last = lastNames[rand.Next(lastNames.Length)];
                string translitFirst = Transliterate(first);
                string translitLast = Transliterate(last);

                clients.Add(new Client
                {
                    FirstName = first,
                    LastName = last,
                    Gender = rand.Next(2) == 0 ? "M" : "F",
                    BirthDate = RandomDate(new DateTime(1985, 1, 1), new DateTime(2005, 12, 31)),
                    Phone = $"+37529{rand.Next(1000000, 9999999)}",
                    Email = $"{translitFirst.ToLower()}.{translitLast.ToLower()}{i}@example.com",
                    RegistrationDate = DateTime.Now.AddDays(-rand.Next(0, 500))
                });
            }
            db.Clients.AddRange(clients);
            db.SaveChanges();

            // ---- Продажи абонементов ----
            var sales = new List<MembershipSale>();
            foreach (var client in clients)
            {
                var plan = plans[rand.Next(plans.Length)];
                var start = DateTime.Now.AddDays(-rand.Next(0, 300));
                var end = start.AddDays(plan.DurationDays);
                sales.Add(new MembershipSale
                {
                    ClientID = client.ClientID,
                    MembershipPlanID = plan.MembershipPlanID,
                    StartDate = start,
                    EndDate = end
                });
            }
            db.MembershipSales.AddRange(sales);
            db.SaveChanges();

            // ---- Посещения ----
            var visits = new List<Visit>();
            foreach (var client in clients)
            {
                int count = rand.Next(5, 25); // Количество посещений на клиента
                for (int j = 0; j < count; j++)
                {
                    DateTime checkIn = RandomVisitTime();
                    visits.Add(new Visit
                    {
                        ClientID = client.ClientID,
                        CheckInTime = checkIn,
                        CheckOutTime = checkIn.AddHours(rand.Next(1, 3))
                    });
                }
            }

            db.Visits.AddRange(visits);
            db.SaveChanges();
        }

        // --- Вспомогательные методы ---
        private static string Transliterate(string text)
        {
            var map = new Dictionary<char, string>
            {
                ['а'] = "a",
                ['б'] = "b",
                ['в'] = "v",
                ['г'] = "g",
                ['д'] = "d",
                ['е'] = "e",
                ['ё'] = "e",
                ['ж'] = "zh",
                ['з'] = "z",
                ['и'] = "i",
                ['й'] = "y",
                ['к'] = "k",
                ['л'] = "l",
                ['м'] = "m",
                ['н'] = "n",
                ['о'] = "o",
                ['п'] = "p",
                ['р'] = "r",
                ['с'] = "s",
                ['т'] = "t",
                ['у'] = "u",
                ['ф'] = "f",
                ['х'] = "h",
                ['ц'] = "ts",
                ['ч'] = "ch",
                ['ш'] = "sh",
                ['щ'] = "sch",
                ['ъ'] = "",
                ['ы'] = "y",
                ['ь'] = "",
                ['э'] = "e",
                ['ю'] = "yu",
                ['я'] = "ya"
            };

            var sb = new StringBuilder();
            foreach (var c in text.ToLower())
                sb.Append(map.ContainsKey(c) ? map[c] : c.ToString());
            return sb.ToString();
        }

        private static DateTime RandomDate(DateTime from, DateTime to)
        {
            int range = (to - from).Days;
            return from.AddDays(rand.Next(range));
        }

        private static DateTime RandomVisitTime()
        {
            var baseDate = DateTime.Now.AddDays(-rand.Next(0, 90));
            int hour;
            int r = rand.Next(100);
            if (r < 50) hour = rand.Next(6, 11);        // утренние тренировки
            else if (r < 80) hour = rand.Next(12, 17);  // дневные
            else hour = rand.Next(17, 22);              // вечерние
            return new DateTime(baseDate.Year, baseDate.Month, baseDate.Day, hour, rand.Next(0, 60), 0);
        }
    }
}
