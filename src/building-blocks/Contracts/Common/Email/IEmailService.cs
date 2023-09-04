namespace Contracts.Common.Email;

public interface IEmailService<in T> where T: class
{
    Task SendEmailAsync(T request, CancellationToken cancellationToken = new CancellationToken());
}