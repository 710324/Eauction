using EAuction.Models;
using EAuction.Models.Enum;
using EAuction.Models.Seller;
using EAuction.Processor.Exceptions;
using EAuction.Processor.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAuction.Processor.Services
{
    public class UserService : IUserService
    {
        public readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            if (userRepository is null)
            {
                throw new ArgumentException(nameof(userRepository));
            }

            _userRepository = userRepository;
        }

        public async Task<User> CreateOrUpdateAsync(User user)
        {
            if (user.UserId != null)
            {
                var existingUser = await _userRepository.GetUserByIDAsync(user.UserId);

                if (existingUser == null)
                {
                    throw new UserNotFounException(Constants.UserNotFound);
                }

                var updateduser = await _userRepository.CreateOrUpdateAsync(user);
                return updateduser;
            }
            else
            {
                var newUser = await _userRepository.CreateOrUpdateAsync(user);
                newUser.UserType = newUser.UserType == "1" ? UserType.Seller.ToString() : UserType.Buyer.ToString();
                return newUser;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<User>> GetUsersAsync()
        {
            var users = await _userRepository.GetUsersAsync();

            users.ForEach(item =>
            {
                item.UserType = UserTypeNameById(item.UserType);
            });

            return users;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(string userID)
        {
            var existingProduct = await _userRepository.GetUserByIDAsync(userID);
            if (existingProduct == null)
            {
                throw new UserNotFounException($"{Constants.UserNotFound}");
            }

            return await _userRepository.DeleteAsync(userID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<User> GetUserByIdAsync(string userID)
        {
            var user = await _userRepository.GetUserByIDAsync(userID);

            user.UserType = UserTypeNameById(user.UserType);

            return user;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        public async Task<User> GetUserByEmailAsync(string userEmail)
        {
            var user = await _userRepository.GetUserByEmailAsync(userEmail);

            user.UserType = UserTypeNameById(user.UserType);

            return user;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userType"></param>
        /// <returns></returns>
        private string UserTypeNameById(string userType)
        {
            var result = string.Empty;
            switch (Convert.ToInt32(userType))
            {
                case (int)UserType.Seller:
                    {
                        result = UserType.Seller.ToString();
                        break;
                    }
                case (int)UserType.Buyer:
                    {
                        result = UserType.Buyer.ToString();
                        break;
                    }
            }
            return result;
        }
    }
}
