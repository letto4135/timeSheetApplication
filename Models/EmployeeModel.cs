using System;
using System.ComponentModel.DataAnnotations;

namespace Zeit
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
                if (!firstName.IsNullOrEmpty())
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
                if (!lastName.IsNullOrEmpty())
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
                if (!divison.IsNullOrEmpty())
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

