using Lab_Humeniuk.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lab_Humeniuk.Services
{
    public static  class PersonService
    {
        private static readonly string FilePath = "persons.json";

        public static List<Person> Load()
        {
            if (!File.Exists(FilePath))
            {
                return GeneratePersons();
            }

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Person>>(json);
        }

        public static void Save(List<Person> people)
        {
            var json = JsonSerializer.Serialize(people, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }

        private static List<Person> GeneratePersons()
        {
            var random = new Random();
            var people = new List<Person>();
            for (int i = 0; i < 50; i++)
            {
                var first = $"Name{i}";
                var last = $"Last{i}";
                var email = $"user{i}@mail.com";
                var date = DateTime.Now.AddYears(-random.Next(10, 100)).AddDays(random.Next(-365, 365));
                people.Add(new Person(first, last, email, date));
            }

            Save(people);
            return people;
        }
    }
}
