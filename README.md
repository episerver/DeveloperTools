Developer Tools project
==============

[Download latest build here.](https://github.com/episerver/DeveloperTools/releases)

Experimental project to build small tools useful for developers. Install as an add-on in EPiServer CMS 7.5 (or later) using the manual upload button under Addons. When installed you must be part of the Administrators group to use the tool, a new menu "Developer" should appear in the top menu. And remember, use at your own risk - this is not a supported product.

Current features:

* View contents of the StructureMap container 
* View Log4net logs (based on in-memory appender)
* View Content Type sync state between Code and DB
* Take memory dumps
* View templates for content
- View and test ASP.NET routes
* View loaded assemblies in AppDomain
* View startup time for initialization modules
* View remote event statistics, provider and servers


TODO:
* Being able to group templates per content type in the "Templates"-tool
* Revert to Default and Content Type Analyzer should be merged in some way, seems weird with 2 tools.
