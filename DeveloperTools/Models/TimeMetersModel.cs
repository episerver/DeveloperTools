namespace EPiServer.DeveloperTools.Models;

public class TimeMetersModel
{
    public string AssemblyName { get; set; }
    public string TypeName { get; set; }
    public string MethodName { get; set; }
    public long ElapsedMilliseconds { get; set; }
}
