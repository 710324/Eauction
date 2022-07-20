using EAuction.Models.Enum;
using EAuction.Models.Seller;
using EAuction.Processor.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace EAuctionSeller.Controllers
{
    [Route("e-auction-seller/api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class SellerController : ControllerBase
    {
        private readonly ISellerProcessor _sellerProcessor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sellerProcessor"></param>
        public SellerController(ISellerProcessor sellerProcessor)
        {
            if (sellerProcessor is null)
            {
                throw new ArgumentException(nameof(sellerProcessor));
            }
            _sellerProcessor = sellerProcessor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("addseller")]
        public async Task<IActionResult> AddNewSeller(User user)
        {
            user.UserType = "1";

            var result = await _sellerProcessor.AddNewSeller(user);

            return result.ResponseCode switch
            {
                ResponseCode.Success => Ok(result),
                ResponseCode.Error => BadRequest(result),
                _ => StatusCode((int)HttpStatusCode.InternalServerError, result.Errors),
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("addproduct")]
        public async Task<IActionResult> AddProduct(Product product)
        {
            var result = await _sellerProcessor.AddProduct(product);

            return result.ResponseCode switch
            {
                ResponseCode.Success => Ok(result),
                ResponseCode.Error => BadRequest(result),
                _ => StatusCode((int)HttpStatusCode.InternalServerError, result.Errors),
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("show-bids/{productID}")]
        public async Task<IActionResult> ShowBids(string productID)
        {
            var result = await _sellerProcessor.ShowBids(productID);
            return result.ResponseCode switch
            {
                ResponseCode.Success => Ok(result),
                ResponseCode.Error => BadRequest(result),
                _ => StatusCode((int)HttpStatusCode.InternalServerError, result.Errors),
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("products")]
        public async Task<IActionResult> ShowAllProducts()
        {
            var result = await _sellerProcessor.ShowAllProducts();
            return result.ResponseCode switch
            {
                ResponseCode.Success => Ok(result),
                ResponseCode.Error => BadRequest(result),
                _ => StatusCode((int)HttpStatusCode.InternalServerError, result.Errors),
            };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("productbyid/{productID}")]
        public async Task<IActionResult> GetProductById(string productID)
        {
            var result = await _sellerProcessor.GetProductById(productID);
            return result.ResponseCode switch
            {
                ResponseCode.Success => Ok(result),
                ResponseCode.Error => BadRequest(result),
                _ => StatusCode((int)HttpStatusCode.InternalServerError, result.Errors),
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete/{productID}")]
        public async Task<IActionResult> DeletProduct(string productID)
        {
            var result = await _sellerProcessor.DeletProduct(productID);
            return result.ResponseCode switch
            {
                ResponseCode.Success => Ok(result),
                ResponseCode.Error => BadRequest(result),
                _ => StatusCode((int)HttpStatusCode.InternalServerError, result.Errors),
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("showalluser")]
        public async Task<IActionResult> ShowAllUser()
        {

            var result = await _sellerProcessor.ShowAllUser();

            return result.ResponseCode switch
            {
                ResponseCode.Success => Ok(result),
                ResponseCode.Error => BadRequest(result),
                _ => StatusCode((int)HttpStatusCode.InternalServerError, result.Errors),
            };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete-user/{userID}")]
        public async Task<IActionResult> DeleteUser(string userID)
        {
            var result = await _sellerProcessor.DeleteUser(userID);
            return result.ResponseCode switch
            {
                ResponseCode.Success => Ok(result),
                ResponseCode.Error => BadRequest(result),
                _ => StatusCode((int)HttpStatusCode.InternalServerError, result.Errors),
            };
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="userEmail"></param>
       /// <returns></returns>
        [HttpGet]
        [Route("getuserbyemail/{userEmail}")]
        public async Task<IActionResult> GetUserEmail(string userEmail)
        {
            var result = await _sellerProcessor.GetUserByEmail(userEmail);
            return result.ResponseCode switch
            {
                ResponseCode.Success => Ok(result),
                ResponseCode.Error => BadRequest(result),
                _ => StatusCode((int)HttpStatusCode.InternalServerError, result.Errors),
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getuserbyid/{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var result = await _sellerProcessor.GetUserByID(userId);
            return result.ResponseCode switch
            {
                ResponseCode.Success => Ok(result),
                ResponseCode.Error => BadRequest(result),
                _ => StatusCode((int)HttpStatusCode.InternalServerError, result.Errors),
            };
        }
    }
}
