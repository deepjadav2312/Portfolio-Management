namespace PortfolioManagement_Utility
{
    public class SD
    {

        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }

        public static string SessionToken = "JWTToken";

        public const string SD_Storage_Container = "redmango";

        ////public const string Role_Customer = "Customer";
        ////public const string Role_Company = "Company";
        ////public const string Role_Admin = "Admin";
        ////public const string Role_Employee = "Employee";

        //public const string StatusPending = "Pending";
        //public const string StatusApproved = "Approved";
        //public const string StatusInProcess = "Processing";
        //public const string StatusShipped = "Shipped";
        //public const string StatusCancelled = "Cancelled";
        //public const string StatusRefunded = "Refunded";

        //public const string PaymentStatusPending = "Pending";
        //public const string PaymentStatusApproved = "Approved";
        //public const string PaymentStatusDelayedPayment = "ApprovedForDelayedPayment";
        //public const string PaymentStatusRejected = "Rejected";

        //public const string SessionCart = "SessionShoppingCart";

    }
}