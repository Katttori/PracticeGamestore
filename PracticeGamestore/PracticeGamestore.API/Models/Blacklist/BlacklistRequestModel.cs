namespace PracticeGamestore.Models.Blacklist;

public class BlacklistRequestModel
{
    public string UserEmail { get; set; }
    
    public BlacklistRequestModel(string userEmail)
    {
        UserEmail = userEmail;
    }
}