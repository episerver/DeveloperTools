namespace EPiServer.DeveloperTools.Features.StartupPerf;

public class TimeMetersModel
{
    public string AssemblyName { get; set; }
    public string TypeName { get; set; }
    public string MethodName { get; set; }
    public long ElapsedMilliseconds { get; set; }
}
