using EAuction.Models.API;
using EAuction.Models.Seller;
using EAuction.Processor.Interface;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAuction.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        public readonly AuctionDBContext _dBContext;
        public UserRepository(AuctionDBContext dBContext)
        {
            _dBContext = dBContext;
        }
        public async Task<User> CreateOrUpdateAsync(User user)
        {
            try
            {
                var existUser = await GetUserByEmailAsync(user.Email);

                if (existUser != null)
                {
                    return existUser;
                }

                if (user.UserId != null && user.UserId != string.Empty)
                {
                    _dBContext._userCollection.ReplaceOne(x => x.UserId == user.UserId, user);
                }
                else
                {
                    user.UserId = Guid.NewGuid().ToString();
                    await _dBContext._userCollection.InsertOneAsync(user);
                }

                var updateUser = await GetUserByIDAsync(user.UserId);

                return updateUser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteAsync(string UserId)
        {
            try
            {
                var user = await _dBContext._userCollection.Find(c => c.UserId == UserId).FirstOrDefaultAsync();
                if (user != null)
                {
                    await _dBContext._ProductToBuyerCollection.DeleteManyAsync(x => x.UserID == UserId);
                    await _dBContext._userCollection.DeleteManyAsync(x => x.UserId == UserId);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _dBContext._userCollection.Find(c => c.Email.ToLower() == email.ToLower()).FirstOrDefaultAsync();
                return user;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task<User> GetUserByIDAsync(string userid)
        {
            try
            {
                var user = await _dBContext._userCollection.Find(c => c.UserId == userid).FirstOrDefaultAsync();
                return user;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _dBContext._userCollection.Find(c => true).ToListAsync();
        }
    }
}
