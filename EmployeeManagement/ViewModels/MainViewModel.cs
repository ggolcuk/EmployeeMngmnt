using EmployeeManagement.Models;
using EmployeeManagement.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Common.WPF.WPFUtilities;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Windows;
using System.Linq;

namespace EmployeeManagement.ViewModels
{
    public class MainViewModel : ViewModel
    {
        private readonly IApiService _apiService;
        
        private ListViewViewModel _listViewViewModel;


       

        public ListViewViewModel EmployeesListViewModel
        {
            get { return _listViewViewModel; }
            set
            {
                _listViewViewModel = value;
                OnPropertyChanged(nameof(EmployeesListViewModel));
            }
        }


        public IApiService ApiService
        {
            get { return _apiService; }
        }

        public MainViewModel()
        {
            _apiService = new ApiService(new DefaultHttpClient());
            _apiService.ApiErrorOccurred += HandleApiError;

           EmployeesListViewModel = new ListViewViewModel(ApiService);
            
        }

        #region Api service methods


        #endregion

        #region Command handlers

        private async void OnCreateEmployee(object parameter)
        {

            //template
            var newEmployee = new Employee
            {
                name = "New Employee",
                email = "newemployee@example.com",
                gender = "Male",
                status = "Active"
            };

           // await CreateEmployeeAsync(newEmployee);
        }

        

        #endregion

        private void HandleApiError(string errorMessage)
        {
            MessageBox.Show(errorMessage, "API Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }


}

