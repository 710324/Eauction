using AutoFixture.NUnit3;
using EAuction.Models;
using EAuction.Models.Seller;
using EAuction.Processor.Exceptions;
using EAuction.Processor.Interface;
using EAuction.Processor.Processors;
using EAuctionTests.TestDependencies;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EAuctionTests.Processors
{
    public class BuyerProcessorTests
    {
        [Test]
        [FakeDependencies]
        public void BuyerProcessor_WithLoggerFactoryNull_ThrownError(
            ILoggerFactory loggerFactory,
            IProductService product,
            IUserService user,
            IValidator<User> userValidator)
        {
            loggerFactory = null;
            Assert.Throws<ArgumentException>(() => new BuyerProcessor(loggerFactory, product, user, userValidator));
        }

        [Test]
        [FakeDependencies]
        public void BuyerProcessor_WithProductNull_ThrownError(
            ILoggerFactory loggerFactory,
            IProductService product,
            IUserService user,
            IValidator<User> userValidator)
        {
            product = null;
            Assert.Throws<ArgumentException>(() => new BuyerProcessor(loggerFactory, product, user, userValidator));
        }

        [Test]
        [FakeDependencies]
        public void BuyerProcessor_WithuserNull_ThrownError(
            ILoggerFactory loggerFactory,
            IProductService product,
            IUserService user,
            IValidator<User> userValidator)
        {
            user = null;
            Assert.Throws<ArgumentException>(() => new BuyerProcessor(loggerFactory, product, user, userValidator));
        }

        [Test]
        [FakeDependencies]
        public void BuyerProcessor_WithuserValidatorNull_ThrownError(
            ILoggerFactory loggerFactory,
            IProductService product,
            IUserService user,
            IValidator<User> userValidator)
        {
            userValidator = null;
            Assert.Throws<ArgumentException>(() => new BuyerProcessor(loggerFactory, product, user, userValidator));
        }

        [Test]
        [FakeDependencies]
        public async Task BuyerProcessor_WithAddBidForProductAsync_ThrowError(
            [Frozen] Mock<IProductService> mockProductService,
            BuyerProcessor processor)
        {
            var buyerBid = new BuyerBid();

            mockProductService.Setup(x => x.AddBidForProductAsync(It.IsAny<BuyerBid>()))
                .ThrowsAsync(new ProductException(Constants.ProductNotFound));

            var result = await processor.PlaceBid(buyerBid);
            result.Data.ShouldBeNull();
            result.Errors.ShouldNotBeNull();
            Assert.Contains(Constants.ProductNotFound, result.Errors.ToList());
        }

        [Test]
        [FakeDependencies]
        public async Task BuyerProcessor_PlaceBid_WithValid_Success(
            BuyerProcessor processor)
        {
            var buyerBid = new BuyerBid();
            var result = await processor.PlaceBid(buyerBid);
            result.Data.ShouldNotBeNull();
        }

        [Test]
        [FakeDependencies]
        public async Task BuyerProcessor_WithUpdateBidForProduct_ThrowError(
            [Frozen] Mock<IProductService> mockProductService,
            BuyerProcessor processor)
        {
            var buyerBid = new BuyerBid();

            mockProductService.Setup(x => x.UpdateBidForProduct(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>()))
                .ThrowsAsync(new ProductException(Constants.ProductNotFound));

            var result = await processor.UpdateBid("test001", "buyer@gmail.com", Convert.ToDecimal(0.00));
            result.Data.ShouldBeNull();
            result.Errors.ShouldNotBeNull();
            Assert.Contains(Constants.ProductNotFound, result.Errors.ToList());
        }

        [Test]
        [FakeDependencies]
        public async Task BuyerProcessor_UpdateBid_WithValid_Success(
            BuyerProcessor processor)
        {
            var buyerBid = new BuyerBid();
            var result = await processor.UpdateBid("test001", "buyer@gmail.com", Convert.ToDecimal(0.00));
            result.Data.ShouldNotBeNull();
        }
    }
}
