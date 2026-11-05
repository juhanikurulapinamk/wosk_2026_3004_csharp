
// API/Interfaces/IUsersRepository.cs

using API.Models;

namespace API.Interfaces
{

    // huomaa, että IUsersRepository-interfacemme perii IDisposable-interfacen
    public interface IUsersRepository : IDisposable
    {
        public Task<IEnumerable<AppUser>> GetAll();
        public Task<AppUser?> GetById(long id);

        public Task<bool> Save(AppUser user);

        public Task<bool> Remove(AppUser user);
    }
}