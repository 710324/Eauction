using EAuction.Models.Enum;
using EAuction.Models.Seller;
using EAuction.Processor.Exceptions;
using EAuction.Processor.Helpers;
using EAuction.Processor.Interface;
using EAuction.Processor.Models;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EAuction.Processor.Processors
{
    public class SellerProcessor : ISellerProcessor
    {

        private readonly IUserService _userService;
        private readonly IProductService _product;
        private readonly ILogger _logger;
        private readonly IValidator<User> _userValidator;
        private readonly IValidator<Product> _productValidator;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="product"></param>
        /// <param name="userService"></param>
        /// <param name="userValidator"></param>
        /// <param name="productValidator"></param>
        public SellerProcessor(ILoggerFactory loggerFactory, IProductService product, IUserService userService, IValidator<User> userValidator, IValidator<Product> productValidator)
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

            if (productValidator is null)
            {
                throw new ArgumentException(nameof(productValidator));
            }

            _logger = loggerFactory.CreateLogger<SellerProcessor>();
            _product = product;
            _userService = userService;
            _userValidator = userValidator;
            _productValidator = productValidator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<ProcessorResponse<User>> AddNewSeller(User user)
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
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<ProcessorResponse<Product>> AddProduct(Product product)
        {
            try
            {
                var validate = await _productValidator.ValidateAsync(product);

                if (!validate.IsValid)
                {
                    var errorList = ValidatorExtension.ValidationErrorExtract(validate);
                    var error = ResponseHelper.Error<Product>(ResponseCode.Error, errorList.ToArray());
                    return error;
                }

                _logger.LogInformation($"Adding New product: {product.ProductName}");
                var result = await _product.CreateOrUpdateProductAsync(product);
                var success = ResponseHelper.Success<Product>(result);
                return success;

            }
            catch (ProductException ex)
            {
                _logger.LogInformation($"Exception : {ex}");
                var error = ResponseHelper.Error<Product>(ResponseCode.Error, ex.Message);
                return error;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public async Task<ProcessorResponse<ProductBids>> ShowBids(string productID)
        {
            try
            {
                _logger.LogInformation($"Showing all bids under product: {productID}");
                var result = await _product.GetbidsByProductID(productID);
                var success = ResponseHelper.Success<ProductBids>(result);
                return success;
            }
            catch (ProductNotFounException ex)
            {
                _logger.LogInformation($"Exception : {ex}");
                var error = ResponseHelper.Error<ProductBids>(ResponseCode.Error, ex.Message);
                return error;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ProcessorResponse<List<Product>>> ShowAllProducts()
        {
            try
            {
                var result = await _product.GetProductsAsync();
                var success = ResponseHelper.Success<List<Product>>(result);
                return success;
            }
            catch (ProductException ex)
            {
                _logger.LogInformation($"Exception : {ex}");
                var error = ResponseHelper.Error<List<Product>>(ResponseCode.Error, ex.Message);
                return error;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ProcessorResponse<List<User>>> ShowAllUser()
        {
            try
            {
                var result = await _userService.GetUsersAsync();
                var success = ResponseHelper.Success<List<User>>(result);
                return success;
            }
            catch (ProductException ex)
            {
                _logger.LogInformation($"Exception : {ex}");
                var error = ResponseHelper.Error<List<User>>(ResponseCode.Error, ex.Message);
                return error;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public async Task<ProcessorResponse<bool>> DeletProduct(string productID)
        {
            try
            {
                var result = await _product.DeleteAsync(productID);
                var success = ResponseHelper.Success<bool>(result);
                return success;
            }
            catch (ProductNotFounException ex)
            {
                _logger.LogInformation($"Exception : {ex}");
                var error = ResponseHelper.Error<bool>(ResponseCode.Error, ex.Message);
                return error;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public async Task<ProcessorResponse<Product>> GetProductById(string productID)
        {
            try
            {
                var result = await _product.GetProductByIDAsync(productID);
                var success = ResponseHelper.Success<Product>(result);
                return success;
            }
            catch (ProductNotFounException ex)
            {
                _logger.LogInformation($"Exception : {ex}");
                var error = ResponseHelper.Error<Product>(ResponseCode.Error, ex.Message);
                return error;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<ProcessorResponse<bool>> DeleteUser(string userID)
        {
            try
            {
                var result = await _userService.DeleteAsync(userID);
                var success = ResponseHelper.Success<bool>(result);
                return success;
            }
            catch (ProductNotFounException ex)
            {
                _logger.LogInformation($"Exception : {ex}");
                var error = ResponseHelper.Error<bool>(ResponseCode.Error, ex.Message);
                return error;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        public async Task<ProcessorResponse<User>> GetUserByEmail(string userEmail)
        {
            try
            {
                var result = await _userService.GetUserByEmailAsync(userEmail);
                var success = ResponseHelper.Success<User>(result);
                return success;
            }
            catch (ProductNotFounException ex)
            {
                _logger.LogInformation($"Exception : {ex}");
                var error = ResponseHelper.Error<User>(ResponseCode.Error, ex.Message);
                return error;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<ProcessorResponse<User>> GetUserByID(string userID)
        {
            try
            {
                var result = await _userService.GetUserByIdAsync(userID);
                var success = ResponseHelper.Success<User>(result);
                return success;
            }
            catch (ProductNotFounException ex)
            {
                _logger.LogInformation($"Exception : {ex}");
                var error = ResponseHelper.Error<User>(ResponseCode.Error, ex.Message);
                return error;
            }
        }


    }
}
