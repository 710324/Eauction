using EAuction.Models.Seller;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAuction.Processor.Interface
{
    public interface IProductRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<List<Product>> GetProductsAsync();

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
        Task<Product> CreateOrUpdateAsync(Product product);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(string ProductID);
    }
}
