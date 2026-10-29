using API.Models;
using Microsoft.Data.Sqlite;

namespace API.Repositories;

public class UsersSQLiteRepository : IDisposable {
    
    private readonly SqliteConnection _connection;

    public UsersSQLiteRepository()
    {
        _connection = new SqliteConnection("Data Source=tuntiharjoitus1.db");
        _connection.Open();
    }

    public void Dispose()
    {
        if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
            }
    }

    public async Task<IEnumerable<AppUser>> GetAll()
    {


        // luodaan tyhjä lista tietokannan rivejä varten
        var users = new List<AppUser>();
        // luodaan yhteys ja avataan se
        using var connection = new SqliteConnection("Data source=tuntiharjoitus1.db");

        connection.Open();


        // luodaan sql-komento ja suoritetaan se
        var command = connection.CreateCommand();
        command.CommandText = $"SELECT * FROM users";

        // Jälkeen ExecuteReadeAsync, koska odotetaan rivejä vataukseksi tietokannasta
        using var reader = await command.ExecuteReaderAsync();





        // luetaan luupissa kaikki rivit, jotka kyselyllä saadaan ulos
        // tietokannasta
        while (await reader.ReadAsync())
        {
            // lisätään ne ylempänä luotuun listaan
            users.Add(new AppUser
            {
                Id = reader.GetInt64(0),
                Firstname = reader.GetString(1),
                Lastname = reader.GetString(2),
                Username = reader.GetString(3)
            });
        }

        return users;



    }

    public async Task<AppUser?> GetById(long id)
    {


        
        AppUser? user;
        // luodaan yhteys ja avataan se
        using var connection = new SqliteConnection("Data source=tuntiharjoitus1.db");

        connection.Open();


        // luodaan sql-komento ja suoritetaan se
        var command = connection.CreateCommand();
        command.CommandText = $"SELECT * FROM users WHERE id = @id";
        command.Parameters.AddWithValue("@id", id);

        // Jälkeen ExecuteReadeAsync, koska odotetaan rivejä vataukseksi tietokannasta
        using var reader = await command.ExecuteReaderAsync();

        if (!reader.HasRows)
        {
            return null;
        }





        // luetaan luupissa kaikki rivit, jotka kyselyllä saadaan ulos
        // tietokannasta
        await reader.ReadAsync();

        // lisätään ne ylempänä luotuun listaan
        user = new AppUser
        {
            Id = reader.GetInt64(0),
            Firstname = reader.GetString(1),
            Lastname = reader.GetString(2),
            Username = reader.GetString(3)
        };


        return user;



    }

    private async Task<bool> Add(AppUser user)
    {
        using var connection = new SqliteConnection("Data source=tuntiharjoitus1.db");
        connection.Open();

        var insertCommand = connection.CreateCommand();
        insertCommand.CommandText = @"
            INSERT INTO users(first_name, last_name, username)
            VALUES(@firstName, @lastName, @username);
            SELECT last_insert_rowid();";
        insertCommand.Parameters.AddWithValue("@firstName", user.Firstname);
        insertCommand.Parameters.AddWithValue("@lastName", user.Lastname);
        insertCommand.Parameters.AddWithValue("@username", user.Username);

        var newId = await insertCommand.ExecuteScalarAsync();
        if (newId == null)
        {
            return false;
        }

        user.Id = (long)newId;

        return true;



    }

    private async Task<bool> Update(AppUser user)
    {
        using var connection = new SqliteConnection("Data source=tuntiharjoitus1.db");
        connection.Open();

        // luodaan sql-komento päivitystä varten ja asetetaan sille yo. parametrit
        var updateCommand = connection.CreateCommand();
        updateCommand.CommandText = @"
            UPDATE users SET
                first_name = @newFirstName,
                last_name = @newLastName,
                username = @newUsername
            WHERE id = @id;";

        // Koska pääsemme modelin metodin sisällä luokan instanssin propertyihin käsiksi,
        // metodille ei tarvitse lähettää erillisiä parametrejä
        updateCommand.Parameters.AddWithValue("@newFirstName", user.Firstname);
        updateCommand.Parameters.AddWithValue("@newLastName", user.Lastname);
        updateCommand.Parameters.AddWithValue("@newUsername", user.Username);
        updateCommand.Parameters.AddWithValue("@id", user.Id);

        int rowsAffected = await updateCommand.ExecuteNonQueryAsync();

        return rowsAffected == 1;




    }

    public async Task<bool> Save(AppUser user)
    {
        bool success;
        if (user.Id == null)
        {
            success = await Add(user);
        }
        else
        {
            success = await Update(user);
        }

        return success;


    }

    public async Task<bool> Remove(AppUser user)
    {
        using var connection = new SqliteConnection("Data source=tuntiharjoitus1.db");
        
        connection.Open();

        // luodaan sql-komento poistoa varten
        var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM users WHERE id = @id;";
        command.Parameters.AddWithValue("@id", user.Id);


        // tässä voidaan käyttää ExecuteNonQueryAsyncia,
        // koska DELETE-kysely ei palauta varsinaista tulosta tietokannasta
        int rowsAffected = await command.ExecuteNonQueryAsync();


        return rowsAffected == 1;
        
        
        
    }
}

    
