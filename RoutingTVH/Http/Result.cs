namespace RoutingTVH.Http;

public abstract class Result
{
    protected Result(int statusCode)
    {
        StatusCode = statusCode;
    }

    public int StatusCode { get; set; }
}