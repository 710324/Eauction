using EAuction.Models.Seller;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAuction.Processor.Interface
{
    public interface IProductService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<List<Product>> GetProductsAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task<Product> CreateOrUpdateProductAsync(Product product);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(string ProductID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        Task<Product> GetProductByIDAsync(string ProductID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task<ProductToBuyer> AddBidForProductAsync(BuyerBid product);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        Task<ProductBids> GetbidsByProductID(string ProductID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task<ProductToBuyer> UpdateBidForProduct(BuyerBid product);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buyerProductId"></param>
        /// <returns></returns>
        Task<BuyerBid> GetProductBidByBuyerProductId(string buyerProductId);
    }
}
