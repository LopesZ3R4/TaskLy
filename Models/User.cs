//File Path: /Models/User.cs

public class User
{
    public int Id { get; set; }
    #pragma warning disable CS8618
    public string Username { get; set; }
    public string Password { get; set; }
    public string UserType { get; set; }
    public string Email {get; set;}
    public string? Token { get; set; }    
}