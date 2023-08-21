using Common.Utilities.EnumUtilities;
using Common.WPF.ErrorUtilities;
using Common.WPF.WPFUtilities;
using EmployeeManagement.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;

namespace EmployeeManagement.ViewModels
{
    public enum StatusType
    {
        [Description("active")]
        Active = 0,

        [Description("inactive")]
        Inactive = 1,

        [Description("")]
        All = 2
    }

    public enum GenderType
    {
        [Description("male")]
        Male,

        [Description("female")]
        Female,
        
        [Description("")]
        All
    }




    public class EmployeeViewModel : ViewModel
    {
        private bool _isChanged;
        private bool _isCreated;
        
        private readonly Employee _employee;
        private readonly Employee _originalEmployee;
        private GenderType _selectedGender;
        private StatusType _selectedStatus;

        
        public Employee MainEmployee
        {
            get { return _employee; }
        }

        public Employee OriginalEmployee
        {
            get { return _originalEmployee; }
        }

        public int Id
        {
            get { return _employee.id; }
        }


        public string Name
        {
            get { return _employee.name; }
            set
            {
                if (_employee.name == value) return;

                _employee.name = value;
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(CanUpdated));
                
            }
        }

        public string Gender
        {
            get { return _employee.gender; }
            set
            {
                if (_employee.gender == value) return;

                _employee.gender = value;
                OnPropertyChanged(nameof(Gender));
                OnPropertyChanged(nameof(CanUpdated));
            }
        }

        public GenderType SelectedGender
        {
            get { return _selectedGender;  }
            set
            {
                var gender = EnumUtility.GetDescription(value);
                if (_selectedGender == value) return;
                _employee.gender = gender;
                _selectedGender = value;
                OnPropertyChanged(nameof(SelectedGender));
                OnPropertyChanged(nameof(Gender));
                OnPropertyChanged(nameof(CanUpdated));

            }

        }

        public StatusType SelectedStatus
        {
            get { return _selectedStatus; }
            set
            {
                var status = EnumUtility.GetDescription(value);
                if (_selectedStatus == value) return;
                _employee.status = status;
                _selectedStatus = value;
                OnPropertyChanged(nameof(SelectedStatus));
                OnPropertyChanged(nameof(Status));
                OnPropertyChanged(nameof(CanUpdated));

            }

        }

        public string Email
        {
            get { return _employee.email; }
            set
            {
                if (_employee.email == value) return;

                _employee.email = value;
                OnPropertyChanged(nameof(Email));
                OnPropertyChanged(nameof(CanUpdated));
               
            }
        }

        public string Status
        {
            get { return _employee.status; }

            set
            {
                if (_employee.status == value) return;

                _employee.status = value;
                OnPropertyChanged(nameof(Status));
                OnPropertyChanged(nameof(CanUpdated));

                
            }
        }

        public bool IsCreated
        {
            get { return _isCreated; }

            set
            {
                if (_isCreated == value) return;

                _isCreated = value;
                OnPropertyChanged(nameof(IsCreated));

            }
        }

        public EmployeeViewModel(Employee employee, bool isCreated = true)
        {
            _employee = employee;
            _originalEmployee = employee.Clone(); // Save a clone of the original employee
            _isChanged = false;
            _isCreated = isCreated;

            SelectedStatus = (StatusType)EnumUtility.FromDescription(_employee.status, typeof(StatusType));
            SelectedGender = (GenderType)EnumUtility.FromDescription(_employee.gender, typeof(GenderType));

        }

        public bool IsChanged
        {
            get { return _isChanged; }
            set
            {
                if (_isChanged == value) return;
                _isChanged = value;
                OnPropertyChanged(nameof(IsChanged));
                OnPropertyChanged(nameof(CanUpdated));
            }
        }

        public bool CanUpdated
        {
            get {
               var isValid = IsValid();
                return (IsChanged && isValid); 
                }

        }


        public override void OnPropertyChanged(string name = "")
        {
            base.OnPropertyChanged(name);
            // Compare current employee with the original to determine if changes were made
            IsChanged = !_employee.IsEqual(_originalEmployee);
        }

        internal void Revert()
        {
            Name = _originalEmployee.name;
            Email = _originalEmployee.email;
            Status = _originalEmployee.status;
            Gender = _originalEmployee.gender;
            OnPropertyChanged(nameof(IsChanged));

        }

        internal void Update()
        {
            _originalEmployee.name = Name;
            _originalEmployee.email = Email;
            _originalEmployee.status = Status;
            _originalEmployee.gender = Gender;

            OnPropertyChanged(nameof(IsChanged));
        }


        #region UI Validation
        private readonly string[] propertiesToValidate = new[] { nameof(Name), nameof(Email) };

        public bool IsValid()
        {
            return string.IsNullOrEmpty(GetErrors());

        }
        private string GetErrors()
        {
            List<string> errors = new List<string>();

            errors.AddRange(propertiesToValidate.Select(GetValidationError).NotNull());

            if (errors.Count != 0)
                return errors.ToError();
            return null;

        }

        public override string GetValidationError(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Name):
                    return ErrorManagement.ErrorManagement.ValidateEmployeeName(Name);
                case nameof(Email):
                    return ErrorManagement.ErrorManagement.ValidateEmail(Email);
                    break;
                default:
                    return string.Empty;
            }
        }
        #endregion



    }

}
