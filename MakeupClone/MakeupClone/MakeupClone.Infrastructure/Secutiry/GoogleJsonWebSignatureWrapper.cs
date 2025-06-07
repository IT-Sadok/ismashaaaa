using Google.Apis.Auth;
using MakeupClone.Application.Interfaces;

namespace MakeupClone.Infrastructure.Secutiry;

public class GoogleJsonWebSignatureWrapper : IGoogleJsonWebSignatureWrapper
{
    public Task<GoogleJsonWebSignature.Payload> Validate(string idToken, CancellationToken cancellationToken = default)
    {
        return GoogleJsonWebSignature.ValidateAsync(idToken);
    }
}