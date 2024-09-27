namespace RoutingTVH.Http;

public class JsonData: Result
{
    public object? Data { get; set; }
    public JsonData(object? data,int statusCode = 200) :base(statusCode)
    {
        Data = data;
    }
    public override string ToString()
    {
        
        return System.Text.Json.JsonSerializer.Serialize(Data);
    }
     
}