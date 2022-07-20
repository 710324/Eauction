using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;
using EAuction.Models.Seller;
using EAuction.Processor.Interface;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;

namespace EAuctionTests.TestDependencies
{
    public class FakeDependencies : AutoDataAttribute
    {
        public FakeDependencies() : base(CreateFixture)
        {

        }

        private static IFixture CreateFixture()
        {
            var fixture = new Fixture();

            var productService = fixture.Freeze<Mock<IProductService>>();
            productService.Setup(x => x.CreateOrUpdateProductAsync(It.IsAny<Product>()))
                .ReturnsAsync(new Product());
            productService.Setup(x => x.GetbidsByProductID(It.IsAny<string>()))
                .ReturnsAsync(new ProductBids());
            productService.Setup(x => x.GetProductsAsync())
                .ReturnsAsync(new List<Product>());
            productService.Setup(x => x.DeleteAsync(It.IsAny<string>()))
                .ReturnsAsync(true);
            productService.Setup(x => x.AddBidForProductAsync(It.IsAny<BuyerBid>()))
                .ReturnsAsync(new ProductToBuyer());
            productService.Setup(x => x.UpdateBidForProduct(It.IsAny<BuyerBid>()))
               .ReturnsAsync(new ProductToBuyer());

            var userService = fixture.Freeze<Mock<IUserService>>();
            userService.Setup(x => x.CreateOrUpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(new User());

            var mockUserValidator = new Mock<IValidator<User>>();
            mockUserValidator.Setup(x => x.ValidateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            fixture.Register(() => mockUserValidator);


            var mockProductValidator = new Mock<IValidator<Product>>();
            mockProductValidator.Setup(x => x.ValidateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            fixture.Register(() => mockProductValidator);

            var mockUserRepository = new Mock<IUserRepository>();

            User usermodel = null;

            mockUserRepository.Setup(x => x.CreateOrUpdateAsync(It.IsAny<User>()))
                .Callback((User userInput) =>
                {
                    if (userInput.UserId == null)
                    {
                        userInput.UserId = Guid.NewGuid().ToString();
                    }
                    else
                    {
                        userInput.Updateddate = DateTime.Now;
                    }
                    usermodel = userInput;

                })
                .ReturnsAsync(() =>
                {
                    return usermodel;
                });

            mockUserRepository.Setup(x => x.GetUserByIDAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            fixture.Register(() => mockUserRepository);

            var mockProductRepository = new Mock<IProductRepository>();

            mockProductRepository.Setup(x => x.GetProductByIDAsync(It.IsAny<string>()))
                .ReturnsAsync(new Product());

            Product productModel = null;

            mockProductRepository.Setup(x => x.CreateOrUpdateAsync(It.IsAny<Product>()))
                .Callback((Product productInput) =>
                {
                    if (productInput.ProductId == null)
                    {
                        productInput.ProductId = Guid.NewGuid().ToString();
                    }
                    else
                    {
                        productInput.BidEndDate = DateTime.Now.AddDays(30);
                    }
                    productModel = productInput;

                })
                .ReturnsAsync(() =>
                {
                    return productModel;
                });


            fixture.Register(() => mockProductRepository);

            var mockProductToBuyerRepository = new Mock<IProductToBuyerRepository>();
            mockProductToBuyerRepository.Setup(x => x.CreateOrUpdateAsync(It.IsAny<ProductToBuyer>()))
                .ReturnsAsync(new ProductToBuyer());

            mockProductToBuyerRepository.Setup(x => x.GetBidByProductIDAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<ProductToBuyer>() { new ProductToBuyer(){
                    CreatedDate= DateTime.Now.AddDays(2)
                } });

            fixture.Register(() => mockProductToBuyerRepository);

            fixture.Customize(new AutoMoqCustomization());

            return fixture;
        }
    }
}
