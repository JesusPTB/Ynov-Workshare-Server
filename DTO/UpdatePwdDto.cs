namespace Ynov_WorkShare_Server.DTO;

public class UpdatePwdDto
{
    public string Email { get; set; }
    
    public string CurrentPassword { get; set; }
    
    public string NewPassword { get; set; }

    public UpdatePwdDto(string email, string currentPassword, string newPassword)
    {
        this.Email = email;
        this.CurrentPassword = currentPassword;
        this.NewPassword = newPassword;
    }
}