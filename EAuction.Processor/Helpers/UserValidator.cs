using EAuction.Models;
using EAuction.Models.Seller;
using FluentValidation;
using System.Text.RegularExpressions;

namespace EAuction.Processor.Helpers
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.FirstName)
                .Must((model, val) => ValidateFirstName(val))
                .WithMessage(Constants.InvalidFirstName);
            RuleFor(x => x.LastName)
                .Must((model, val) => ValidateLastName(val))
                .WithMessage(Constants.InvalidLastName);
            RuleFor(x => x.Phone)
                .Must((model, val) => ValidatePhoneNumber(val))
                .WithMessage(Constants.InvalidPhoneNumber);
            RuleFor(x => x.Email)
                .Must((model, val) => ValidateEmail(val))
                .WithMessage(Constants.InvalidEmail);
        }

        public bool ValidateFirstName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;
            else if (name.Length > 30 || name.Length < 5)
                return false;
            else
                return true;
        }

        public bool ValidateLastName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;
            else if (name.Length > 30 || name.Length < 3)
                return false;
            else
                return true;

        }

        public bool ValidateEmail(string emailaddress)
        {
            if (emailaddress == null || emailaddress == "")
            {
                return false;
            }

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(emailaddress);

            return match.Success;
        }

        public bool ValidatePhoneNumber(string number)
        {
            if (number != null)
            {
                if (number.Length != 10)
                {
                    return false;
                }
                else
                {
                    return Regex.IsMatch(number, Constants.PhFormat);
                }
            }
            else
            {
                return false;
            }
        }
    }
}
