using Google.Apis.Auth;

namespace MakeupClone.Application.Interfaces;

public interface IGoogleJsonWebSignatureWrapper
{
    Task<GoogleJsonWebSignature.Payload> Validate(string idToken, CancellationToken cancellationToken = default);
}