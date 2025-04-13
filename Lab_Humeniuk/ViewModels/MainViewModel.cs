using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using Lab_Humeniuk.Models;
using CommunityToolkit.Mvvm.Input;
using Lab_Humeniuk.Exceptions;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Data;
using Lab_Humeniuk.Services;
using System.Text.Json;
using System.IO;
using System.Runtime.CompilerServices;


namespace Lab_Humeniuk.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Person> _users;
        private ObservableCollection<Person> _filteredUsers;
        private string _filterText;
        private string _firstName;
        private string _lastName;
        private string _email;
        private DateTime? _dateOfBirth;
        private string _selectedSortProperty;
        private Person _selectedPerson;

        public ObservableCollection<Person> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged();
                FilterUsers();
            }
        }

        public ObservableCollection<Person> FilteredUsers
        {
            get => _filteredUsers;
            set
            {
                _filteredUsers = value;
                OnPropertyChanged();
            }
        }

        public string FilterText
        {
            get => _filterText;
            set
            {
                _filterText = value;
                OnPropertyChanged();
                FilterUsers();
            }
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged();
                CheckCanProceed();
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged();
                CheckCanProceed();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
                CheckCanProceed();
            }
        }

        public DateTime? DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                _dateOfBirth = value;
                OnPropertyChanged();
                CheckCanProceed();
            }
        }

        public string SelectedSortProperty
        {
            get => _selectedSortProperty;
            set
            {
                _selectedSortProperty = value;
                OnPropertyChanged();
            }
        }

        public List<string> SortProperties { get; } = new List<string>
    {
        "FirstName", "LastName", "Email", "BirthDate", "IsAdult", "IsBirthday", "SunSign", "ChineseSign"
    };

        public RelayCommand ProceedCommand { get; }
        public RelayCommand SortCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand<Person> DeleteCommand { get; }

        public MainViewModel()
        {
            Users = new ObservableCollection<Person>(PersonService.Load());
            FilterUsers();

            ProceedCommand = new RelayCommand(Proceed, CanProceed);
            SortCommand = new RelayCommand(SortUsers);
            SaveCommand = new RelayCommand(SaveUsers);
            DeleteCommand = new RelayCommand<Person>(DeleteUser);
        }

        private void Proceed()
        {
            try
            {
                var newUser = new Person(FirstName, LastName, Email, DateOfBirth);
                Users.Add(newUser);
                ClearInputFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool CanProceed()
        {
            return !string.IsNullOrWhiteSpace(FirstName) &&
                   !string.IsNullOrWhiteSpace(LastName) &&
                   !string.IsNullOrWhiteSpace(Email) &&
                   DateOfBirth.HasValue;
        }

        private void CheckCanProceed()
        {
            ProceedCommand.NotifyCanExecuteChanged();
        }

        private void FilterUsers()
        {
            if (string.IsNullOrWhiteSpace(FilterText))
            {
                FilteredUsers = Users;
                return;
            }

            FilteredUsers = new ObservableCollection<Person>(
                Users.Where(p => p.FirstName.Contains(FilterText, StringComparison.OrdinalIgnoreCase) ||
                                 p.LastName.Contains(FilterText, StringComparison.OrdinalIgnoreCase) ||
                                 p.Email.Contains(FilterText, StringComparison.OrdinalIgnoreCase))
            );
        }

        private void SortUsers()
        {
            if (string.IsNullOrWhiteSpace(SelectedSortProperty)) return;

            Users = new ObservableCollection<Person>(Users.OrderBy(p => GetPropertyValue(p, SelectedSortProperty)));
        }

        private object GetPropertyValue(Person person, string propertyName)
        {
            return person.GetType().GetProperty(propertyName)?.GetValue(person);
        }

        private void SaveUsers()
        {
            PersonService.Save(Users.ToList());
        }

        private void DeleteUser(Person person)
        {
            Users.Remove(person);
        }

        private void ClearInputFields()
        {
            FirstName = "";
            LastName = "";
            Email = "";
            DateOfBirth = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}