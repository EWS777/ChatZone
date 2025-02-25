using ChatZone.Context;
using ChatZone.Core.Models;
using ChatZone.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Repositories;

public class AuthRepository(ChatZoneDbContext dbContext) : IAuthRepository
{
    public async Task<Person?> GetPersonByEmailAsync(string email)
    {
        var person = await dbContext.Persons.SingleOrDefaultAsync(x => x.Email == email);
        return person;
    }

    public async Task<Person?> GetPersonByUsernameAsync(string username)
    {
        var person = await dbContext.Persons.SingleOrDefaultAsync(x => x.Username == username);
        return person;
    }

    public async Task<Person> RegisterPersonAsync(Person person)
    {
        await dbContext.Persons.AddAsync(person);
        await dbContext.SaveChangesAsync();
        return person;
    }
}