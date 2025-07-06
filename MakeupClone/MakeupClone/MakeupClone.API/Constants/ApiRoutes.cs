namespace MakeupClone.API.Constants;

public static class ApiRoutes
{
    public const string Root = "/";
    public const string ById = "/{id}";
    public const string Discounts = "/{id}/discounts";

    public static class AdminProducts
    {
        public const string Base = "/api/admin/products";
        public const string GetById = ById;
        public const string GetAll = Root;
        public const string Create = Root;
        public const string Update = ById;
        public const string Delete = ById;

        public const string AddDiscount = Discounts;
        public const string UpdateDiscount = Discounts;
        public const string RemoveDiscount = Discounts;
    }

    public static class Auth
    {
        public const string Base = "/api/auth";
        public const string Register = "/register";
        public const string Login = "/login";
        public const string GoogleLogin = "/google-login";
    }

    public static class Brands
    {
        public const string Filter = "/api/brands/filter";
    }

    public static class Categories
    {
        public const string Filter = "/api/categories/filter";
    }

    public static class Products
    {
        public const string Base = "/api/products";
        public const string GetAll = Root;
        public const string Filter = "/filter";
    }

    public static class Orders
    {
        public const string Base = "/api/orders";
        public const string GetById = ById;
        public const string GetAll = Root;
        public const string Create = Root;
        public const string Update = ById;
        public const string Delete = ById;
    }
}