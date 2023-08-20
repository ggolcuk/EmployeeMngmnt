using EmployeeManagement.Models;
using Common.WPF.WPFUtilities;
using System;
using System.Windows.Controls;

namespace EmployeeManagement.ViewModels
{
    public class SearchParametersViewModel: ViewModel
    {

        private readonly SearchParameters _searchParameters;

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

        public SearchParametersViewModel(SearchParameters sp)
        {
            _searchParameters = sp;
        }

        internal void SetDefault()
        {
            _searchParameters.SetDefault();
            OnPropertyChanged(string.Empty);
        }
    }
}