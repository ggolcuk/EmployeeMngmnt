using Common.WPF.WPFUtilities;
using EmployeeManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModels
{
    public class EmployeeViewModel : ViewModel
    {
        private bool _isChanged;
        private readonly Employee _employee;
        private readonly Employee _originalEmployee;

        public Employee MainEmployee
        {
            get { return _employee; }
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
                
            }
        }

        public EmployeeViewModel(Employee employee)
        {
            _employee = employee;
            _originalEmployee = employee.Clone(); // Save a clone of the original employee
            _isChanged = false;
        }

        public bool IsChanged
        {
            get { return _isChanged; }
            set
            {
                if (_isChanged == value) return;
                _isChanged = value;
                OnPropertyChanged(nameof(IsChanged));
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

        }

        internal void Update()
        {
            _originalEmployee.name = Name;
            _originalEmployee.email = Email;
            _originalEmployee.status = Status;
            _originalEmployee.gender = Gender;
        }
    }

}
