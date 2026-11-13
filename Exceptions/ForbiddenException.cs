// Owner: Reyes | Type: class (exception -> HTTP 403)
namespace GymApi.Exceptions;

public class ForbiddenException : Exception
{
    public ForbiddenException(string message) : base(message) { }
}
