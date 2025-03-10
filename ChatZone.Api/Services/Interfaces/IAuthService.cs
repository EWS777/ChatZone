using ChatZone.Core.Extensions;
using ChatZone.DTO.Requests;
using ChatZone.DTO.Responses;

namespace ChatZone.Services.Interfaces;

public interface IAuthService
{ 
    Task<Result<bool>> RegisterPersonAsync(RegisterRequest request);
    Task<Result<RegisterResponse>> ConfirmEmailAsync(string username);
    Task<Result<RegisterResponse>> LoginAsync(string usernameOrEmail, string password);
}