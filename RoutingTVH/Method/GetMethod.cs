namespace RoutingTVH.Method;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class Get:Attribute
{    
    public string Method { get
        { return "GET"; }
    }
    public Get()
    {
    }
}