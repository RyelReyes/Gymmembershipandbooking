// Owner: Reyes | Type: class (exception -> HTTP 409 Conflict)
namespace GymApi.Exceptions;

/// <summary>Thrown by services when a business rule blocks the request (class full, duplicate booking, ...).</summary>
public class BusinessRuleException : Exception
{
    public BusinessRuleException(string message) : base(message) { }
}
