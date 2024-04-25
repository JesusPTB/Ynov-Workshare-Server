namespace Ynov_WorkShare_Server.DTO;

public class UpdatePwdDto
{
    public UpdatePwdDto(string email, string currentPassword, string newPassword)
    {
        Email = email;
        CurrentPassword = currentPassword;
        NewPassword = newPassword;
    }

    public string Email { get; set; }

    public string CurrentPassword { get; set; }

    public string NewPassword { get; set; }
}