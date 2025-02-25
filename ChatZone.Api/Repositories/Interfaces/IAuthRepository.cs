using ChatZone.Core.Models;

namespace ChatZone.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<Person?> GetPersonByEmailAsync(string email);
    Task<Person?> GetPersonByUsernameAsync(string username);
    Task<Person> RegisterPersonAsync(Person person);
}