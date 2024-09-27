namespace RoutingTVH.Http;

public class View: Result
{
    public string ViewName { get; set; }
    public View(string view,int statusCode = 200) :base(statusCode)
    {
        ViewName = view;
    }

    public override string ToString()
    {
        string htmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views", ViewName+".html");
        return File.ReadAllText(htmlPath);
    }
}