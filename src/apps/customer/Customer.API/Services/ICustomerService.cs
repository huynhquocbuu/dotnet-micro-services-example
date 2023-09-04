namespace Customer.API.Services;

public interface ICustomerService
{
    Task<IResult> GetCustomerByUsernameAsync(string username);
}