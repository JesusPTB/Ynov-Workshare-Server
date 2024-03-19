namespace Ynov_WorkShare_Server.DTO;

public class LoginForm
{
    public string Email { get; set; }
    public string Password { get; set; }

    public LoginForm(string email, string password)
    {
        this.Email = email;
        this.Password = password;
    }
}