using System.Collections.Generic;

public enum LogType {
    Sys,
    Trace,
    Debug,
    Reload,
    Params,
    IPC
}

public class LogTypeDictionary
{
    static Dictionary<string, LogType> _logTypeDict = new Dictionary<string, LogType>()
    {
        {"syslog", LogType.Sys },
        {"tracelog", LogType.Trace },
        {"debug", LogType.Debug },
        {"reloadlog", LogType.Reload },
        {"params", LogType.Params },
        {"ipc", LogType.IPC }

    };


    public static LogType? GetLogType(string par)
    {
        return _logTypeDict.ContainsKey(par) ? _logTypeDict[par] : null;
    }
}