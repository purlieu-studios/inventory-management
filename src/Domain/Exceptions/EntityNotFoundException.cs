namespace Domain.Exceptions;

public class EntityNotFoundException(string resource, object key)
    : Exception($"{resource} with key '{key}' was not found.")
{
}
