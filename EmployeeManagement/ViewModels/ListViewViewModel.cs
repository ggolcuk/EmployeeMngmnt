using Common.WPF.WPFUtilities;
using EmployeeManagement.Models;
using EmployeeManagement.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EmployeeManagement.ViewModels
{
    public class ListViewViewModel : ViewModel
    {
        private readonly IApiService _apiService;
        public ObservableCollection<int> PerPageOptions { get; } = new ObservableCollection<int>
        {
            5, 10, 25, 50, 100
        };

        public ObservableCollection<EmployeeViewModel> Employees { get; }
        public SearchParametersViewModel SearchParametersVM { get; }


        public ICommand RefreshCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public IApiService ApiService
        {
            get { return _apiService; }
        }


        public ListViewViewModel(IApiService apiService)
        {
            _apiService = apiService ?? throw new ArgumentNullException(nameof(ApiService));
            Employees = new ObservableCollection<EmployeeViewModel>();

            SearchParametersVM = new SearchParametersViewModel(new SearchParameters());


            RefreshCommand = new DelegatingCommand(Refresh);
            SearchCommand = new DelegatingCommand(Search);
            AddCommand = new DelegatingCommand(Add);
            UpdateCommand = new DelegatingCommand(CreateOrUpdate);
            DeleteCommand = new DelegatingCommand(Delete);

            Refresh(null);
        }


        private async void Refresh(object parameter)
        {
            this.SearchParametersVM.SetDefault();
            await RefreshAsync();
        }
        private async Task RefreshAsync()
        {
            var employees = await ApiService.GetEmployeesAsync();
            Employees.Clear();
            foreach (var employee in employees)
            {
                Employees.Add(new EmployeeViewModel(employee));
            }
        }


        private async void Search(object parameter)
        {
            await SearchAsync();
        }

        private async Task SearchAsync()
        {
            List<Employee> searchResults = await ApiService.GetEmployeesAsync(SearchParametersVM.Instance);

            // Update the employees collection
            Employees.Clear();
            foreach (var employee in searchResults)
            {
                Employees.Add(new EmployeeViewModel(employee));
            }
        }

        private void Add()
        {
            var newEmployee = new EmployeeViewModel(new Employee()) { IsCreated = false };
            Employees.Insert(0, newEmployee);
           
        }

        private async void CreateOrUpdate(object parameter)
        {
            if (!(parameter is EmployeeViewModel employeeViewModel))
                return;

            if (employeeViewModel.IsCreated)
            {
                await UpdateExistingEmployeeAsync(employeeViewModel);
            }
            else
            {
                await CreateNewEmployeeAsync(employeeViewModel);
            }
        }

        private async Task UpdateExistingEmployeeAsync(EmployeeViewModel employeeViewModel)
        {
            var apiEmployee = await ApiService.GetEmployeeAsync(employeeViewModel.Id);
            
            if (apiEmployee != null)
            {
                if (!apiEmployee.IsEqual(employeeViewModel.MainEmployee))
                {
                    var result = DialogService.ShowDialog("Select Okay to continue editing, Cancel to refresh the list", "Selected employee parameters already changed by someone else.", MessageBoxButton.OKCancel);
                    if (result == DialogResult.OK)
                    {
                        await UpdateAsync(employeeViewModel);

                    }
                    else if (result == DialogResult.Cancel)
                    {
                        //Refresh the list with search parameters
                        await SearchAsync();
                    }
                }
            }
        }

        private async Task CreateNewEmployeeAsync(EmployeeViewModel employeeViewModel)
        {
            employeeViewModel.Update();
            var newEmployee = employeeViewModel.MainEmployee;
            var createdEmployee = await ApiService.CreateEmployeeAsync(newEmployee);

            if (createdEmployee != null)
            {
                employeeViewModel.IsCreated = true;
            }
        }


        private async Task UpdateAsync(EmployeeViewModel employee)
        {
            var result = await ApiService.UpdateEmployeeAsync(employee.Id, employee.MainEmployee);

            if (result == null) //Update the UI accordingly
            {
                employee.Revert();
            }
            else
            {
                employee.Update();
            }
        }


        private async void Delete(object parameter)
        {
            if (!(parameter is EmployeeViewModel employeeViewModel))
                return;

            if (employeeViewModel.IsCreated)
                await DeleteAsync(employeeViewModel);
            else
                Employees.Remove(employeeViewModel);
        }

        private async Task DeleteAsync(EmployeeViewModel employeeViewModel)
        {
            var result = await ApiService.DeleteEmployeeAsync(employeeViewModel.Id);

            if (result)
            {
                Employees.Remove(employeeViewModel);
            }
        }
    }


}


