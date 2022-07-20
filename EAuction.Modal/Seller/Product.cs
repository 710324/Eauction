
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace EAuction.Models.Seller
{
    public class Product
    {
        [BsonId]
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ShortDeceription { get; set; }
        public string DetailedDeceription { get; set; }
        public string Category { get; set; }
        public DateTime BidEndDate { get; set; }
        public decimal StartingPrice { get; set; }
    }
}
