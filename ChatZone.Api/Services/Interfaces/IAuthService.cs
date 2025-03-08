using System.Security.Claims;
using ChatZone.Core.Extensions;
using ChatZone.DTO.Requests;
using ChatZone.DTO.Responses;

namespace ChatZone.Services.Interfaces;

public interface IAuthService
{ 
    Task<Result> RegisterPersonAsync(RegisterRequest request);
    Task<Result<RegisterResponse>> ConfirmEmailAsync(ClaimsPrincipal claimsPrincipal);
}