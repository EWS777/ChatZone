﻿using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Core.Models;
using ChatZone.Core.Models.Enums;
using ChatZone.DTO.Requests;
using ChatZone.DTO.Responses;
using ChatZone.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Repositories;

public class AuthRepository(ChatZoneDbContext dbContext) : IAuthRepository
{
    public async Task<Result<Person>> GetPersonByEmailAsync(string email)
    {
        var person = await dbContext.Persons.SingleOrDefaultAsync(x => x.Email == email);
        if(person is null) return Result<Person>.Failure(new NotFoundException("Email is not found!"));
        return Result<Person>.Ok(person);
    }

    public async Task<Result<Person>> GetPersonByUsernameAsync(string username)
    {
        var person = await dbContext.Persons.SingleOrDefaultAsync(x => x.Username == username);
        if (person is null) return Result<Person>.Failure(new NotFoundException("Username is not found!"));
        return Result<Person>.Ok(person);
    }

    public async Task<Result<Person>> AddPersonAsync(Person person)
    {
        var usernameResult =  await GetPersonByUsernameAsync(person.Username);
        
        if (usernameResult.IsSuccess) return Result<Person>.Failure(new ExistPersonException("The username is exist!"));
        
        var emailResult =  await GetPersonByEmailAsync(person.Email);
        
        if (emailResult.IsSuccess) return Result<Person>.Failure(new ExistPersonException("The email is exist!"));
        
        
        await dbContext.Persons.AddAsync(person);
        await dbContext.SaveChangesAsync();
        return Result<Person>.Ok(person);
    }

    public async Task<Result<Person>> UpdatePersonAsync(string username, PersonRole role)
    {
        var person = await GetPersonByUsernameAsync(username);
        if (!person.IsSuccess) return Result<Person>.Failure(person.Exception);

        person.Value.Role = role;
        dbContext.Persons.Update(person.Value);
        await dbContext.SaveChangesAsync();
        return Result<Person>.Ok(person.Value);
    }

    public async Task<Result<Person>> UpdatePersonTokenAsync(string username, string refreshToken, DateTime tokenExp)
    {
        var person = await GetPersonByUsernameAsync(username);

        person.Value.RefreshToken = refreshToken;
        person.Value.RefreshTokenExp = tokenExp;
        dbContext.Persons.Update(person.Value);
        await dbContext.SaveChangesAsync();
        return Result<Person>.Ok(person.Value);
    }

    public async Task<Result<Person>> UpdatePasswordAsync(string username, string password)
    {
        var person = await GetPersonByUsernameAsync(username);

        person.Value.Password = password;

        dbContext.Persons.Update(person.Value);
        await dbContext.SaveChangesAsync();
        return Result<Person>.Ok(person.Value);
    }
    
    public async Task<Result<UpdateProfileResponse>> UpdateProfileAsync(string username, ProfileRequest profileRequest)
    {
        var person = await GetPersonByUsernameAsync(username);

        person.Value.Username = profileRequest.Username;
        person.Value.IsFindByProfile = profileRequest.IsFindByProfile;

        dbContext.Persons.Update(person.Value);
        await dbContext.SaveChangesAsync();

        return Result<UpdateProfileResponse>.Ok(new UpdateProfileResponse
        {
            Username = person.Value.Username,
            IsFindByProfile = person.Value.IsFindByProfile
        });
    }
}