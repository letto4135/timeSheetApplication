using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Zeit.Models
{
    public class EmployeeModel
    {
        public Guid Id { get; }
        private string _firstName;
        private string _lastName;
        private string _divison;
        private bool _result;
        public string firstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _result = Regex.IsMatch(firstName, @"^[a-zA-Z]+$");
                if (!String.IsNullOrEmpty(firstName) && _result)
                {
                    this._firstName = firstName;
                }
                else
                {
                    throw new FormatException("First name can not be empty, must only contain letters.");
                }
            }
        }
        public string lastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _result = Regex.IsMatch(lastName, @"^[a-zA-Z]+$");
                if (!String.IsNullOrEmpty(lastName) && _result)
                {
                    this._lastName = lastName;
                }
                else
                {
                    throw new FormatException("Last name can not be empty, must only contain letters.");
                }
            }
        }
        public string divison
        {
            get
            {
                return _divison; 
            }
            set
            {
                _result = result = Regex.IsMatch(divison, @"^[a-zA-Z]+$");
                if (!String.IsNullOrEmpty(divison))
                {
                    this._divison = divison;
                }
                else
                {
                    throw new FormatException("Must provide divison name.");
                }
            }
        }
        public bool exempt { get; set; }

    }
}

