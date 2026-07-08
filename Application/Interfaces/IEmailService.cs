namespace Application.Interfaces
{
    public interface IEmailService
    {
        Task SendGlobalNotificationAsync(string subject, string messageBody, List<string> recipients);
    }
}