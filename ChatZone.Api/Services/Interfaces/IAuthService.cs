using ChatZone.Core.DTO.Requests;
using ChatZone.Core.DTO.Responses;

namespace ChatZone.Services.Interfaces;

public interface IAuthService
{ 
    Task<RegisterResponse> RegisterPersonAsync(RegisterRequest request);
}