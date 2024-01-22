Developer Tools project
==============

Download latest build on [NuGet](https://nuget.optimizely.com/package/?id=EPiServer.DeveloperTools) or [under releases](https://github.com/episerver/DeveloperTools/releases)

Experimental project to build small tools useful for developers. Install as an add-on in Optimizely CMS 12 (or later).
When installed you must be part of the Administrators group to use the tool, a new menu "Developer" should appear in the top menu.

# DISCLAIMER
Remember, use at your own risk - this is not a supported product!

## Current Features

* View contents of the Dependency Injection container 
* View Content Type sync state between Code and DB
* View templates for content
* View ASP.NET routes
* View loaded assemblies in AppDomain
* View startup time for initialization modules
* View remote event statistics, provider and servers
* View all registered view engines
* View local object cache content (with option to remove items)

## Getting Started

To get started with Optimizely developer tools - all you need to do is to add it to your project and use it :)

```csharp
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddOptimizelyDeveloperTools();
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    ...
    app.UseOptimizelyDeveloperTools();
}
```

## How Risky it is to install on production?
You can read more in depth analysis of toolset and it's side-effects [here](https://tech-fellow.eu/2019/02/14/how-risky-are-episerver-developertools-on-production-environment/).

## Contributing?

### Sandbox Site
Sandbox site credentials: admin/P@ssword1!

### Building
Post build event is copying Razor views from source project (DeveloperTools/) to test sandbox site (tests/DeveloperTools.SandboxSite).
If it fails to execute - please create an GitHub issue.
