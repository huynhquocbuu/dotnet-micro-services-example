using Shared.Common.Email;

namespace Contracts.Common.Email;

public interface ISmtpEmailService : IEmailService<MailRequest>
{
    
}