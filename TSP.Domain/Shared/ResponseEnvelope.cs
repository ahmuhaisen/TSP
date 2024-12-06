namespace TSP.Domain.Shared;

public class ResponseEnvelope
{
    public object ResponseData { get; }

    public ResponseEnvelope(object responseData) => ResponseData = responseData;

    public static ResponseEnvelope Success(object data) => new ResponseEnvelope(data);

    public static ResponseEnvelope Failure(Error error) => new ResponseEnvelope(new ResponseItem(error));

    public static ResponseEnvelope Error(List<Error> errors) => new ResponseEnvelope(new ResponseItem(errors));
}

public class ResponseItem
{
    public List<Error> Errors { get; set; } = new();

    public ResponseItem(){}

    public ResponseItem(Error error)
    {
        if (error != null)
        {
            AddError(error);
        }
    }

    public ResponseItem(List<Error> errors)
    {
        if (errors != null)
        {
            Errors = errors;
        }
    }

    public void AddError(Error error)
    {
        Errors.Add(error);
    }
}
