namespace EAuction.Models.Seller
{
    public class BidDetails
    {
        public string BuyerProductId { get; set; }
        public string ProductId { get; set; }
        public string UserID { get; set; }
        public decimal BidAmount { get; set; }
        public string BuyerName { get; set; }
        public string EmailId { get; set; }
        public string Phone { get; set; }
    }
}
