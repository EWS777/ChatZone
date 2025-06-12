using ChatZone.Core.Extensions;
using ChatZone.DTO.Requests;
using ChatZone.DTO.Responses;

namespace ChatZone.Services.Interfaces;

public interface IAuthService
{ 
    Task<Result<bool>> RegisterPersonAsync(RegisterRequest request, CancellationToken cancellationToken);
    Task<Result<RegisterResponse>> ConfirmEmailAsync(int id, CancellationToken cancellationToken);
    Task<Result<RegisterResponse>> LoginAsync(string usernameOrEmail, string password, CancellationToken cancellationToken);
    Task<Result<RegisterResponse>> RefreshTokenAsync(int id, CancellationToken cancellationToken);
    Task<Result<bool>> ResetPasswordAsync(string email, CancellationToken cancellationToken);
    Task<Result<RegisterResponse>> UpdatePasswordAsync(int id, string password, CancellationToken cancellationToken);
    public Task<Result<UpdateProfileResponse>> UpdateProfileAsync(int id, ProfileRequest profileRequest, CancellationToken cancellationToken);
}