using EAuction.Models;
using EAuction.Models.Seller;
using EAuction.Processor.Helpers;
using EAuctionTests.TestDependencies;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace EAuctionTests.Helpers
{
    public class ProductValidatorTests
    {
        [Test]
        [FakeDependencies]
        public async Task ProductValidator_EmptyProductName_ThrownError(ProductValidator productValidator)
        {
            var productInput = new Product();
            var result = await productValidator.ValidateAsync(productInput);
            Assert.IsFalse(result.IsValid);
            var validationResult = ValidatorExtension.ValidationErrorExtract(result);
            Assert.Contains($"{nameof(productInput.ProductName)} : {Constants.InvalidProduct}", validationResult.ToList());
        }

        [Test]
        [FakeDependencies]
        public async Task ProductValidator_MinLengthProductName_ThrownError(ProductValidator productValidator)
        {
            var productInput = new Product()
            {
                ProductName = "test"
            };
            var result = await productValidator.ValidateAsync(productInput);
            Assert.IsFalse(result.IsValid);
            var validationResult = ValidatorExtension.ValidationErrorExtract(result);
            Assert.Contains($"{nameof(productInput.ProductName)} : {Constants.InvalidProduct}", validationResult.ToList());
        }

        [Test]
        [FakeDependencies]
        public async Task ProductValidator_MaxLengthProductName_ThrownError(ProductValidator productValidator)
        {
            var productInput = new Product()
            {
                ProductName = new string('a', 40)
            };
            var result = await productValidator.ValidateAsync(productInput);
            Assert.IsFalse(result.IsValid);
            var validationResult = ValidatorExtension.ValidationErrorExtract(result);
            Assert.Contains($"{nameof(productInput.ProductName)} : {Constants.InvalidProduct}", validationResult.ToList());
        }

        [Test]
        [FakeDependencies]
        public async Task ProductValidator_ValidProductName_Success(ProductValidator productValidator)
        {
            var productInput = new Product()
            {
                ProductName = new string('a', 10)
            };
            var result = await productValidator.ValidateAsync(productInput);
            Assert.IsTrue(result.IsValid);
            var validationResult = ValidatorExtension.ValidationErrorExtract(result);
            Assert.IsEmpty(validationResult.ToList());
        }
    }
}
