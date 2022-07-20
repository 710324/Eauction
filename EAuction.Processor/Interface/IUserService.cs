using EAuction.Models.Seller;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAuction.Processor.Interface
{
    public interface IUserService
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<User> CreateOrUpdateAsync(User user);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<List<User>> GetUsersAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(string userID);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task<User> GetUserByIdAsync(string userID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        Task<User> GetUserByEmailAsync(string userEmail);
    }
}
