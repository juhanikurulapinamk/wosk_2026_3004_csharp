
// API/Factories/UsersRepositoryFactory.cs


using API.Repositories;

namespace API.Factories
{
    public static class UsersRepositoryFactory {


        // Kun käytämme IUsersRepository-rajapintaa
        // metodin tietotyyppinä, voimme palauttaa siitä minkä tahansa
        // kyseisen rajapinnan implementoivan luokan instanssin
        public static UsersSQLiteRepository Create() {
            return new UsersSQLiteRepository();
        }
    }
}