namespace Ynov_WorkShare_Server.DTO;

public class LoginForm
{
    public LoginForm(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public string Email { get; set; }
    public string Password { get; set; }
}