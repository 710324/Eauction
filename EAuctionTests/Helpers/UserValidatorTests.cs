using EAuction.Models;
using EAuction.Models.Seller;
using EAuction.Processor.Helpers;
using EAuctionTests.TestDependencies;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace EAuctionTests.Helpers
{
    public class UserValidatorTests
    {
        [Test]
        [FakeDependencies]
        public async Task UserValidator_EmptyFirstName_ThrownError(UserValidator userValidator)
        {
            var userInput = new User();
            var result = await userValidator.ValidateAsync(userInput);
            Assert.IsFalse(result.IsValid);
            var validationResult = ValidatorExtension.ValidationErrorExtract(result);
            Assert.Contains($"{nameof(userInput.FirstName)} : {Constants.InvalidFirstName}", validationResult.ToList());
        }

        [Test]
        [FakeDependencies]
        public async Task UserValidator_MinLengthInvalidFirstName_ThrownError(UserValidator userValidator)
        {
            var userInput = new User()
            {
                FirstName = "fst"
            };
            var result = await userValidator.ValidateAsync(userInput);
            Assert.IsFalse(result.IsValid);
            var validationResult = ValidatorExtension.ValidationErrorExtract(result);
            Assert.Contains($"{nameof(userInput.FirstName)} : {Constants.InvalidFirstName}", validationResult.ToList());
        }

        [Test]
        [FakeDependencies]
        public async Task UserValidator_MaxLengthInvalidFirstName_ThrownError(UserValidator userValidator)
        {
            var userInput = new User()
            {
                FirstName = new string('a', 40)
            };
            var result = await userValidator.ValidateAsync(userInput);
            Assert.IsFalse(result.IsValid);
            var validationResult = ValidatorExtension.ValidationErrorExtract(result);
            Assert.Contains($"{nameof(userInput.FirstName)} : {Constants.InvalidFirstName}", validationResult.ToList());
        }

        [Test]
        [FakeDependencies]
        public async Task UserValidator_EmptyLastName_ThrownError(UserValidator userValidator)
        {
            var userInput = new User()
            {
                FirstName = "FirstName001"
            };
            var result = await userValidator.ValidateAsync(userInput);
            Assert.IsFalse(result.IsValid);
            var validationResult = ValidatorExtension.ValidationErrorExtract(result);
            Assert.Contains($"{nameof(userInput.LastName)} : {Constants.InvalidLastName}", validationResult.ToList());
        }

        [Test]
        [FakeDependencies]
        public async Task UserValidator_MinLengthInvalidLastName_ThrownError(UserValidator userValidator)
        {
            var userInput = new User()
            {
                FirstName = "FirstName001",
                LastName = "ln"
            };
            var result = await userValidator.ValidateAsync(userInput);
            Assert.IsFalse(result.IsValid);
            var validationResult = ValidatorExtension.ValidationErrorExtract(result);
            Assert.Contains($"{nameof(userInput.LastName)} : {Constants.InvalidLastName}", validationResult.ToList());
        }

        [Test]
        [FakeDependencies]
        public async Task UserValidator_MaxLengthInvalidLastName_ThrownError(UserValidator userValidator)
        {
            var userInput = new User()
            {
                FirstName = "FirstName001",
                LastName = new string('a', 40)
            };
            var result = await userValidator.ValidateAsync(userInput);
            Assert.IsFalse(result.IsValid);
            var validationResult = ValidatorExtension.ValidationErrorExtract(result);
            Assert.Contains($"{nameof(userInput.LastName)} : {Constants.InvalidLastName}", validationResult.ToList());
        }

        [Test]
        [FakeDependencies]
        public async Task UserValidator_InvalidPhoneNull_ThrownError(UserValidator userValidator)
        {
            var userInput = new User()
            {
                FirstName = "FirstName001",
                LastName = "lst"
            };
            var result = await userValidator.ValidateAsync(userInput);
            Assert.IsFalse(result.IsValid);
            var validationResult = ValidatorExtension.ValidationErrorExtract(result);
            Assert.Contains($"{nameof(userInput.Phone)} : {Constants.InvalidPhoneNumber}", validationResult.ToList());
        }

        [Test]
        [FakeDependencies]
        public async Task UserValidator_InvalidPhoneLength_ThrownError(UserValidator userValidator)
        {
            var userInput = new User()
            {
                FirstName = "FirstName001",
                LastName = "lst",
                Phone = "123456789"
            };
            var result = await userValidator.ValidateAsync(userInput);
            Assert.IsFalse(result.IsValid);
            var validationResult = ValidatorExtension.ValidationErrorExtract(result);
            Assert.Contains($"{nameof(userInput.Phone)} : {Constants.InvalidPhoneNumber}", validationResult.ToList());
        }

        [Test]
        [FakeDependencies]
        public async Task UserValidator_InvalidEmailNull_ThrownError(UserValidator userValidator)
        {
            var userInput = new User()
            {
                FirstName = "FirstName001",
                LastName = "lst",
                Phone = "1234567890"
            };
            var result = await userValidator.ValidateAsync(userInput);
            Assert.IsFalse(result.IsValid);
            var validationResult = ValidatorExtension.ValidationErrorExtract(result);
            Assert.Contains($"{nameof(userInput.Email)} : {Constants.InvalidEmail}", validationResult.ToList());
        }

        [Test]
        [FakeDependencies]
        public async Task UserValidator_InvalidEmailEmpty_ThrownError(UserValidator userValidator)
        {
            var userInput = new User()
            {
                FirstName = "FirstName001",
                LastName = "lst",
                Phone = "1234567890",
                Email = ""
            };
            var result = await userValidator.ValidateAsync(userInput);
            Assert.IsFalse(result.IsValid);
            var validationResult = ValidatorExtension.ValidationErrorExtract(result);
            Assert.Contains($"{nameof(userInput.Email)} : {Constants.InvalidEmail}", validationResult.ToList());
        }

        [Test]
        [FakeDependencies]
        public async Task UserValidator_ValidInput_Success(UserValidator userValidator)
        {
            var userInput = new User()
            {
                FirstName = "FirstName001",
                LastName = "lst",
                Phone = "1234567890",
                Email = "test@gmail.com"
            };
            var result = await userValidator.ValidateAsync(userInput);
            Assert.IsTrue(result.IsValid);
            var validationResult = ValidatorExtension.ValidationErrorExtract(result);
            Assert.IsEmpty(validationResult.ToList());
        }
    }
}
