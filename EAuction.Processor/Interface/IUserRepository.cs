using EAuction.Models.Seller;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAuction.Processor.Interface
{
    public interface IUserRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<List<User>> GetUsersAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        Task<User> GetUserByIDAsync(string UserId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<User> GetUserByEmailAsync(string email);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<User> CreateOrUpdateAsync(User user);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(string UserId);
    }
}
