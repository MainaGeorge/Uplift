namespace Uplift.Utility
{
    public static class AppConstants
    {
        public static string ShoppingCart = "Shopping Cart";
        public static string StatusSubmitted { get; set; } = "submitted";
        public static string StatusApproved { get; set; } = "approved";
        public static string StatusRejected { get; set; } = "rejected";
        public static string Admin { get; set; } = "Admin";
        public static string Manager { get; set; } = "Manager";

        public static string GetCategoryStoredProcedure { get; set; } = "GET_CATEGORIES";
    }
}
