namespace EPiServer.DeveloperTools.Infrastructure
{
    //public class HttpParamActionAttribute : ActionNameSelectorAttribute
    //{
    //    public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
    //    {
    //        if(actionName.Equals(methodInfo.Name, StringComparison.InvariantCultureIgnoreCase))
    //            return true;

    //        if(!actionName.Equals("Action", StringComparison.InvariantCultureIgnoreCase))
    //            return false;

    //        var request = controllerContext.RequestContext.HttpContext.Request;
    //        return request[methodInfo.Name] != null;
    //    }
    //}
}
