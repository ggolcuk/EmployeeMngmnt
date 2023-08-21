using Common.Utilities.EnumUtilities;
using Common.WPF.WPFUtilities;
using EmployeeManagement.Models;
using EmployeeManagement.Services;
using EmployeeManagement.Views;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace EmployeeManagement.ViewModels
{
    public class ListViewViewModel : ViewModel
    {
        private readonly IApiService _apiService;

        public ObservableCollection<EmployeeViewModel> Employees { get; }
        public SearchParametersViewModel SearchParametersVM { get; }

        public IList<GenderType> AllGenderOptions { get; }
        public IList<StatusType> AllStatusOptions { get; }


        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ExportAllCommand { get; }
        public ICommand ExportSearchCommand { get; }
        public IApiService ApiService
        {
            get { return _apiService; }
        }


        public ListViewViewModel(IApiService apiService)
        {
            _apiService = apiService ?? throw new ArgumentNullException(nameof(ApiService));
            Employees = new ObservableCollection<EmployeeViewModel>();

            SearchParametersVM = new SearchParametersViewModel(new SearchParameters(), this);


            UpdateCommand = new DelegatingCommand(CreateOrUpdate);
            DeleteCommand = new DelegatingCommand(Delete);

            ExportAllCommand = new DelegatingCommand(ExportAllToTxt);
            ExportSearchCommand = new DelegatingCommand(ExportFilteredToTxt);

            AllGenderOptions = new GenderType[] { GenderType.Female, GenderType.Male };
            AllStatusOptions = new StatusType[] { StatusType.Active, StatusType.Inactive};

            SearchParametersVM.Refresh(null);

        }

        internal async void ExportAllToTxt(object parameter)
        {
            var loadingMessageWindow = new LoadingMessageWindow();
            loadingMessageWindow.Show(); // won't block the UI
            await ExportEmployeesToTxTAsync(false);
            loadingMessageWindow.Close();
        }

        internal async void ExportFilteredToTxt(object parameter)
        {
            var loadingMessageWindow = new LoadingMessageWindow();
            loadingMessageWindow.Show(); // won't block the UI

            await ExportEmployeesToTxTAsync(true);

            loadingMessageWindow.Close();
        }

        public async Task ExportEmployeesToTxTAsync(bool useSearchParameters)
        {

           
            List<Employee> allEmployees = new List<Employee>();
            int currentPage = 1;
            int perPage = 100; // Adjust the perPage value as needed

            while (true)
            {
                var tempSearchParameters = useSearchParameters ? SearchParametersVM.Instance.Clone() : new SearchParameters { page = currentPage, perPage = perPage };
                tempSearchParameters.page = currentPage;
                tempSearchParameters.perPage = perPage;

               
                var employees = await ApiService.GetEmployeesAsync(tempSearchParameters);

                if (employees == null || employees.Count == 0)
                {
                    break;
                }

                allEmployees.AddRange(employees);
                currentPage++;
            }

            if (allEmployees.Count > 0)
            {
                // Call method to save employees to TXT file
                SaveEmployeesToTxt(allEmployees);
            }
        }

        private void SaveEmployeesToTxt(List<Employee> employees)
        {
            if (employees == null || employees.Count == 0)
            {
                MessageBox.Show("No data to print.");
                return;
            }
                

            // Create a SaveFileDialog to allow the user to choose the file location
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                Title = "Save Employees to Text File",
                FileName = "EmployeeData.txt"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                using (var writer = new StreamWriter(saveFileDialog.FileName))
                {
                    // Write the property names as the first line
                    var propertyNames = typeof(Employee).GetProperties().Select(property => property.Name);
                    writer.WriteLine(string.Join("\t", propertyNames));

                    // Write employee data
                    foreach (var employee in employees)
                    {
                        var values = typeof(Employee).GetProperties().Select(property =>
                        {
                            var value = property.GetValue(employee);
                            return value != null ? value.ToString() : string.Empty;
                        });
                        writer.WriteLine(string.Join("\t", values));
                    }
                }

                MessageBox.Show("Employees data saved to the text file.");
            }
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
                if (!apiEmployee.IsEqual(employeeViewModel.OriginalEmployee))
                {
                    var result = DialogService.ShowDialog("Select Okay to continue editing, Cancel to refresh the list", "Selected employee parameters already changed by someone else.", MessageBoxButton.OKCancel);
                    if (result == DialogResult.OK)
                    {
                        await UpdateAsync(employeeViewModel);

                    }
                    else if (result == DialogResult.Cancel)
                    {
                        //Refresh the list with search parameters
                        await SearchParametersVM.SearchAsync();
                    }
                }
            }

            await UpdateAsync(employeeViewModel);
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


