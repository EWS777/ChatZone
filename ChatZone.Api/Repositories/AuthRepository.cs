using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Core.Models;
using ChatZone.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Repositories;

public class AuthRepository(ChatZoneDbContext dbContext) : IAuthRepository
{
    public async Task<Result<Person>> GetPersonByEmailAsync(string email)
    {
        var person = await dbContext.Persons.SingleOrDefaultAsync(x => x.Email == email);
        if (person is null) return Result<Person>.FailResultT(new NotFoundException("Email not found!"));
        return Result<Person>.Ok(person);
    }

    public async Task<Result<Person>> GetPersonByUsernameAsync(string username)
    {
        var person = await dbContext.Persons.SingleOrDefaultAsync(x => x.Username == username);
        if (person is null) return Result<Person>.FailResultT(new NotFoundException("Username not found!"));
        return Result<Person>.Ok(person);
    }

    public async Task<Result<Person>> AddPersonAsync(Person person)
    {
        var usernameResult =  await GetPersonByUsernameAsync(person.Username);
        
        if (usernameResult.IsSuccess) return Result<Person>.FailResultT(new ExistPersonException("The username is exist!"));
        
        var emailResult =  await GetPersonByEmailAsync(person.Email);
        
        if (emailResult.IsSuccess) return Result<Person>.FailResultT(new ExistPersonException("The email is exist!"));
        
        
        await dbContext.Persons.AddAsync(person);
        await dbContext.SaveChangesAsync();
        return Result<Person>.Ok(person);
    }

    public async Task<Result<Person>> UpdatePersonAsync(Person person)
    {
        var getPerson = GetPersonByUsernameAsync(person.Username);
        if (!getPerson.Result.IsSuccess)
            return Result<Person>.FailResultT(new NotFoundException("Username is not found!"));
        // dbContext.Persons.Attach(person);


    }
}