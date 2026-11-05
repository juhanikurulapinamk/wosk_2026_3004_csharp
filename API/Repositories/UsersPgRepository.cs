using API.Interfaces;
using API.Models;

namespace API.Repositories;

public class UsersPgRepository : IUsersRepository
{
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<AppUser>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<AppUser?> GetById(long id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Remove(AppUser user)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Save(AppUser user)
    {
        throw new NotImplementedException();
    }
}