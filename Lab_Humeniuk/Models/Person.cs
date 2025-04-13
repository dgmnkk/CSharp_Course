using Lab_Humeniuk.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Lab_Humeniuk.Models
{
    [Serializable]
    public class Person
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public DateTime? BirthDate { get; }
        public bool IsAdult { get; }
        public bool IsBirthday { get; }
        public string SunSign { get; }
        public string ChineseSign { get; }

        public Person() { }

        [JsonConstructor]
        public Person(string firstName, string lastName, string email, DateTime? birthDate = null)
        {
            if (!string.IsNullOrWhiteSpace(email) && !IsValidEmail(email))
                throw new InvalidEmailException();

            if (birthDate.HasValue)
            {
                if (birthDate > DateTime.Now)
                    throw new FutureDateBirthException();

                if (CalculateAge(birthDate.Value) > 135)
                    throw new MoreThan135YOException();
            }
            else
            {
                throw new UnknownDataException();
            }

            FirstName = firstName;
            LastName = lastName;
            Email = email;
            BirthDate = birthDate;
            IsAdult = CalculateAge(birthDate.Value) >= 18;
            IsBirthday = birthDate.HasValue &&
                         birthDate.Value.Day == DateTime.Now.Day &&
                         birthDate.Value.Month == DateTime.Now.Month;
            SunSign = GetWesternZodiac(birthDate);
            ChineseSign = GetChineseZodiac(birthDate);
        }
        public Person(string firstName, string lastName, string email)
           : this(firstName, lastName, email, null) { }

        public Person(string firstName, string lastName, DateTime birthDate)
            : this(firstName, lastName, null, birthDate) { }

        private string GetWesternZodiac(DateTime? birthDate)
        {
            if (!birthDate.HasValue) throw new UnknownDataException();

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
            if (!birthDate.HasValue) throw new UnknownDataException();
            string[] animals = { "Мавпа", "Півень", "Собака", "Свиня", "Щур", "Бик", "Тигр", "Кролик", "Дракон", "Змія", "Кінь", "Коза" };
            return animals[birthDate.Value.Year % 12];
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private int CalculateAge(DateTime birthDate)
        {
            int age = DateTime.Now.Year - birthDate.Year;
            if (birthDate > DateTime.Now.AddYears(-age)) age--;
            return age;
        }
    }
}
