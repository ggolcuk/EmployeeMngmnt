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
using System.Windows;

namespace EmployeeManagement.ViewModels
{
    public class MainViewModel : ViewModel
    {
        private readonly ApiService apiService;
        private ObservableCollection<Employee> users;

        public ObservableCollection<Employee> Users
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
            apiService.ApiErrorOccurred += HandleApiError;
        }

        private async void OnLoadUsers(object parameter)
        {
            Users = new ObservableCollection<Employee>(await apiService.GetEmployeesAsync());
        }

        private void HandleApiError(string errorMessage)
        {
            // Show the error message in a MessageBox or some other UI element
            MessageBox.Show(errorMessage, "API Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }



    }
}
