using EAuction.Models.API;
using EAuction.Models.Seller;
using EAuction.Processor.Interface;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAuction.Data.Repository
{
    public class ProductToBuyerRepository : IProductToBuyerRepository
    {
        public readonly AuctionDBContext _dBContext;
        public ProductToBuyerRepository(AuctionDBContext dBContext)
        {
            _dBContext = dBContext;
        }
        public async Task<ProductToBuyer> CreateOrUpdateAsync(ProductToBuyer product)
        {
            try
            {

                if (product.BuyerProductId != null && product.BuyerProductId != string.Empty)
                {
                    _dBContext._ProductToBuyerCollection.ReplaceOne(x => x.BuyerProductId == product.BuyerProductId, product);
                }
                else
                {
                    product.BuyerProductId = Guid.NewGuid().ToString();
                    await _dBContext._ProductToBuyerCollection.InsertOneAsync(product);
                }
                return product;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteAsync(string ProductID, string userID)
        {
            try
            {
                var prod = await _dBContext._ProductToBuyerCollection.Find(x => x.BuyerProductId == ProductID && x.UserID == userID).FirstOrDefaultAsync();
                if (prod != null)
                {
                    await _dBContext._ProductToBuyerCollection.DeleteManyAsync(x => x.ProductId == ProductID && x.UserID == userID);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ProductToBuyer>> GetBidByProductIDAsync(string productId)
        {
            try
            {
                var prod = await _dBContext._ProductToBuyerCollection.Find(x => x.ProductId == productId).ToListAsync();
                return prod;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task<ProductToBuyer> GetProductByUserIDAsync(string productId, string userID)
        {
            try
            {
                var prod = await _dBContext._ProductToBuyerCollection.Find(x => x.ProductId == productId && x.UserID == userID).FirstOrDefaultAsync();
                return prod;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task<ProductToBuyer> GetProductBidByBuyerProductId(string buyerProductId)
        {
            try
            {
                var prod = await _dBContext._ProductToBuyerCollection.Find(x => x.BuyerProductId == buyerProductId).FirstOrDefaultAsync();
                return prod;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task<List<ProductToBuyer>> GetProductsByBuyerIdAsync()
        {
            return await _dBContext._ProductToBuyerCollection.Find(c => true).ToListAsync();
        }
    }
}
