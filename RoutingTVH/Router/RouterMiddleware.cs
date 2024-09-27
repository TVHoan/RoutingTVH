using System.Reflection;
using System.Security.Cryptography.Xml;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using RoutingTVH.As;
using RoutingTVH.Http;

namespace RoutingTVH.Router;

public class RouterMiddleware
{
    private readonly RequestDelegate _next;
    private const string homeLower = "home";
    private const string index= "Index";
    private const string controllerLower = "controller";
    private const string HttpContext = "HttpContext";
    public RouterMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async  Task InvokeAsync(HttpContext context)
    {
         var Refec = context.RequestServices.GetRequiredService<Refec>();
        var controller =  Refec.LoadTypes.Where(type => type.IsClass && !type.IsAbstract)
             .Where(type => type.BaseType == typeof(ControllerBasic));
        string pattern = @"(?:\/(?<parent>[^\/]+)(?:\/(?<child>[^\/]+)\/?)?)?\/?$";
        string patternController = @"^(.*)Controller$";
        string path = context.Request.Path.Value;
        // Áp dụng regex
        var match = Regex.Match(path, pattern);
        string rootPath = match.Groups["parent"].Value;
        if (string.IsNullOrEmpty(rootPath))
        {
            rootPath =homeLower; 
        }
        // Lấy child path từ nhóm 2 (nếu có)
        string childPath = match.Groups["child"].Value;
        if (string.IsNullOrEmpty(childPath))
        {
           childPath =index; 
        }
        var mappingControllerWithPrefix = controller.Where(x => x.Name.ToLower() == rootPath.ToLower()+controllerLower).ToList();
        var mappingController = controller.
            Where(x => x.Name.ToLower() == rootPath.ToLower() ).ToList();
        List<Type> controllerTypes = new List<Type>();
        if (mappingControllerWithPrefix.Count() > 0 || mappingController.Count() > 0)
        {
            controllerTypes = mappingControllerWithPrefix.Count() > 0 ? mappingControllerWithPrefix : mappingController;
            var ClassController = Activator.CreateInstance(controllerTypes[0]);
            PropertyInfo nameProperty = controllerTypes[0].GetProperty(HttpContext);
            nameProperty.SetValue(ClassController, context);
            MethodInfo method = controllerTypes[0].GetMethod(childPath);
            IEnumerable<Attribute> attributes = method?.GetCustomAttributes().Where(x => x.GetType().Name.ToLower() == context.Request.Method.ToLower()) ?? null;
            if (method is null ||  !attributes.Any())
            {
                context.Response.StatusCode = 404;
                 await _next(context);
                 return;

            }
            ParameterInfo[] parameters = method.GetParameters();
            List<object> parameterValues = parameters.Any() ? new List<object>() : null;
            var dictionary = new Dictionary<string, string>();
            if (new string[] {"POST"}.Contains(context.Request.Method) )
            {
                
                using (var reader = new StreamReader(context.Request.Body))
                {
                    var body = await reader.ReadToEndAsync();

                    // Chuyển đổi JSON thành Dictionary
                     dictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(body);

                }

               

            }
            else
            {
                dictionary =  context.Request.Query.ToDictionary(x => x.Key, x => x.Value.ToString());
            }
            foreach (var param in parameters)
            {
                if (dictionary.ContainsKey(param.Name))
                {
                    parameterValues.Add((Convert.ChangeType(dictionary[param.Name], param.ParameterType)));
                }
                else
                {
                    parameterValues.Add(null);
                }
            }
            var data = method.Invoke(ClassController,parameters.Any() ? parameterValues.ToArray() : null );
            var statusCode = data.GetType().GetProperty("StatusCode",typeof(int)).GetValue(data);
            context.Response.StatusCode = (int)statusCode;
            if (method.ReturnType == typeof(JsonData))
            {
                
                await context.Response.WriteAsJsonAsync(data.GetType().GetProperty("Data").GetValue(data, null));
            }
            else {
                await context.Response.WriteAsync(data.ToString());
            }
        }
        else
        {
            await _next(context);
        }
    }


}