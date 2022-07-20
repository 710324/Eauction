using EAuction.Models.Seller;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EAuction.Models.API
{
    public class AuctionDBContext
    {
        public readonly IMongoCollection<User> _userCollection;
        public readonly IMongoCollection<Product> _ProductCollection;
        public readonly IMongoCollection<ProductToBuyer> _ProductToBuyerCollection;
        public AuctionDBContext(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _userCollection = database.GetCollection<User>(mongoDBSettings.Value.UserCollection);
            _ProductCollection = database.GetCollection<Product>(mongoDBSettings.Value.ProductCollection);
            _ProductToBuyerCollection = database.GetCollection<ProductToBuyer>(mongoDBSettings.Value.ProductToBuyerCollection);
        }
    }
}
