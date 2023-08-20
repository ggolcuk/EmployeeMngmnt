using EmployeeManagement.Models;
using Common.WPF.WPFUtilities;
using System;
using System.Windows.Controls;
using Common.Utilities.EnumUtilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EmployeeManagement.Services;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EmployeeManagement.ViewModels
{
    public class SearchParametersViewModel: ViewModel
    {

        private readonly SearchParameters _searchParameters;
        private readonly ListViewViewModel _parentViewModel;
        private readonly IApiService _apiService;
        public IList<GenderType> FilterGenderOptions { get; }
        public IList<StatusType> FilterStatusOptions { get; }
        public ObservableCollection<int> PerPageOptions { get; } = new ObservableCollection<int>
        {
            10, 25, 50, 100
        };

        public ICommand RefreshCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand AddCommand { get; }

        public IApiService ApiService
        {
            get { return _apiService; }
        }


        public SearchParameters Instance
        {
            get { return _searchParameters; }
        }

        public string Name
        {
            get { return _searchParameters.name; }
            set
            {
                if (_searchParameters.name == value) return;

                _searchParameters.name = value;
                OnPropertyChanged(nameof(Name));
              }
        }

        public string Gender
        {
            get { return _searchParameters.gender; }
            set
            {
                if (_searchParameters.gender == value) return;

                _searchParameters.gender = value;
                OnPropertyChanged(nameof(Gender));
                
            }
        }

        public string Email
        {
            get { return _searchParameters.email; }
            set
            {
                if (_searchParameters.email == value) return;

                _searchParameters.email = value;
                OnPropertyChanged(nameof(Email));
                
            }
        }

        public string Status
        {
            get { return _searchParameters.status; }

            set
            {
                if (_searchParameters.status == value) return;

                _searchParameters.status = value;
                OnPropertyChanged(nameof(Status));
               
            }
        }

        public ListViewViewModel ParentViewModel
        {
            get { return _parentViewModel; }
        }

        public GenderType SelectedGender
        {
            get { return (GenderType)EnumUtility.FromDescription(_searchParameters.gender, typeof(GenderType)); }
            set
            {
                var gender = EnumUtility.GetDescription(value);
                if (Gender == gender) return;
                Gender = gender;
                SelectedGender = value;
                OnPropertyChanged(nameof(SelectedGender));

            }

        }

        public StatusType SelectedStatus
        {
            get { return (StatusType)EnumUtility.FromDescription(_searchParameters.status, typeof(StatusType)); }
            set
            {
                var status = EnumUtility.GetDescription(value);
                if (Status == status) return;
                Status = status;
                SelectedStatus = value;
                OnPropertyChanged(nameof(SelectedStatus));

            }

        }

        public int Page
        {
            get { return _searchParameters.page; }

            set
            {
                if (_searchParameters.page == value) return;

                _searchParameters.page = value;
                OnPropertyChanged(nameof(Page));

            }
        }

        public int PerPage
        {
            get { return _searchParameters.perPage; }
            set
            {
                if (_searchParameters.perPage == value) return;

                _searchParameters.perPage = value;
                OnPropertyChanged(nameof(PerPage));

            }

        }

        public SearchParametersViewModel(SearchParameters sp, ListViewViewModel listViewModel)
        {
            _apiService = listViewModel.ApiService;
            _parentViewModel = listViewModel;
            _searchParameters = sp;
            PerPage = PerPageOptions.FirstOrDefault();

            FilterGenderOptions = new GenderType[] { GenderType.Female, GenderType.Male, GenderType.All };
            FilterStatusOptions = new StatusType[] { StatusType.Active, StatusType.Inactive, StatusType.All };

            SelectedGender = GenderType.All;
            SelectedStatus = StatusType.All;

            RefreshCommand = new DelegatingCommand(Refresh);
            SearchCommand = new DelegatingCommand(Search);
            AddCommand = new DelegatingCommand(Add);

            
        }

        internal void SetDefault()
        {
            _searchParameters.SetDefault();
            OnPropertyChanged(string.Empty);
        }


        internal async void Refresh(object parameter)
        {
            this.SetDefault();
            await RefreshAsync();
        }
        private async Task RefreshAsync()
        {
            var employees = await ApiService.GetEmployeesAsync();
            ParentViewModel.Employees.Clear();
            foreach (var employee in employees)
            {
                ParentViewModel.Employees.Add(new EmployeeViewModel(employee));
            }
        }


        internal async void Search(object parameter)
        {
            await SearchAsync();
        }

        internal async Task SearchAsync()
        {
            List<Employee> searchResults = await ApiService.GetEmployeesAsync(Instance);

            // Update the employees collection
            ParentViewModel.Employees.Clear();
            foreach (var employee in searchResults)
            {
                ParentViewModel.Employees.Add(new EmployeeViewModel(employee));
            }
        }

        private void Add()
        {
            var newEmployee = new EmployeeViewModel(new Employee()) { IsCreated = false };
            ParentViewModel.Employees.Insert(0, newEmployee);

        }
    }
}