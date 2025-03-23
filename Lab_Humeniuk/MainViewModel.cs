using System.ComponentModel;
using System.Windows.Input;
using System.Windows;

namespace Lab_Humeniuk
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private DateTime? _birthDate;
        private string _ageText;
        private string _westernZodiac;
        private string _chineseZodiac;

        public event PropertyChangedEventHandler PropertyChanged;

        public DateTime? BirthDate
        {
            get => _birthDate;
            set
            {
                _birthDate = value;
                OnPropertyChanged(nameof(BirthDate));
            }
        }

        public string AgeText
        {
            get => _ageText;
            set
            {
                _ageText = value;
                OnPropertyChanged(nameof(AgeText));
            }
        }

        public string WesternZodiac
        {
            get => _westernZodiac;
            set
            {
                _westernZodiac = value;
                OnPropertyChanged(nameof(WesternZodiac));
            }
        }

        public string ChineseZodiac
        {
            get => _chineseZodiac;
            set
            {
                _chineseZodiac = value;
                OnPropertyChanged(nameof(ChineseZodiac));
            }
        }

        public void Calculate()
        {
            if (BirthDate == null)
            {
                MessageBox.Show("Будь ласка, введіть дату народження.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime birthDate = BirthDate.Value;
            int age = DateTime.Now.Year - birthDate.Year;
            if (birthDate.Date > DateTime.Now.AddYears(-age)) age--;

            if (age < 0 || age > 135)
            {
                MessageBox.Show("Введено некоректний вік.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            AgeText = $"Ваш вік: {age} років";
            WesternZodiac = $"Західний знак: {GetWesternZodiac(birthDate)}";
            ChineseZodiac = $"Китайський знак: {GetChineseZodiac(birthDate.Year)}";

            if (birthDate.Month == DateTime.Now.Month && birthDate.Day == DateTime.Now.Day)
            {
                MessageBox.Show("Вітаємо з Днем Народження! 🎉", "Святкуємо!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private string GetWesternZodiac(DateTime birthDate)
        {
            int day = birthDate.Day, month = birthDate.Month;
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

        private string GetChineseZodiac(int year)
        {
            string[] animals = { "Мавпа", "Півень", "Собака", "Свиня", "Щур", "Бик", "Тигр", "Кролик", "Дракон", "Змія", "Кінь", "Коза" };
            return animals[year % 12];
        }

        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}