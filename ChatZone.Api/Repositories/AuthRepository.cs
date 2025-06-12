using ChatZone.Context;
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

    public async Task<Result<Person>> GetPersonByIdAsync(int id)
    {
        var person = await dbContext.Persons.SingleOrDefaultAsync(x => x.IdPerson == id);
        return person is null ? Result<Person>.Failure(new NotFoundException("User is not found!")) : Result<Person>.Ok(person);
    }

    public async Task<Result<bool>> IsPersonExistsAsync(int id)
    {
        var person = await dbContext.Persons.AnyAsync(x => x.IdPerson == id);
        return person ? Result<bool>.Ok(true) : Result<bool>.Failure(new NotFoundException("User is not found!"));
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

    public async Task<Result<Person>> UpdatePersonAsync(int id, PersonRole role)
    {
        var person = await GetPersonByIdAsync(id);
        if (!person.IsSuccess) return Result<Person>.Failure(person.Exception);

        person.Value.Role = role;
        dbContext.Persons.Update(person.Value);
        await dbContext.SaveChangesAsync();
        return Result<Person>.Ok(person.Value);
    }

    public async Task<Result<Person>> UpdatePersonTokenAsync(int id, string refreshToken, DateTime tokenExp)
    {
        var person = await GetPersonByIdAsync(id);

        person.Value.RefreshToken = refreshToken;
        person.Value.RefreshTokenExp = tokenExp;
        dbContext.Persons.Update(person.Value);
        await dbContext.SaveChangesAsync();
        return Result<Person>.Ok(person.Value);
    }

    public async Task<Result<Person>> UpdatePasswordAsync(int id, string password)
    {
        var person = await GetPersonByIdAsync(id);

        person.Value.Password = password;

        dbContext.Persons.Update(person.Value);
        await dbContext.SaveChangesAsync();
        return Result<Person>.Ok(person.Value);
    }
    
    public async Task<Result<UpdateProfileResponse>> UpdateProfileAsync(int id, ProfileRequest profileRequest)
    {
        var person = await GetPersonByIdAsync(id);

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