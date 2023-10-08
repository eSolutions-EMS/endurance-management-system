using Core.ConventionalServices;

namespace Core.Application.Services;

public interface INotificationService
{
    void Error(string message);
}
