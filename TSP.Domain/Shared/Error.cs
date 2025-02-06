namespace TSP.Domain.Shared;

public class Error
{
    public string Message { get; }
    public ErrorCode Code { get; }

    public Error(string message, ErrorCode code = ErrorCode.None)
    {
        Message = message;
        Code = code;
    }

    public static Error None => new(string.Empty, ErrorCode.None);

    public static Error ValueInvalid(string property) =>
        new($"{property} is invalid", ErrorCode.ValueInvalid);
    public static Error ValueInvalid(string property, string value) =>
        new($"{property} is invalid, value: {value}", ErrorCode.ValueInvalid);

    public static Error ValueRequired(string property) =>
        new($"{property} is required", ErrorCode.ValueRequired);

    public static Error ValueAlreadyExist(string property, string value) =>
        new($"'{property}': {value} already exists", ErrorCode.ValueAlreadyExist);

    public static Error InUse(string property, string id, string usedIn) =>
        new($"{property} is already used in one or more '{usedIn}'. Id: {id}", ErrorCode.InUse);
    public static Error InUse(string property, int id, string usedIn) =>
        new($"{property} is already used in one or more '{usedIn}'. Id: {id}", ErrorCode.InUse);
    public static Error InUse(string property, string id, string[] usedIn)
    {
        string usedInText = string.Join(", ", usedIn.Select(u => $"'{u}'"));
        return new($"{property} is already used in one or more {usedInText}. Id: {id}", ErrorCode.InUse);
    }

    public static Error NotFound(string entityName, string id) =>
        new($"{entityName}: record not found. Id: {id}", ErrorCode.NotFound);
    public static Error NotFound(string entityName) =>
        new($"{entityName}: record not found", ErrorCode.NotFound);

    public static Error AccessDenied(string resource) =>
        new($"Access to {resource} is denied", ErrorCode.AccessDenied);

    public static Error InvalidCredentials() =>
        new($"Invalid email or password", ErrorCode.InvalidCredentials);

    public static Error GuidInvalid(Guid guid) =>
        new($"Invalid Id value, Id must be a valid GUID. Id: {guid}", ErrorCode.GuidInvalid);

    public static Error FormatInvalid(string property, string value, string correctFormat) =>
        new($"Invalid format for {property}. Current value: {value} does not match the expected format: {correctFormat}", ErrorCode.FormatInvalid);

    public static Error InternalServerError(string errorText)
    {
        return new Error(errorText, ErrorCode.InternalServer);
    }

    public static Error CustomError(string message)
    {
        return new Error(message, ErrorCode.Custom);
    }
}



public enum ErrorCode
{
    None,
    NotFound,
    ValueInvalid,
    ValueRequired,
    ValueAlreadyExist,
    InUse,
    AccessDenied,
    InvalidCredentials,
    GuidInvalid,
    FormatInvalid,
    InternalServer,
    Custom
}
