namespace MakeupClone.API.Constants;

public static class ApiRoutes
{
    public static class AdminProducts
    {
        public const string Base = "/api/admin/products";
        public const string GetById = "/{id}";
        public const string GetAll = "/";
        public const string Create = "/";
        public const string Update = "/{id}";
        public const string Delete = "/{id}";

        public const string AddDiscount = "/{id}/discounts";
        public const string UpdateDiscount = "/{id}/discounts";
        public const string RemoveDiscount = "/{id}/discounts";
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
        public const string GetAll = "/";
        public const string Filter = "/filter";
    }

    public static class Orders
    {
        public const string Base = "api/orders";
        public const string GetById = "/{id}";
        public const string GetAll = "/";
        public const string Create = "/";
        public const string Update = "/{id}";
        public const string Delete = "/{id}";
    }
}