using ChatZone.Core.Extensions;
using ChatZone.Core.Models;
using ChatZone.Core.Models.Enums;
using ChatZone.DTO.Requests;
using ChatZone.DTO.Responses;

namespace ChatZone.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<Result<Person>> GetPersonByEmailAsync(string email);
    Task<Result<Person>> GetPersonByUsernameAsync(string username);
    Task<Result<Person>> GetPersonByIdAsync(int id);
    Task<Result<bool>> IsPersonExistsAsync(int id);
    Task<Result<Person>> AddPersonAsync(Person person);
    Task<Result<Person>> UpdatePersonAsync(int id, PersonRole role);
    Task<Result<Person>> UpdatePersonTokenAsync(int id, string refreshToken, DateTime tokenExp);
    Task<Result<Person>> UpdatePasswordAsync(int id, string password);
    Task<Result<UpdateProfileResponse>> UpdateProfileAsync(int id, ProfileRequest profileRequest);
}