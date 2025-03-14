﻿using ChatZone.Core.Extensions;
using ChatZone.Core.Models;
using ChatZone.Core.Models.Enums;

namespace ChatZone.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<Result<Person>> GetPersonByEmailAsync(string email);
    Task<Result<Person>> GetPersonByUsernameAsync(string username);
    Task<Result<Person>> AddPersonAsync(Person person);

    Task<Result<Person>> UpdatePersonAsync(string username, PersonRole role);
    Task<Result<Person>> UpdatePersonTokenAsync(string username, string refreshToken, DateTime tokenExp);
    Task<Result<Person>> UpdatePasswordAsync(string username, string password);
}