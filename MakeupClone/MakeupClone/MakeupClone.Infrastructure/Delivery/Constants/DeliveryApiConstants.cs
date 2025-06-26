namespace MakeupClone.Infrastructure.Delivery.Constants;

public static class DeliveryApiConstants
{
    public const string HeaderContentType = "Content-Type";
    public const string ContentTypeJson = "application/json";

    public const string HeaderAuthorization = "Authorization";
    public const string AuthorizationSchemeBearer = "Bearer ";
    public const string HeaderXApiKey = "X-Api-Key";

    public const string ApiSegment = "api";
    public const string ShipmentsSegment = "shipments";
    public const string TrackingSegment = "tracking";
    public const string BackofficeSegment = "backoffice";
}