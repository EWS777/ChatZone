using ChatZone.DTO.Requests;
using ChatZone.DTO.Responses;

namespace ChatZone.Services.Interfaces;

public interface IAuthService
{ 
    Task<RegisterResponse> RegisterPersonAsync(RegisterRequest request);
}