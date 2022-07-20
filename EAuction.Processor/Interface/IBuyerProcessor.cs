using EAuction.Models.Seller;
using EAuction.Processor.Models;
using System.Threading.Tasks;

namespace EAuction.Processor.Interface
{
    public interface IBuyerProcessor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<ProcessorResponse<User>> AddNewBuyer(User user);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task<ProcessorResponse<ProductToBuyer>> PlaceBid(BuyerBid product);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task<ProcessorResponse<ProductToBuyer>> UpdateBid(BuyerBid product);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buyerProductId"></param>
        /// <returns></returns>
        Task<ProcessorResponse<BuyerBid>> GetProductBidByBuyerProductId(string buyerProductId);
    }


}
