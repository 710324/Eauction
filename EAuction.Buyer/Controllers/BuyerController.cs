
using EAuction.Models.Enum;
using EAuction.Models.Seller;
using EAuction.Processor.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace EAuctionBuyer.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("e-auction-buyer/api/v{v:apiVersion}/[controller]")]
    public class BuyerController : ControllerBase
    {
        private readonly IBuyerProcessor _buyerProcessor;
        public BuyerController(IBuyerProcessor buyerProcessor)
        {
            if (buyerProcessor is null)
            {
                throw new ArgumentException(nameof(buyerProcessor));
            }
            _buyerProcessor = buyerProcessor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("place-bid")]
        public async Task<IActionResult> PlaceBid(BuyerBid product)
        {
            var result = await _buyerProcessor.PlaceBid(product);
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
        /// <param name="ProductId"></param>
        /// <param name="buyerEmailId"></param>
        /// <param name="newBidAmount"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update-bid")]
        public async Task<IActionResult> UpdateBid(BuyerBid product)
        {
            var result = await _buyerProcessor.UpdateBid(product);
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
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("addbuyer")]
        public async Task<IActionResult> AddNewBuyer(User user)
        {
            user.UserType = "2";

            var result = await _buyerProcessor.AddNewBuyer(user);

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
        /// <param name="productBidId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getproductbidbyid/{productBidId}")]
        public async Task<IActionResult> GetProductBidByBuyerProductId(string productBidId)
        {
            var result = await _buyerProcessor.GetProductBidByBuyerProductId(productBidId);
            return result.ResponseCode switch
            {
                ResponseCode.Success => Ok(result),
                ResponseCode.Error => BadRequest(result),
                _ => StatusCode((int)HttpStatusCode.InternalServerError, result.Errors),
            };
        }
    }
}

