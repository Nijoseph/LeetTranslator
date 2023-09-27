using LeetTranslator.Models;

public interface IUserDataServices
{
    Task DeleteUserAsync (int userId);
    Task<IEnumerable<UserAccount>> GetAllUsersAsync ();
    Task<UserAccount> GetUserByIdAsync (int userId);
    Task<UserAccount> GetUserByUserNameAsync (string UserName);
    Task<int> InsertUserAsync (UserAccount user);
    Task UpdateUserAsync (UserAccount user);

    string HashPassword(string password, string salt);
}