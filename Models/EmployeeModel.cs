using System;
using System.ComponentModel.DataAnnotations;

namespace Zeit.Models
{
    public class EmployeeModel
    {
        public Guid Id { get; }
        private string _firstName;
        private string _lastName;
        private string _divison;
        public string firstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                if (!String.IsNullOrEmpty(firstName))
                {
                    this._firstName = firstName;
                }
                else
                {
                    throw new FormatException("First name can not be empty");
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
                if (!String.IsNullOrEmpty(lastName))
                {
                    this._lastName = lastName;
                }
                else
                {
                    throw new FormatException("Last name can not be empty");
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

