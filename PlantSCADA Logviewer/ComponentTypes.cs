using System.Collections.Generic;

public enum ComponentType {
    Client,
    IOServer,
    Alarm,
    Trend,
    Report,
    DeploymentServer,
    DeploymentClient,
    Studio
}

public static class ComponentDictionary
{
    static Dictionary<string, ComponentType> _componentDict = new Dictionary<string, ComponentType>()
    {
        {"", ComponentType.Client },
        {"ioserver", ComponentType.IOServer },
        {"alarm", ComponentType.Alarm },
        {"trend", ComponentType.Trend },
        {"report", ComponentType.Report },
        {"se.asb.Deployment.server.windowsservice",ComponentType.DeploymentServer },
        {"se.asb.Deployment.node.windowsservice",ComponentType.DeploymentClient },
        {"idetracelog", ComponentType.Studio }
    };

    public static ComponentType? GetComponent (string name)
    {
        return _componentDict.ContainsKey(name) ? _componentDict[name] : null;
    }
}