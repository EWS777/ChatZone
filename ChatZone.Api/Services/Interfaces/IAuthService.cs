using ChatZone.Core.DTO.Requests;
using ChatZone.Core.Extensions;

namespace ChatZone.Services.Interfaces;

public interface IAuthService
{ 
    Task<Result> RegisterPersonAsync(RegisterRequest request);
}