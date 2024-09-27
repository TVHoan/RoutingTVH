namespace RoutingTVH.Method;


[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class Post: Attribute
{
    public string Method { get
        { return "POST"; }
    }

    public Post()
    {
    }
}