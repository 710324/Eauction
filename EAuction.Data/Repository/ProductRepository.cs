using EAuction.Models.API;
using EAuction.Models.Seller;
using EAuction.Processor.Interface;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAuction.Data.Repository
{
    public class ProductRepository : IProductRepository
    {
        public readonly AuctionDBContext _dBContext;
        public ProductRepository(AuctionDBContext dBContext)
        {
            _dBContext = dBContext;
        }
        public async Task<Product> CreateOrUpdateAsync(Product product)
        {
            try
            {
                if (product.ProductId != null && product.ProductId != string.Empty)
                    _dBContext._ProductCollection.ReplaceOne(x => x.ProductId == product.ProductId, product);
                else
                {
                    product.ProductId = Guid.NewGuid().ToString();
                    await _dBContext._ProductCollection.InsertOneAsync(product);
                }
                var newproduct = await _dBContext._ProductCollection.Find(c => c.ProductId == product.ProductId).FirstOrDefaultAsync();
                return newproduct;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteAsync(string ProductID)
        {
            try
            {
                var prod = await _dBContext._ProductCollection.Find(x => x.ProductId == ProductID).FirstOrDefaultAsync();
                if (prod != null)
                {
                    await _dBContext._ProductToBuyerCollection.DeleteManyAsync(x => x.ProductId == ProductID);
                    await _dBContext._ProductCollection.DeleteManyAsync(x => x.ProductId == ProductID);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Product> GetProductByIDAsync(string ProductID)
        {
            try
            {
                var prod = await _dBContext._ProductCollection.Find(x => x.ProductId == ProductID).FirstOrDefaultAsync();
                return prod;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            return await _dBContext._ProductCollection.Find(c => true).ToListAsync();
        }
    }
}
