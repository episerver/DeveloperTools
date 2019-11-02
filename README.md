Developer Tools project
==============
Download latest build on [NuGet](http://nuget.episerver.com/en/OtherPages/Package/?packageId=EPiServer.DeveloperTools) or [under releases](https://github.com/episerver/DeveloperTools/releases)

Experimental project to build small tools useful for developers. Install as an add-on in EPiServer CMS 7.5 (or later) using the manual upload button under Addons. When installed you must be part of the Administrators group to use the tool, a new menu "Developer" should appear in the top menu. And remember, use at your own risk - this is not a supported product.

Current features:

* View contents of the StructureMap container 
* View Log4net logs (based on in-memory appender)
* View Content Type sync state between Code and DB
* Take memory dumps
* View templates for content
* View and test ASP.NET routes
* View loaded assemblies in AppDomain
* View startup time for initialization modules
* View remote event statistics, provider and servers
* View all registered view engines
* View local object cache content (with option to remove some items)

## How Risky it is to install on production?
You can read more in depth analysis of toolset and it's side-effects [here](https://blog.tech-fellow.net/2019/02/14/how-risky-are-episerver-developertools-on-production-environment/).

## Contributing?
Sandbox site credentials: admin/P@ssword1!
