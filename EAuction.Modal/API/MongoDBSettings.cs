

namespace EAuction.Models.API
{
    public class MongoDBSettings
    {
        public string ConnectionURI { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string UserCollection { get; set; } = null!;
        public string ProductCollection { get; set; } = null!;
        public string ProductToBuyerCollection { get; set; } = null!;
    }
}
