using EAuction.Models.Enum;
using EAuction.Models.Seller;
using EAuction.Processor.Exceptions;
using EAuction.Processor.Helpers;
using EAuction.Processor.Interface;
using EAuction.Processor.Models;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EAuction.Processor.Processors
{
    public class BuyerProcessor : IBuyerProcessor
    {
        private readonly IUserService _userService;
        private readonly IProductService _product;
        private readonly ILogger _logger;
        private readonly IValidator<User> _userValidator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="product"></param>
        public BuyerProcessor(ILoggerFactory loggerFactory, IProductService product, IUserService userService, IValidator<User> userValidator)
        {
            if (loggerFactory is null)
            {
                throw new ArgumentException(nameof(loggerFactory));
            }

            if (product is null)
            {
                throw new ArgumentException(nameof(product));
            }

            if (userService is null)
            {
                throw new ArgumentException(nameof(userService));
            }

            if (userValidator is null)
            {
                throw new ArgumentException(nameof(userValidator));
            }

            _logger = loggerFactory.CreateLogger<SellerProcessor>();
            _product = product;
            _userService = userService;
            _userValidator = userValidator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<ProcessorResponse<ProductToBuyer>> PlaceBid(BuyerBid product)
        {
            try
            {
                var result = await _product.AddBidForProductAsync(product);
                var success = ResponseHelper.Success<ProductToBuyer>(result);
                return success;
            }
            catch (ProductException ex)
            {
                _logger.LogInformation($"Exception : {ex}");
                var error = ResponseHelper.Error<ProductToBuyer>(ResponseCode.Error, ex.Message);
                return error;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<ProcessorResponse<ProductToBuyer>> UpdateBid(BuyerBid product)
        {
            try
            {
                var result = await _product.UpdateBidForProduct(product);
                var success = ResponseHelper.Success<ProductToBuyer>(result);
                return success;
            }
            catch (ProductException ex)
            {
                _logger.LogInformation($"Exception : {ex}");
                var error = ResponseHelper.Error<ProductToBuyer>(ResponseCode.Error, ex.Message);
                return error;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<ProcessorResponse<User>> AddNewBuyer(User user)
        {
            try
            {
                var validate = await _userValidator.ValidateAsync(user);

                if (!validate.IsValid)
                {
                    var errorList = ValidatorExtension.ValidationErrorExtract(validate);
                    var error = ResponseHelper.Error<User>(ResponseCode.Error, errorList.ToArray());
                    return error;
                }

                _logger.LogInformation($"Register New seller: {user.FirstName}");
                var result = await _userService.CreateOrUpdateAsync(user);
                var success = ResponseHelper.Success<User>(result);
                return success;
            }
            catch (UserException ex)
            {
                _logger.LogInformation($"Exception : {ex}");
                var error = ResponseHelper.Error<User>(ResponseCode.Error, ex.Message);
                return error;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buyerProductId"></param>
        /// <returns></returns>
        public async Task<ProcessorResponse<BuyerBid>> GetProductBidByBuyerProductId(string buyerProductId)
        {
            try
            {
                var result = await _product.GetProductBidByBuyerProductId(buyerProductId);

                var success = ResponseHelper.Success<BuyerBid>(result);
                return success;
            }
            catch (ProductException ex)
            {
                _logger.LogInformation($"Exception : {ex}");
                var error = ResponseHelper.Error<BuyerBid>(ResponseCode.Error, ex.Message);
                return error;
            }
        }
    }
}
