using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using Lab_Humeniuk.Models;
using CommunityToolkit.Mvvm.Input;
using Lab_Humeniuk.Exceptions;


namespace Lab_Humeniuk
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string firstName;
        private string lastName;
        private string email;
        private DateTime dateOfBirth = DateTime.Now;
        private string result;

        public string FirstName
        {
            get => firstName;
            set
            {
                firstName = value;
                OnPropertyChanged(nameof(FirstName));
                OnPropertyChanged(nameof(CanProceed));
                ((AsyncRelayCommand)ProceedCommand).NotifyCanExecuteChanged();
            }
        }

        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
                OnPropertyChanged(nameof(LastName));
                OnPropertyChanged(nameof(CanProceed));
                ((AsyncRelayCommand)ProceedCommand).NotifyCanExecuteChanged();
            }
        }

        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged(nameof(Email));
                OnPropertyChanged(nameof(CanProceed));
                ((AsyncRelayCommand)ProceedCommand).NotifyCanExecuteChanged();
            }
        }

        public DateTime DateOfBirth
        {
            get => dateOfBirth;
            set
            {
                dateOfBirth = value;
                OnPropertyChanged(nameof(DateOfBirth));
                OnPropertyChanged(nameof(CanProceed));
                ((AsyncRelayCommand)ProceedCommand).NotifyCanExecuteChanged();
            }
        }

        public string Result
        {
            get => result;
            set
            {
                result = value;
                OnPropertyChanged(nameof(Result));
            }
        }

        public bool CanProceed => !string.IsNullOrWhiteSpace(FirstName) &&
                                  !string.IsNullOrWhiteSpace(LastName) &&
                                  !string.IsNullOrWhiteSpace(Email) &&
                                  DateOfBirth != DateTime.MinValue;

        public ICommand ProceedCommand { get; }

        public MainViewModel()
        {
            ProceedCommand = new AsyncRelayCommand(ProceedAsync, () => CanProceed);
        }

        private async Task ProceedAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    var person = new Person(FirstName, LastName, Email, DateOfBirth);

                    if (person.IsBirthday)
                    {
                        MessageBox.Show("З Днем Народження!");
                    }

                    Result = $"Ім'я: {person.FirstName}\nПрізвище: {person.LastName}\nEmail: {person.Email}\nДата народження: {person.BirthDate}\n" +
                             $"Повнолітній: {person.IsAdult}\nЗнак зодіаку: {person.SunSign}\nКитайський знак: {person.ChineseSign}\nДень народження сьогодні: {person.IsBirthday}";
                }
                catch (FutureDateBirthException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (MoreThan135YOException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (InvalidEmailException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (UnknownDataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Сталася помилка: " + ex.Message);
                }
            });
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}