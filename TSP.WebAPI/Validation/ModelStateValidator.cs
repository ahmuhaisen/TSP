using Microsoft.AspNetCore.Mvc;
using System.Net;
using TSP.Domain.Shared;

namespace TSP.WebAPI.Validation;

public static class ModelStateValidator
{
    public static IActionResult ValidateModelState(ActionContext context)
    {
        var errors = context.ModelState
              .Where(x => x.Value?.Errors.Count > 0)
              .SelectMany(kvp => kvp.Value!.Errors.Select(error => new Error(
                  $"{kvp.Key}: {error.ErrorMessage}",
                  ErrorCode.ValueInvalid)))
              .ToList();

        var envelope = ResponseEnvelope.Error(errors);
        return new EnvelopeResult(envelope, HttpStatusCode.BadRequest);
    }
}

public class EnvelopeResult : ObjectResult
{
    public EnvelopeResult(object value, HttpStatusCode statusCode)
        : base(value)
    {
        StatusCode = (int)statusCode;
    }
}
