using System.Collections.Generic;

public enum ComponentType {
    Client,
    IOServer,
    Alarm,
    Trend,
    Report
}

public static class ComponentDictionary
{
    static Dictionary<string, ComponentType> _componentDict = new Dictionary<string, ComponentType>()
    {
        {"", ComponentType.Client },
        {"ioserver", ComponentType.IOServer },
        {"alarm", ComponentType.Alarm },
        {"trend", ComponentType.Trend },
        {"report", ComponentType.Report }
    };

    public static ComponentType? GetComponent (string name)
    {
        return _componentDict.ContainsKey(name) ? _componentDict[name] : null;
    }
}