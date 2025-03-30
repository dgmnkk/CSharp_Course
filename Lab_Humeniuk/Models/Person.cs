using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_Humeniuk.Models
{
    public class Person
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public DateTime? BirthDate { get; }

        public Person(string firstName, string lastName, string email, DateTime? birthDate = null)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            BirthDate = birthDate;
        }
        public Person(string firstName, string lastName, string email)
           : this(firstName, lastName, email, null) { }

        public Person(string firstName, string lastName, DateTime birthDate)
            : this(firstName, lastName, null, birthDate) { }

        public int Age => BirthDate.HasValue ? (int)((DateTime.Now - BirthDate.Value).TotalDays / 365.25) : 0;

        public bool IsAdult => Age >= 18;

        public bool IsBirthday => BirthDate.HasValue &&
                                  BirthDate.Value.Day == DateTime.Now.Day &&
                                  BirthDate.Value.Month == DateTime.Now.Month;

        public string SunSign => GetWesternZodiac(BirthDate);
        public string ChineseSign => GetChineseZodiac(BirthDate);

        private string GetWesternZodiac(DateTime? birthDate)
        {
            if (!birthDate.HasValue) return "Невідомо";

            int day = birthDate.Value.Day, month = birthDate.Value.Month;
            return (month, day) switch
            {
                (1, >= 20) or (2, <= 18) => "Водолій",
                (2, >= 19) or (3, <= 20) => "Риби",
                (3, >= 21) or (4, <= 19) => "Овен",
                (4, >= 20) or (5, <= 20) => "Телець",
                (5, >= 21) or (6, <= 20) => "Близнюки",
                (6, >= 21) or (7, <= 22) => "Рак",
                (7, >= 23) or (8, <= 22) => "Лев",
                (8, >= 23) or (9, <= 22) => "Діва",
                (9, >= 23) or (10, <= 22) => "Терези",
                (10, >= 23) or (11, <= 21) => "Скорпіон",
                (11, >= 22) or (12, <= 21) => "Стрілець",
                _ => "Козеріг"
            };
        }

        private string GetChineseZodiac(DateTime? birthDate)
        {
            if (!birthDate.HasValue) return "Невідомо";
            string[] animals = { "Мавпа", "Півень", "Собака", "Свиня", "Щур", "Бик", "Тигр", "Кролик", "Дракон", "Змія", "Кінь", "Коза" };
            return animals[birthDate.Value.Year % 12];
        }
    }
}
