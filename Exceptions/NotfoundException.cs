// Owner: Reyes | Type: class (exception -> HTTP 404)
namespace GymApi.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}
