namespace ShopSite.Models
{
    public interface IEmailSender
    {
        Task SendAsync(string senderName, string to, string subject, string body);
    }
}
