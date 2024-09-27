# RoutingTVH
see in project https://github.com/TVHoan/DotnetWithTVH
how to using route
---- Nuget----
https://www.nuget.org/packages/RoutingTVH/1.0.0

Simple Routing With TVH

Easy Routing to Controller

----Program.cs---

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouterService();

var app = builder.Build();

app.UseRouter();

app.Run();

----InController---

impl Class ControllerBasic

---View-----

Define Folder Views and you can add file html to this folder
