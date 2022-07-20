using AutoFixture.NUnit3;
using EAuction.Models;
using EAuction.Models.Enum;
using EAuction.Models.Seller;
using EAuction.Processor.Exceptions;
using EAuction.Processor.Interface;
using EAuction.Processor.Services;
using EAuctionTests.TestDependencies;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAuction.Tests.Service
{
    public class ProductServiceTests
    {
        [Test]
        [FakeDependencies]
        public void ProductService_WithProductRepositoryNull_ThrownError(IProductRepository productRepository, 
            IProductToBuyerRepository productToBuyerRepository, 
            IUserRepository userRepository, 
            IServiceBusConsumer serviceBusConsumer,
            IServiceBusPublisher serviceBusPublisher)
        {
            productRepository = null;
            Assert.Throws<ArgumentException>(() => new ProductService(productRepository, productToBuyerRepository, userRepository, serviceBusConsumer, serviceBusPublisher));
        }

        [Test]
        [FakeDependencies]
        public void ProductService_WithProductToBuyerRepositoryNull_ThrownError(
            IProductRepository productRepository, 
            IProductToBuyerRepository productToBuyerRepository, 
            IUserRepository userRepository, 
            IServiceBusConsumer serviceBusConsumer,
            IServiceBusPublisher serviceBusPublisher)
        {
            productToBuyerRepository = null;
            Assert.Throws<ArgumentException>(() => new ProductService(productRepository, productToBuyerRepository, userRepository, serviceBusConsumer,serviceBusPublisher));
        }

        [Test]
        [FakeDependencies]
        public void ProductService_WithUserRepositoryNull_ThrownError(
            IProductRepository productRepository, 
            IProductToBuyerRepository productToBuyerRepository, 
            IUserRepository userRepository,
            IServiceBusConsumer serviceBusConsumer,
            IServiceBusPublisher serviceBusPublisher)
        {
            userRepository = null;
            Assert.Throws<ArgumentException>(() => new ProductService(productRepository, productToBuyerRepository, userRepository, serviceBusConsumer, serviceBusPublisher));
        }

        [Test]
        [FakeDependencies]
        public void ProductService_WithIServiceBusConsumerNull_ThrownError(
            IProductRepository productRepository, 
            IProductToBuyerRepository productToBuyerRepository, 
            IUserRepository userRepository,
            IServiceBusConsumer serviceBusConsumer,
            IServiceBusPublisher serviceBusPublisher)
        {
            serviceBusConsumer = null;
            Assert.Throws<ArgumentException>(() => new ProductService(productRepository, productToBuyerRepository, userRepository, serviceBusConsumer, serviceBusPublisher));
        }

        [Test]
        [FakeDependencies]
        public void ProductService_WithIServiceBusPublisherNull_ThrownError(
            IProductRepository productRepository,
            IProductToBuyerRepository productToBuyerRepository,
            IUserRepository userRepository,
            IServiceBusConsumer serviceBusConsumer,
            IServiceBusPublisher serviceBusPublisher)
        {
            serviceBusPublisher = null;
            Assert.Throws<ArgumentException>(() => new ProductService(productRepository, productToBuyerRepository, userRepository, serviceBusConsumer, serviceBusPublisher));
        }

        [Test]
        [FakeDependencies]
        public void ProductService_WithProductNotFound_AddBidForProductAsync_Error(ProductService productService, Mock<IProductRepository> mockProductRepository)
        {
            var buyerBid = new BuyerBid();

            Product product = null;

            mockProductRepository.Setup(x => x.GetProductByIDAsync(It.IsAny<string>()))
                .ReturnsAsync(product);

            Assert.ThrowsAsync<ProductNotFounException>(async () => await productService.AddBidForProductAsync(buyerBid));
        }

        [Test]
        [FakeDependencies]
        public void ProductService_WithProductException_AddBidForProductAsync_Error(ProductService productService, Mock<IProductRepository> mockProductRepository)
        {
            var buyerBid = new BuyerBid();

            var product = new Product();
            product.BidEndDate = DateTime.Now.AddDays(-2);

            mockProductRepository.Setup(x => x.GetProductByIDAsync(It.IsAny<string>()))
                .ReturnsAsync(product);

            Assert.ThrowsAsync<ProductException>(async () => await productService.AddBidForProductAsync(buyerBid));
        }

        [Test]
        [FakeDependencies]
        public void ProductService_WithUserNotFound_AddBidForProductAsync_Error(ProductService productService,
            [Frozen] Mock<IProductRepository> mockProductRepository,
            [Frozen] Mock<IUserRepository> mockUserRepository,
            Mock<IRabbitMqProducer> mockRabbitMqProducer)
        {
            var buyerBid = new BuyerBid()
            {
                FirstName = "FName001",
                LastName = "LName001",
                Address = "Add001",
                City = "City001",
                State = "State001",
                Pin = "123456",
                Phone = "1234567890",
                Email = "test@gmail.com",
                BidAmount = 100,
                BuyerOrSeller = "Seller",
                ProductId = "001"
            };

            var product = new Product();
            product.BidEndDate = DateTime.Now.AddDays(2);

            mockProductRepository.Setup(x => x.GetProductByIDAsync(It.IsAny<string>()))
                .ReturnsAsync(product);

            User user = null;
            mockUserRepository.Setup(x => x.GetUserByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            Assert.ThrowsAsync<UserNotFounException>(async () => await productService.AddBidForProductAsync(buyerBid));
        }

        [Test]
        [FakeDependencies]
        public async Task ProductService_WithValidInput_AddBidForProductAsync_Success(ProductService productService, 
            [Frozen]Mock<IProductRepository> mockProductRepository,
            [Frozen] Mock<IUserRepository> mockUserRepository,
            Mock<IRabbitMqProducer> mockRabbitMqProducer)
        {
            var buyerBid = new BuyerBid()
            {
                FirstName = "FName001",
                LastName = "LName001",
                Address = "Add001",
                City = "City001",
                State = "State001",
                Pin = "123456",
                Phone = "1234567890",
                Email = "test@gmail.com",
                BidAmount = 100,
                BuyerOrSeller = "Seller",
                ProductId = "001"
            };

            var product = new Product();
            product.BidEndDate = DateTime.Now.AddDays(2);

            mockProductRepository.Setup(x => x.GetProductByIDAsync(It.IsAny<string>()))
                .ReturnsAsync(product);

            User user = new User()
            {
                UserId = Guid.NewGuid().ToString(),
                Email = "test@gmail.com",
            };
            mockUserRepository.Setup(x => x.GetUserByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            mockRabbitMqProducer.Setup(x => x.Publish(It.IsAny<string>()));

            var result = await productService.AddBidForProductAsync(buyerBid);

            Assert.IsNotNull(result);
        }


        [Test]
        [FakeDependencies]
        public async Task CreateOrUpdateProductAsync_WithValidInput_Success(ProductService productService)
        {
            Product productModel = new Product()
            {
                ProductName = "test001",
                ShortDeceription = "t001"
            };

            var result = await productService.CreateOrUpdateProductAsync(productModel);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.BidEndDate);
            Assert.IsNotNull(result.ProductId);
        }

        [FakeDependencies]
        public async Task CreateOrUpdateProductAsync_WithExisitingValidInput_Success(ProductService productService)
        {
            var newGuid = Guid.NewGuid().ToString();
            Product productModel = new Product()
            {
                ProductId = newGuid,
                ProductName = "test001",
                ShortDeceription = "t001"
            };

            var result = await productService.CreateOrUpdateProductAsync(productModel);

            Assert.IsNotNull(result);
            Assert.AreEqual(newGuid, result.ProductId);
            Assert.IsNotNull(result.BidEndDate);
        }

        [Test]
        [FakeDependencies]
        public void CreateOrUpdateProductAsync_WithInvalidInput_Error(ProductService productService,
            [Frozen] Mock<IProductRepository> mockProductRepository)
        {
            Product productModel = new Product()
            {
                ProductId = Guid.NewGuid().ToString(),
                ProductName = "test001",
                ShortDeceription = "t001"
            };

            Product product = null;

            mockProductRepository.Setup(x => x.GetProductByIDAsync(It.IsAny<string>()))
                .ReturnsAsync(product);

            Assert.ThrowsAsync<ProductNotFounException>(async () => await productService.CreateOrUpdateProductAsync(productModel));
        }

        [Test]
        [FakeDependencies]
        public void DeleteAsync_WithInvalidInput_Error(ProductService productService,
            [Frozen] Mock<IProductRepository> mockProductRepository)
        {
            Product productModel = new Product()
            {
                ProductId = Guid.NewGuid().ToString(),
                ProductName = "test001",
                ShortDeceription = "t001"
            };

            Product product = null;

            mockProductRepository.Setup(x => x.GetProductByIDAsync(It.IsAny<string>()))
                .ReturnsAsync(product);

            Assert.ThrowsAsync<ProductNotFounException>(async () => await productService.DeleteAsync(productModel.ProductId));
        }

        [Test]
        [FakeDependencies]
        public void DeleteAsync_WithInvalidInput_ProductHaveBid_Error(ProductService productService,
            [Frozen] Mock<IProductRepository> mockProductRepository)
        {
            Product productModel = new Product()
            {
                ProductId = Guid.NewGuid().ToString(),
                ProductName = "test001",
                ShortDeceription = "t001",
                BidEndDate = DateTime.Now
            };

            mockProductRepository.Setup(x => x.GetProductByIDAsync(It.IsAny<string>()))
                .ReturnsAsync(new Product());

            Assert.ThrowsAsync<ProductException>(async () => await productService.DeleteAsync(productModel.ProductId));
        }

        [Test]
        [FakeDependencies]
        public void DeleteAsync_WithInvalidInput_ProducBidNotEnd_Error(ProductService productService,
            [Frozen] Mock<IProductRepository> mockProductRepository,
            [Frozen] Mock<IProductToBuyerRepository> mockProductToBuyerRepository)
        {
            Product productModel = new Product()
            {
                ProductId = Guid.NewGuid().ToString(),
                ProductName = "test001",
                ShortDeceription = "t001",
                BidEndDate = DateTime.Now.AddDays(4)
            };

            mockProductRepository.Setup(x => x.GetProductByIDAsync(It.IsAny<string>()))
                .ReturnsAsync(productModel);

            var productToBuyer = new ProductToBuyer()
            {
                CreatedDate = DateTime.Now
            };

            mockProductToBuyerRepository.Setup(x => x.GetBidByProductIDAsync(It.IsAny<string>()))
               .ReturnsAsync(new List<ProductToBuyer>() { productToBuyer });

            Assert.ThrowsAsync<ProductException>(async () => await productService.DeleteAsync(productModel.ProductId));
        }

        [Test]
        [FakeDependencies]
        public async Task DeleteAsync_WithValidInput_Success(ProductService productService,
            [Frozen] Mock<IProductRepository> mockProductRepository,
            [Frozen] Mock<IProductToBuyerRepository> mockProductToBuyerRepository)
        {
            Product productModel = new Product()
            {
                ProductId = Guid.NewGuid().ToString(),
                ProductName = "test001",
                ShortDeceription = "t001",
                BidEndDate = DateTime.Now.AddDays(4)
            };

            mockProductRepository.Setup(x => x.GetProductByIDAsync(It.IsAny<string>()))
                .ReturnsAsync(productModel);

            List<ProductToBuyer> productToBuyer = null;

            mockProductToBuyerRepository.Setup(x => x.GetBidByProductIDAsync(It.IsAny<string>()))
               .ReturnsAsync(productToBuyer);

            var result = await productService.DeleteAsync(productModel.ProductId);
            Assert.IsNotNull(result);
        }

        [Test]
        [FakeDependencies]
        public async Task GetProductsAsync_WithValidInput_Success(ProductService productService,
            [Frozen] Mock<IProductRepository> mockProductRepository)
        {

            var productList = new List<Product>(){
                new Product()
            };

            mockProductRepository.Setup(x => x.GetProductsAsync())
                .ReturnsAsync(productList);

            var result = await productService.GetProductsAsync();
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        [FakeDependencies]
        public void GetbidsByProductID_WithInvalidInput_BidNotFound_Error(ProductService productService,
            [Frozen] Mock<IProductRepository> mockProductRepository,
            [Frozen] Mock<IProductToBuyerRepository> mockProductToBuyerRepository)
        {

            var product =new Product();

            mockProductRepository.Setup(x => x.GetProductByIDAsync(It.IsAny<string>()))
                .ReturnsAsync(product);

            List<ProductToBuyer> productToBuyer = null;

            mockProductToBuyerRepository.Setup(x => x.GetBidByProductIDAsync(It.IsAny<string>()))
                .ReturnsAsync(productToBuyer);

            Assert.ThrowsAsync<ProductNotFounException>(async () => await productService.GetbidsByProductID("test0001"));
        }

        [Test]
        [FakeDependencies]
        public async Task GetbidsByProductID_WithValidInput_Success(ProductService productService,
            [Frozen] Mock<IProductRepository> mockProductRepository,
            [Frozen] Mock<IProductToBuyerRepository> mockProductToBuyerRepository)
        {

            var product = new Product()
            {
                Category = ((int)Category.Ornament).ToString()
            };

            mockProductRepository.Setup(x => x.GetProductByIDAsync(It.IsAny<string>()))
                .ReturnsAsync(product);

            var productToBuyer = new List<ProductToBuyer>(){
                new ProductToBuyer()
            };

            mockProductToBuyerRepository.Setup(x => x.GetBidByProductIDAsync(It.IsAny<string>()))
                .ReturnsAsync(productToBuyer);

            var result = await productService.GetbidsByProductID("test0001");
            Assert.IsNotNull(result);
        }

        [Test]
        [FakeDependencies]
        public async Task GetbidsByProductID_WithValidInput_Sculptor_Success(ProductService productService,
            [Frozen] Mock<IProductRepository> mockProductRepository,
            [Frozen] Mock<IProductToBuyerRepository> mockProductToBuyerRepository)
        {

            var product = new Product()
            {
                Category = ((int)Category.Sculptor).ToString()
            };

            mockProductRepository.Setup(x => x.GetProductByIDAsync(It.IsAny<string>()))
                .ReturnsAsync(product);

            var productToBuyer = new List<ProductToBuyer>(){
                new ProductToBuyer()
            };

            mockProductToBuyerRepository.Setup(x => x.GetBidByProductIDAsync(It.IsAny<string>()))
                .ReturnsAsync(productToBuyer);

            var result = await productService.GetbidsByProductID("test0001");
            Assert.IsNotNull(result);
        }

        [Test]
        [FakeDependencies]
        public async Task GetbidsByProductID_WithValidInput_Painting_Success(ProductService productService,
            [Frozen] Mock<IProductRepository> mockProductRepository,
            [Frozen] Mock<IProductToBuyerRepository> mockProductToBuyerRepository)
        {

            var product = new Product()
            {
                Category = ((int)Category.Painting).ToString()
            };

            mockProductRepository.Setup(x => x.GetProductByIDAsync(It.IsAny<string>()))
                .ReturnsAsync(product);

            var productToBuyer = new List<ProductToBuyer>(){
                new ProductToBuyer()
            };

            mockProductToBuyerRepository.Setup(x => x.GetBidByProductIDAsync(It.IsAny<string>()))
                .ReturnsAsync(productToBuyer);

            var result = await productService.GetbidsByProductID("test0001");
            Assert.IsNotNull(result);
        }


        [Test]
        [FakeDependencies]
        public void UpdateBidForProduct_WithInvalidInput_Error(ProductService productService,
            [Frozen] Mock<IUserRepository> mockUserRepository,
            [Frozen] Mock<IProductToBuyerRepository> mockProductToBuyerRepository)
        {

            User user = null;

            mockUserRepository.Setup(x => x.GetUserByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            var buyerBid = new BuyerBid()
            {
                Email = "test@gmail.com",
                BidAmount = Convert.ToDecimal("0.00"),
                ProductId = "product001",
                UserId = "user001"
            };
            Assert.ThrowsAsync<UserNotFounException>(async () => await productService.UpdateBidForProduct(buyerBid));
        }

        [Test]
        [FakeDependencies]
        public async Task UpdateBidForProduct_WithValidInput_Success(ProductService productService,
            [Frozen] Mock<IUserRepository> mockUserRepository,
            [Frozen] Mock<IProductToBuyerRepository> mockProductToBuyerRepository)
        {

            User user = new User()
            {
                UserId = Guid.NewGuid().ToString()
            };

            mockUserRepository.Setup(x => x.GetUserByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            var productToBuyer = new ProductToBuyer()
            {
                BidAmount = 100
            };

            mockProductToBuyerRepository.Setup(x => x.GetProductByUserIDAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(productToBuyer);

            ProductToBuyer productToBuyerOutput = null;

            mockProductToBuyerRepository.Setup(x => x.CreateOrUpdateAsync(It.IsAny<ProductToBuyer>()))
                .Callback((ProductToBuyer input) =>
                {
                    productToBuyerOutput = input;
                })
                .ReturnsAsync(() =>
                {
                    return productToBuyerOutput;
                });
            var buyerBid = new BuyerBid()
            {
                Email = "test@gmail.com",
                BidAmount = Convert.ToDecimal("100.50"),
                ProductId = "product001",
                UserId = "user001"
            };
            var result = await productService.UpdateBidForProduct(buyerBid);

            Assert.IsNotNull(result);
            Assert.AreEqual(Convert.ToDecimal("100.50"), result.BidAmount);
        }
    }
}
