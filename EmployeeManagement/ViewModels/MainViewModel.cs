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
        private readonly ApiService _apiService;
        private ObservableCollection<Employee> _employees;

        public ICommand LoadUsersCommand { get; }
        public ICommand CreateEmployeeCommand { get; }
        public ICommand UpdateEmployeeCommand { get; }
        public ICommand DeleteEmployeeCommand { get; }

        public ObservableCollection<Employee> Employees
        {
            get { return _employees; }
            set
            {
                _employees = value;
                OnPropertyChanged(nameof(Employees));
            }
        }

        public MainViewModel()
        {
            _apiService = new ApiService(new DefaultHttpClient());
            _apiService.ApiErrorOccurred += HandleApiError;

            LoadUsersCommand = new DelegatingCommand(OnLoadUsers);
            CreateEmployeeCommand = new DelegatingCommand(OnCreateEmployee);
            UpdateEmployeeCommand = new DelegatingCommand(OnUpdateEmployee);
            DeleteEmployeeCommand = new DelegatingCommand(OnDeleteEmployee);

            LoadEmployeesAsync();
        }

        #region Api service methods

        public async Task LoadEmployeesAsync()
        {
            Employees = new ObservableCollection<Employee>(
                await _apiService.GetAllEmployeesAsync()
            );
        }

        public async Task CreateEmployeeAsync(Employee employee)
        {
            var createdEmployee = await _apiService.CreateEmployeeAsync(employee);
            if (createdEmployee != null)
            {
                Employees.Add(createdEmployee);
            }
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            var updatedEmployee = await _apiService.UpdateEmployeeAsync(employee.Id, employee);
            if (updatedEmployee != null)
            {
                // Find the existing employee and update its properties
                var existingEmployee = Employees.FirstOrDefault(e => e.Id == updatedEmployee.Id);
                if (existingEmployee != null)
                {
                    existingEmployee.Name = updatedEmployee.Name;
                    existingEmployee.Email = updatedEmployee.Email;
                    existingEmployee.Gender = updatedEmployee.Gender;
                    existingEmployee.Status = updatedEmployee.Status;
                }
            }
        }

        public async Task DeleteEmployeeAsync(Employee employee)
        {
            var isDeleted = await _apiService.DeleteEmployeeAsync(employee.Id);
            if (isDeleted)
            {
                Employees.Remove(employee);
            }
        }

        #endregion

        #region Command handlers

        private async void OnLoadUsers(object parameter)
        {
            await LoadEmployeesAsync();
        }

        private async void OnCreateEmployee(object parameter)
        {

            //template
            var newEmployee = new Employee
            {
                Name = "New Employee",
                Email = "newemployee@example.com",
                Gender = "Male",
                Status = "Active"
            };

            await CreateEmployeeAsync(newEmployee);
        }

        private async void OnUpdateEmployee(object parameter)
        {

            //template
            if (parameter is Employee employee)
            {
                // Simulate updating employee's properties
                employee.Name += " Updated";
                employee.Email = "updated@example.com";
                employee.Gender = "Female";

                await UpdateEmployeeAsync(employee);
            }
        }

        private async void OnDeleteEmployee(object parameter)
        {
            if (parameter is Employee employee)
            {
                await DeleteEmployeeAsync(employee);
            }
        }

        #endregion

        private void HandleApiError(string errorMessage)
        {
            MessageBox.Show(errorMessage, "API Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        #region IDataErrorInfo Members

        public override string GetValidationError(string propertyName)
        {
            // Placeholder validation logic
            return null;
        }

        #endregion

        #region Local OnpropertyChange

        public override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
        }

        #endregion
    }


}

