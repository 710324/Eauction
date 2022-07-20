using EAuction.Models.Seller;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAuction.Processor.Interface
{
    public interface IProductToBuyerRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<List<ProductToBuyer>> GetProductsByBuyerIdAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<List<ProductToBuyer>> GetBidByProductIDAsync(string productId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        Task<ProductToBuyer> GetProductByUserIDAsync(string productId, string UserID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task<ProductToBuyer> CreateOrUpdateAsync(ProductToBuyer product);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProductID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(string ProductID, string userID);


        /// <summary>
        /// /
        /// </summary>
        /// <param name="buyerProductId"></param>
        /// <returns></returns>
        Task<ProductToBuyer> GetProductBidByBuyerProductId(string buyerProductId);
    }
}
