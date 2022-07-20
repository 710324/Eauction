using EAuction.Models;
using EAuction.Models.API;
using EAuction.Models.Enum;
using EAuction.Models.Seller;
using EAuction.Processor.Exceptions;
using EAuction.Processor.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EAuction.Processor.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductToBuyerRepository _productToBuyerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IServiceBusConsumer _serviceBusConsumer;
        private readonly IServiceBusPublisher _serviceBusPublisher;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productRepository"></param>
        /// <param name="productToBuyerRepository"></param>
        /// <param name="userRepository"></param>
        /// <param name="rabbitMqProducer"></param>
        public ProductService(IProductRepository productRepository, 
            IProductToBuyerRepository productToBuyerRepository, 
            IUserRepository userRepository, 
            IServiceBusConsumer serviceBusConsumer,
            IServiceBusPublisher serviceBusPublisher)
        {
            if (productRepository is null)
            {
                throw new ArgumentException(nameof(productRepository));
            }

            if (productToBuyerRepository is null)
            {
                throw new ArgumentException(nameof(productToBuyerRepository));
            }

            if (userRepository is null)
            {
                throw new ArgumentException(nameof(userRepository));
            }

            if (serviceBusConsumer is null)
            {
                throw new ArgumentException(nameof(serviceBusConsumer));
            }

            if (serviceBusPublisher is null)
            {
                throw new ArgumentException(nameof(serviceBusPublisher));
            }
            

            _productRepository = productRepository;
            _productToBuyerRepository = productToBuyerRepository;
            _userRepository = userRepository;
            _serviceBusConsumer = serviceBusConsumer;
            _serviceBusPublisher = serviceBusPublisher;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<ProductToBuyer> AddBidForProductAsync(BuyerBid product)
        {
            var existingProduct = await _productRepository.GetProductByIDAsync(product.ProductId);
            if (existingProduct == null)
            {
                throw new ProductNotFounException($"{Constants.ProductNotFound}");
            }

            if (existingProduct.BidEndDate < DateTime.Today)
            {
                throw new ProductException($"{Constants.BidPlaceError}");
            }

            var user = await _userRepository.GetUserByEmailAsync(product.Email);

            if(user == null)
            {
                throw new UserNotFounException($"{Constants.UserNotFound}");
            }

            var productToBuyer = new ProductToBuyer()
            {
                BidAmount = product.BidAmount,
                ProductId = product.ProductId,
                UserID = user.UserId,
                CreatedDate = DateTime.Now
            };
            var prod = await _productToBuyerRepository.CreateOrUpdateAsync(productToBuyer);

            await _serviceBusPublisher.Publish(new MessagePayload()
            {
                Id = Guid.NewGuid().ToString(),
                Message = String.Format($"{Constants.NewBid}", product.FirstName, product.LastName),
                Created = DateTime.Now
            });
            return prod;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<Product> CreateOrUpdateProductAsync(Product product)
        {
            if (product.ProductId != null)
            {
                var existingProduct = await _productRepository.GetProductByIDAsync(product.ProductId);
                if (existingProduct == null)
                {
                    throw new ProductNotFounException($"{Constants.ProductNotFound}");
                }
                var updatedproduct = await _productRepository.CreateOrUpdateAsync(product);
                return updatedproduct;
            }
            else
            {
                var newProduct = await _productRepository.CreateOrUpdateAsync(product);
                return newProduct;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(string ProductID)
        {
            var existingProduct = await _productRepository.GetProductByIDAsync(ProductID);
            if (existingProduct == null)
            {
                throw new ProductNotFounException($"{Constants.ProductNotFound}");
            }

            var bids = await _productToBuyerRepository.GetBidByProductIDAsync(ProductID);

            if (bids != null && bids.Count() > 0)
            {
                foreach (var item in bids)
                {
                    if (item.CreatedDate > existingProduct.BidEndDate)
                    {
                        throw new ProductException($"{Constants.ProductCannotDelete}");
                    }
                }
                throw new ProductException($"{Constants.ProductCannotDelete}");
            }
            
            return await _productRepository.DeleteAsync(ProductID);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public async Task<Product> GetProductByIDAsync(string ProductID)
        {
            var product = await _productRepository.GetProductByIDAsync(ProductID);
            if (product == null)
            {
                throw new ProductNotFounException($"{Constants.ProductNotFound}");
            }
            return product;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Product>> GetProductsAsync()
        {
            var products = await _productRepository.GetProductsAsync();

            products.ForEach(item =>
              {
                  item.Category = ProductCatIdToName(item.Category);
              });

            return products;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public async Task<ProductBids> GetbidsByProductID(string ProductID)
        {
            var prod = await _productRepository.GetProductByIDAsync(ProductID);
            var bids = await _productToBuyerRepository.GetBidByProductIDAsync(ProductID);

            if (bids == null || bids.Count == 0)
            {
                throw new ProductNotFounException($"{Constants.ProductBidNotFound}");
            }

            var category = string.Empty;
            switch (Convert.ToInt32(prod.Category))
            {
                case (int)Category.Ornament:
                    {
                        category = Category.Ornament.ToString();
                        break;
                    }
                case (int)Category.Sculptor:
                    {
                        category = Category.Sculptor.ToString();
                        break;
                    }
                case (int)Category.Painting:
                    {
                        category = Category.Painting.ToString();
                        break;
                    }
            }
            var productbids = new ProductBids()
            {
                ProductName = prod.ProductName,
                ShortDescription = prod.ShortDeceription,
                DetailedDeceription = prod.DetailedDeceription,
                Category = category,
                BidEndDate = prod.BidEndDate,
                StartingPrice = prod.StartingPrice
            };

            var bidDetails = new List<BidDetails>();
            foreach (var item in bids)
            {
                var existingUser = await _userRepository.GetUserByIDAsync(item.UserID);
                var prodbid = new BidDetails()
                {
                    BuyerProductId = item.BuyerProductId,
                    ProductId= item.ProductId,
                    UserID= item.UserID,
                    BidAmount = item.BidAmount,
                    BuyerName = string.Format($"{existingUser.FirstName} {existingUser.LastName}"),
                    EmailId = existingUser.Email,
                    Phone = existingUser.Phone
                };
                bidDetails.Add(prodbid);
            }
            productbids.BidDetails = bidDetails.OrderByDescending(x => x.BidAmount).ToList();
            await _serviceBusConsumer.RegisterOnMessageHandlerAndReceiveMessages();
            return productbids;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<ProductToBuyer> UpdateBidForProduct(BuyerBid product)
        {

            var existingProduct = await _productRepository.GetProductByIDAsync(product.ProductId);
            if (existingProduct == null)
            {
                throw new ProductNotFounException($"{Constants.ProductNotFound}");
            }

            if (existingProduct.BidEndDate < DateTime.Today)
            {
                throw new ProductException($"{Constants.ProductBidDateExpiredError}");
            }

            var user = await _userRepository.GetUserByEmailAsync(product.Email);
            if (user == null)
            {
                throw new UserNotFounException($"{Constants.UserNotFound}");
            }

            var result = await _productToBuyerRepository.GetProductByUserIDAsync(product.ProductId, user.UserId);

            if (result.BidAmount > product.BidAmount)
            {
                throw new UserNotFounException($"{Constants.ProductBidAmountError}");
            }

            result.BidAmount = product.BidAmount;
            result.Updateddate = DateTime.Now;
            return await _productToBuyerRepository.CreateOrUpdateAsync(result);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="buyerProductId"></param>
        /// <returns></returns>
        public async Task<BuyerBid> GetProductBidByBuyerProductId(string buyerProductId)
        {

            var result = await _productToBuyerRepository.GetProductBidByBuyerProductId(buyerProductId);

            var existingProduct = await _productRepository.GetProductByIDAsync(result.ProductId);
            if (existingProduct == null)
            {
                throw new ProductNotFounException($"{Constants.ProductNotFound}");
            }

            if (existingProduct.BidEndDate < DateTime.Today)
            {
                throw new ProductException($"{Constants.ProductBidDateExpiredError}");
            }

            var user = await _userRepository.GetUserByIDAsync(result.UserID);
            if (user == null)
            {
                throw new UserNotFounException($"{Constants.UserNotFound}");
            }

            var bidresult = new BuyerBid()
            {
                BidId = result.BuyerProductId,
                ProductId = result.ProductId,
                UserId = result.UserID,
                Email = user.Email,
                BidAmount = result.BidAmount
            };

            return bidresult;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        private string ProductCatIdToName(string category)
        {
            var result = string.Empty;
            switch (Convert.ToInt32(category))
            {
                case (int)Category.Ornament:
                    {
                        result = Category.Ornament.ToString();
                        break;
                    }
                case (int)Category.Sculptor:
                    {
                        result = Category.Sculptor.ToString();
                        break;
                    }
                case (int)Category.Painting:
                    {
                        result = Category.Painting.ToString();
                        break;
                    }
            }
            return result;
        }
    }
}
