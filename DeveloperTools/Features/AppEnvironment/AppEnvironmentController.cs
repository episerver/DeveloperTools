using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DeveloperTools.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace EPiServer.DeveloperTools.Features.AppEnvironment;

public class AppEnvironmentController : DeveloperToolsController
{
    private readonly IWebHostEnvironment _environment;
    private readonly IConfiguration _configuration;

    public AppEnvironmentController(IWebHostEnvironment environment, IConfiguration configuration)
    {
        _environment = environment;
        _configuration = configuration;
    }

    public ActionResult Index()
    {
        var model = new AppEnvironmentModel
        {
            Assemblies = GetLoadedAssemblies(), Misc = GetMiscValues(), ConnectionString = GetConnectionStrings()
        };

        return View(model);
    }

    private IReadOnlyDictionary<string, string> GetConnectionStrings()
    {
        return _configuration
            .GetSection("ConnectionStrings")
            .GetChildren()
            .ToDictionary(x => x.Key, x => Sanitize(x.Value));
    }

    private string Sanitize(string value)
    {
        try
        {
            var builder = new SqlConnectionStringBuilder(value) { Password = "****" };
            return builder.ToString();
        }
        catch
        {
            return value;
        }
    }

    private IReadOnlyDictionary<string, string> GetMiscValues()
    {
        var result = new Dictionary<string, string>
        {
            { "ApplicationName", _environment.ApplicationName },
            { "WebRootPath", _environment.WebRootPath },
            { "ContentRootPath", _environment.ContentRootPath },
            { "EnvironmentName", _environment.EnvironmentName },
            { "WebRootFileProvider", _environment.WebRootFileProvider.ToString() },
            { "ContentRootFileProvider", _environment.ContentRootFileProvider.ToString() },
        };

        return result;
    }

    private static List<AssemblyInfo> GetLoadedAssemblies()
    {
        var assemblies = new List<AssemblyInfo>();
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            string fileVersion = null;
            var location = assembly.IsDynamic ? "dynamic" :
                string.IsNullOrEmpty(assembly.Location) ? new Uri(assembly.CodeBase).LocalPath : assembly.Location;

            if (!assembly.IsDynamic)
            {
                var fileVersionAttribute =
                    (AssemblyFileVersionAttribute)assembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false)
                        .FirstOrDefault();
                if (fileVersionAttribute != null)
                {
                    fileVersion = fileVersionAttribute.Version;
                }

                if (fileVersion == null)
                {
                    var versionInfo = assembly.IsDynamic ? null : FileVersionInfo.GetVersionInfo(location);
                    if (versionInfo != null)
                    {
                        fileVersion = versionInfo.FileVersion;
                    }
                }
            }

            assemblies.Add(new AssemblyInfo
            {
                Name = assembly.FullName,
                AssemblyVersion = assembly.GetName().Version?.ToString(),
                FileVersion = fileVersion ?? assembly.GetName().Version?.ToString(),
                Location = assembly.IsDynamic ? "dynamic" : location
            });
        }

        return assemblies;
    }
}
