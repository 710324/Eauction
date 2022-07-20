using EAuction.Models.Enum;
using EAuction.Models.Seller;
using EAuction.Processor.Helpers;
using EAuction.Processor.Interface;
using EAuctionBuyer.Controllers;
using EAuctionTests.TestDependencies;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EAuction.Tests.Controller
{
    class BuyerControllerTests
    {
        [Test]
        [FakeDependencies]
        public void BuyerController_WithLoggerFactoryNull_ThrownError(
           IBuyerProcessor buyerProcessor)
        {
            buyerProcessor = null;
            Assert.Throws<ArgumentException>(() => new BuyerController(buyerProcessor));
        }

        [Test]
        [FakeDependencies]
        public async Task BuyerController_WithValidOutput_PlaceBid_Success(
           Mock<IBuyerProcessor> buyerProcessor)
        {
            var processorResponse = ResponseHelper.Success<ProductToBuyer>(new ProductToBuyer());
            buyerProcessor.Setup(x => x.PlaceBid(It.IsAny<BuyerBid>()))
                .ReturnsAsync(processorResponse);

            var subject = new BuyerController(buyerProcessor.Object);
            var result = await subject.PlaceBid(new BuyerBid());
            Assert.IsNotNull(result);
            result.ShouldBeOfType<OkObjectResult>();
        }

        [Test]
        [FakeDependencies]
        public async Task BuyerController_WithValidoutput_PlaceBid_Error(
           Mock<IBuyerProcessor> buyerProcessor)
        {
            var processorResponse = ResponseHelper.Error<ProductToBuyer>(ResponseCode.Error, "Internal error");
            buyerProcessor.Setup(x => x.PlaceBid(It.IsAny<BuyerBid>()))
                .ReturnsAsync(processorResponse);

            var subject = new BuyerController(buyerProcessor.Object);
            var result = await subject.PlaceBid(new BuyerBid());
            Assert.IsNotNull(result);
            result.ShouldBeOfType<BadRequestObjectResult>();
        }


        [Test]
        [FakeDependencies]
        public async Task BuyerController_WithValidOutput_UpdateBid_Success(
           Mock<IBuyerProcessor> buyerProcessor)
        {
            var processorResponse = ResponseHelper.Success<ProductToBuyer>(new ProductToBuyer());
            buyerProcessor.Setup(x => x.UpdateBid(It.IsAny<BuyerBid>()))
                .ReturnsAsync(processorResponse);

            var subject = new BuyerController(buyerProcessor.Object);
            var buyerBid = new BuyerBid()
            {
                Email = "test@gmail.com",
                BidAmount = Convert.ToDecimal("0.00"),
                ProductId = "product001",
                UserId = "user001"
            };
            var result = await subject.UpdateBid(buyerBid);
            Assert.IsNotNull(result);
            result.ShouldBeOfType<OkObjectResult>();
        }

        [Test]
        [FakeDependencies]
        public async Task BuyerController_WithValidOutput_UpdateBid_Error(
           Mock<IBuyerProcessor> buyerProcessor)
        {
            var processorResponse = ResponseHelper.Error<ProductToBuyer>(ResponseCode.Error, "Internal error");
            buyerProcessor.Setup(x => x.UpdateBid(It.IsAny<BuyerBid>()))
                .ReturnsAsync(processorResponse);

            var subject = new BuyerController(buyerProcessor.Object);
            var buyerBid = new BuyerBid()
            {
                Email = "test@gmail.com",
                BidAmount = Convert.ToDecimal("0.00"),
                ProductId = "product001",
                UserId = "user001"
            };
            var result = await subject.UpdateBid(buyerBid);
            Assert.IsNotNull(result);
            result.ShouldBeOfType<BadRequestObjectResult>();
        }
    }
}
