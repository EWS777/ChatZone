using ChatZone.Core.Extensions;
using ChatZone.DTO.Requests;
using ChatZone.DTO.Responses;

namespace ChatZone.Services.Interfaces;

public interface IAuthService
{ 
    Task<Result<bool>> RegisterPersonAsync(RegisterRequest request);
    Task<Result<RegisterResponse>> ConfirmEmailAsync(int id);
    Task<Result<RegisterResponse>> LoginAsync(string usernameOrEmail, string password);
    Task<Result<RegisterResponse>> RefreshTokenAsync(int id);
    Task<Result<bool>> ResetPasswordAsync(string email);
    Task<Result<RegisterResponse>> UpdatePasswordAsync(int id, string password);
    public Task<Result<UpdateProfileResponse>> UpdateProfileAsync(int id, ProfileRequest profileRequest);
}