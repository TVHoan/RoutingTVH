using System.Reflection;

namespace RoutingTVH.As;

public class Refec
{
    public IEnumerable<Type> LoadTypes;
    public Refec()
    {
        Assembly refec  = Assembly.Load(Assembly.GetExecutingAssembly().GetName().Name);
        LoadTypes= refec.GetTypes();
    }
}