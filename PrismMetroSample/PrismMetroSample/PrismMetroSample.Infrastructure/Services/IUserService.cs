using PrismMetroSample.Infrastructure.Interceptor.HandlerAttributes;
using PrismMetroSample.Infrastructure.Models;
using System.Collections.Generic;

namespace PrismMetroSample.Infrastructure.Services
{
    [LogHandler]
   public interface IUserService
    {
        List<User> GetAllUsers();
    }
}
