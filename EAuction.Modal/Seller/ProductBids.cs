using System;
using System.Collections.Generic;

namespace EAuction.Models.Seller
{
    public class ProductBids
    {
        public string ProductName { get; set; }
        public string ShortDescription { get; set; }
        public string DetailedDeceription { get; set; }
        public string Category { get; set; }
        public DateTime BidEndDate { get; set; }
        public decimal StartingPrice { get; set; }
        public List<BidDetails> BidDetails { get; set; }
    }
}
