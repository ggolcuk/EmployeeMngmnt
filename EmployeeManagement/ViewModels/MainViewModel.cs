using EmployeeManagement.Models;
using EmployeeManagement.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Common.WPF.WPFUtilities;

namespace EmployeeManagement.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ApiService apiService;
        private ObservableCollection<User> users;

        public ObservableCollection<User> Users
        {
            get => users;
            set
            {
                users = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadUsersCommand { get; }


        public MainViewModel()
        {
            apiService = new ApiService();
            LoadUsersCommand = new DelegatingCommand(OnLoadUsers);
        }

        private async void OnLoadUsers(object parameter)
        {
            Users = new ObservableCollection<User>(await apiService.GetAllUsersAsync());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
