using ChatZone.Core.Extensions;
using ChatZone.Core.Models;
using ChatZone.Core.Models.Enums;
using ChatZone.DTO.Requests;
using ChatZone.DTO.Responses;

namespace ChatZone.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<Result<Person>> GetPersonByEmailAsync(string email, CancellationToken cancellationToken);
    Task<Result<Person>> GetPersonByUsernameAsync(string username, CancellationToken cancellationToken);
    Task<Result<Person>> GetPersonByIdAsync(int id, CancellationToken cancellationToken);
    Task<Result<bool>> IsPersonExistsAsync(int id, CancellationToken cancellationToken);
    Task<Result<Person>> AddPersonAsync(Person person, CancellationToken cancellationToken);
    Task<Result<Person>> UpdatePersonAsync(int id, PersonRole role, CancellationToken cancellationToken);
    Task<Result<Person>> UpdatePersonTokenAsync(int id, string refreshToken, DateTime tokenExp, CancellationToken cancellationToken);
    Task<Result<Person>> UpdatePasswordAsync(int id, string password, CancellationToken cancellationToken);
    Task<Result<UpdateProfileResponse>> UpdateProfileAsync(int id, ProfileRequest profileRequest, CancellationToken cancellationToken);
}