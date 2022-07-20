using EAuction.Models.Seller;
using EAuction.Processor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAuction.Processor.Interface
{
    public interface ISellerProcessor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<ProcessorResponse<User>> AddNewSeller(User user);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task<ProcessorResponse<Product>> AddProduct(Product product);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        Task<ProcessorResponse<ProductBids>> ShowBids(string productID);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<ProcessorResponse<List<Product>>> ShowAllProducts();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<ProcessorResponse<List<User>>> ShowAllUser();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        Task<ProcessorResponse<bool>> DeletProduct(string productID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        Task<ProcessorResponse<Product>> GetProductById(string productID);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task<ProcessorResponse<bool>> DeleteUser(string userID);


       /// <summary>
       /// 
       /// </summary>
       /// <param name="userEmail"></param>
       /// <returns></returns>
        Task<ProcessorResponse<User>> GetUserByEmail(string userEmail);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task<ProcessorResponse<User>> GetUserByID(string userID);
    }
}
