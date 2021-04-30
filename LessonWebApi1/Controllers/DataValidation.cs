using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LessonWebApi1.Controllers
{
    public class DataValidation
    {
        public bool ValidateEmail(string email, out string ErrorMessage)
        {
            var input = email;
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(input))
            {
                ErrorMessage = "Email should not be empty.";
                return false;
            }

            var regex = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                   + "@"
                                   + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z");

            if (!regex.IsMatch(input))
            {
                ErrorMessage = "Wrong Email format. Example: example@gmail.com";
                return false;
            }
            else
            {
                return true;
            }
        }

        // Validating Password.
        public bool ValidatePassword(string password, out string ErrorMessage)
        {
            var input = password;
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(input))
            {
                ErrorMessage = "Password should not be empty.";
                return false;
            }

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{8,15}");
            var hasLowerChar = new Regex(@"[a-z]+");

            if (!hasLowerChar.IsMatch(input))
            {
                ErrorMessage = "Password should contain At least one lower case letter.";
                return false;
            }
            else if (!hasUpperChar.IsMatch(input))
            {
                ErrorMessage = "Password should contain At least one upper case letter.";
                return false;
            }
            else if (!hasMiniMaxChars.IsMatch(input))
            {
                ErrorMessage = "Password should not be less than or greater than 12 characters.";
                return false;
            }
            else if (!hasNumber.IsMatch(input))
            {
                ErrorMessage = "Password should contain At least one numeric value.";
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool ValidateName(string Name, out string ErrorMessage)
        {
            var input = Name;
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(input))
            {
                ErrorMessage = "Name should not be empty.";
                return false;
            }

            var regex = new Regex(@"([А-ЯA-Z]|[А-ЯA-Z][\x27а-яa-z]{1,}|[А-ЯA-Z][\x27а-яa-z]{1,}\-([А-ЯA-Z][\x27а-яa-z]{1,}|(оглы)|(кызы)))\040[А-ЯA-Z][\x27а-яa-z]{1,}(\040[А-ЯA-Z][\x27а-яa-z]{1,})");

            if (!regex.IsMatch(input))
            {
                ErrorMessage = "Wrong Name format. Example: Петров Петр Петрович.";
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool ValidatePhoneNumber(string PhoneNumber, out string ErrorMessage)
        {
            var input = PhoneNumber;
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(input))
            {
                ErrorMessage = "Phonenumber should not be empty.";
                return false;
            }
            //((\+38|8|\+3|\+ )[ ]?)?([(]?\d{3}[)]?[\- ]?)?(\d[ -]?){6,14}

            var regex = new Regex(@"((\+38|8|\+3|\+ )[ ]?)?([(]?\d{3}[)]?[\- ]?)?(\d[ -]?){6,14}");
            if (!regex.IsMatch(input))
            {
                ErrorMessage = "Wrong Phone number format. Example: +380668763212.";
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool ValidateApartment(string ApartmentNumber, out string ErrorMessage)
        {
            var input = ApartmentNumber;
            ErrorMessage = string.Empty;


            if (string.IsNullOrWhiteSpace(input))
            {
                ErrorMessage = "Apartment should not be empty.";
                return false;
            }

            var regex = new Regex(@"([1-9][0-9]{0,2}|1000)");
            if (!regex.IsMatch(input))
            {
                ErrorMessage = "Wrong Apartment format. Example: 43.";
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
