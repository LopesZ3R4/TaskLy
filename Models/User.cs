//File Path: /Models/User.cs
public class User
{
    public int Id { get; private set; }
    #pragma warning disable CS8618
    public string Username { get; private set; }
    public string Password { get; private set; }
    public string UserType { get; private set; }
    public string Email { get; private set; }
    public string? Token { get; private set; }
    public string CountyCode { get; private set; }

    public User(int id, string username, string password, string userType, string email, string? token, string countyCode)
    {
        Id = id;
        Username = username;
        Password = password;
        UserType = userType;
        Email = email;
        Token = token;
        CountyCode = countyCode;
    }

    public int GetId() { return Id; }
    public void SetId(int id) { Id = id; }

    public string GetUsername() { return Username; }
    public void SetUsername(string username) { Username = username; }

    public string GetPassword() { return Password; }
    public void SetPassword(string password) { Password = password; }

    public string GetUserType() { return UserType; }
    public void SetUserType(string userType) { UserType = userType; }

    public string GetEmail() { return Email; }
    public void SetEmail(string email) { Email = email; }

    public string? GetToken() { return Token; }
    public void SetToken(string? token) { Token = token; }

    public string GetCountyCode() { return CountyCode; }
    public void SetCountyCode(string countyCode) { CountyCode = countyCode; }
}