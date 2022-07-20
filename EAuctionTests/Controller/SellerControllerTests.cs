using EAuction.Models.Seller;
using EAuction.Processor.Helpers;
using EAuction.Processor.Interface;
using EAuctionSeller.Controllers;
using EAuctionTests.TestDependencies;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Microsoft.AspNetCore.Mvc;
using EAuction.Models.Enum;

namespace EAuction.Tests.Controller
{
    public class SellerControllerTests
    {
        [Test]
        [FakeDependencies]
        public void SellerController_WithLoggerFactoryNull_ThrownError(
           ISellerProcessor sellerProcessor)
        {
            sellerProcessor = null;
            Assert.Throws<ArgumentException>(() => new SellerController(sellerProcessor));
        }

        [Test]
        [FakeDependencies]
        public async Task SellerController_WithValidOutput_AddNewSeller_Success(
           Mock<ISellerProcessor> sellerProcessor)
        {
            var processorResponse = ResponseHelper.Success<User>(new User());
            sellerProcessor.Setup(x => x.AddNewSeller(It.IsAny<User>()))
                .ReturnsAsync(processorResponse);

            var subject = new SellerController(sellerProcessor.Object);
            var result = await subject.AddNewSeller(new User());
            Assert.IsNotNull(result);
            result.ShouldBeOfType<OkObjectResult>();
        }

        [Test]
        [FakeDependencies]
        public async Task SellerController_WithValidoutput_AddNewSeller_Error(
           Mock<ISellerProcessor> sellerProcessor)
        {
            var processorResponse = ResponseHelper.Error<User>(ResponseCode.Error,"Internal error");
            sellerProcessor.Setup(x => x.AddNewSeller(It.IsAny<User>()))
                .ReturnsAsync(processorResponse);

            var subject = new SellerController(sellerProcessor.Object);
            var result = await subject.AddNewSeller(new User());
            Assert.IsNotNull(result);
            result.ShouldBeOfType<BadRequestObjectResult>();
        }


        [Test]
        [FakeDependencies]
        public async Task SellerController_WithValidOutput_AddProduct_Success(
           Mock<ISellerProcessor> sellerProcessor)
        {
            var processorResponse = ResponseHelper.Success<Product>(new Product());
            sellerProcessor.Setup(x => x.AddProduct(It.IsAny<Product>()))
                .ReturnsAsync(processorResponse);

            var subject = new SellerController(sellerProcessor.Object);
            var result = await subject.AddProduct(new Product());
            Assert.IsNotNull(result);
            result.ShouldBeOfType<OkObjectResult>();
        }

        [Test]
        [FakeDependencies]
        public async Task SellerController_WithValidOutput_AddProduct_Error(
           Mock<ISellerProcessor> sellerProcessor)
        {
            var processorResponse = ResponseHelper.Error<Product>(ResponseCode.Error, "Internal error");
            sellerProcessor.Setup(x => x.AddProduct(It.IsAny<Product>()))
                .ReturnsAsync(processorResponse);

            var subject = new SellerController(sellerProcessor.Object);
            var result = await subject.AddProduct(new Product());
            Assert.IsNotNull(result);
            result.ShouldBeOfType<BadRequestObjectResult>();
        }


        [Test]
        [FakeDependencies]
        public async Task SellerController_WithValidOutput_ShowBids_Success(
       Mock<ISellerProcessor> sellerProcessor)
        {
            var processorResponse = ResponseHelper.Success<ProductBids>(new ProductBids());
            sellerProcessor.Setup(x => x.ShowBids(It.IsAny<string>()))
                .ReturnsAsync(processorResponse);

            var subject = new SellerController(sellerProcessor.Object);
            var result = await subject.ShowBids("test001");
            Assert.IsNotNull(result);
            result.ShouldBeOfType<OkObjectResult>();
        }

        [Test]
        [FakeDependencies]
        public async Task SellerController_WithValidOutput_ShowBids_Error(
           Mock<ISellerProcessor> sellerProcessor)
        {
            var processorResponse = ResponseHelper.Error<ProductBids>(ResponseCode.Error, "Internal error");
            sellerProcessor.Setup(x => x.ShowBids(It.IsAny<string>()))
                .ReturnsAsync(processorResponse);

            var subject = new SellerController(sellerProcessor.Object);
            var result = await subject.ShowBids("test001");
            Assert.IsNotNull(result);
            result.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Test]
        [FakeDependencies]
        public async Task SellerController_WithValidOutput_ShowAllProducts_Success(
       Mock<ISellerProcessor> sellerProcessor)
        {
            var processorResponse = ResponseHelper.Success<List<Product>>(new List<Product>());
            sellerProcessor.Setup(x => x.ShowAllProducts())
                .ReturnsAsync(processorResponse);

            var subject = new SellerController(sellerProcessor.Object);
            var result = await subject.ShowAllProducts();
            Assert.IsNotNull(result);
            result.ShouldBeOfType<OkObjectResult>();
        }

        [Test]
        [FakeDependencies]
        public async Task SellerController_WithValidOutput_ShowAllProducts_Error(
           Mock<ISellerProcessor> sellerProcessor)
        {
            var processorResponse = ResponseHelper.Error<List<Product>>(ResponseCode.Error, "Internal error");
            sellerProcessor.Setup(x => x.ShowAllProducts())
                .ReturnsAsync(processorResponse);

            var subject = new SellerController(sellerProcessor.Object);
            var result = await subject.ShowAllProducts();
            Assert.IsNotNull(result);
            result.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Test]
        [FakeDependencies]
        public async Task SellerController_WithValidOutput_DeletProduct_Success(
       Mock<ISellerProcessor> sellerProcessor)
        {
            var processorResponse = ResponseHelper.Success<bool>(true);
            sellerProcessor.Setup(x => x.DeletProduct(It.IsAny<string>()))
                .ReturnsAsync(processorResponse);

            var subject = new SellerController(sellerProcessor.Object);
            var result = await subject.DeletProduct("test001");
            Assert.IsNotNull(result);
            result.ShouldBeOfType<OkObjectResult>();
        }

        [Test]
        [FakeDependencies]
        public async Task SellerController_WithValidOutput_DeletProduct_Error(
           Mock<ISellerProcessor> sellerProcessor)
        {
            var processorResponse = ResponseHelper.Error<bool>(ResponseCode.Error, "Internal error");
            sellerProcessor.Setup(x => x.DeletProduct(It.IsAny<string>()))
                .ReturnsAsync(processorResponse);

            var subject = new SellerController(sellerProcessor.Object);
            var result = await subject.DeletProduct("test");
            Assert.IsNotNull(result);
            result.ShouldBeOfType<BadRequestObjectResult>();
        }
    }
}
