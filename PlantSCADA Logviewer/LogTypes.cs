using System.Collections.Generic;

public enum LogType {
    Sys,
    Trace,
    Debug,
    Reload,
    Params
}

public class LogTypeDictionary
{
    static Dictionary<string, LogType> _logTypeDict = new Dictionary<string, LogType>()
    {
        {"syslog", LogType.sys },
        {"tracelog", LogType.Trace },
        {"debug", LogType.Debug },
        {"reloadlog", LogType.Reload },
        {"params", LogType.Params },

    };



}