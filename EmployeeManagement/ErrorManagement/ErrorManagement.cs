using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.ErrorManagement
{
    internal class ErrorManagement
    {
        internal static string ValidateEmail(string email)
        {
            string error = null;

            // Your email validation logic for the Email property
            if (string.IsNullOrWhiteSpace(email))
            {
                error = "Email is required.";
            }
            else if (!IsValidEmail(email))
            {
                error = "Invalid email format.";
            }

            return error;
        }

        private static bool IsValidEmail(string email)
        {
            // A simple regular expression for basic email format validation
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            // Use Regex.IsMatch to check if the email matches the pattern
            return System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern);
        }

        internal static string ValidateEmployeeName(string employeeName)
        {
            if (string.IsNullOrEmpty(employeeName))
            {
                return "Employee name is required.";
            }
            return string.Empty;
        }
        
    }
}
