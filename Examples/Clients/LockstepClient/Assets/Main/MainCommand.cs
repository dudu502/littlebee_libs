using Engine.Client.Modules;
using Engine.Client.Network;
using Engine.Common;
using Engine.Common.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityLogger : Engine.Common.Log.ILogger
{
    private string _tag;
    private readonly string _TimeFormatPatten = "yyyy/MM/dd HH:mm:ss.fff";

    public bool IsDebugEnabled { get; set; } = true;

    public UnityLogger(string tag)
    {
        _tag = tag;
    }
    public void Error(string message)
    {
        if (IsDebugEnabled)
            Debug.LogError($"[{DateTime.Now.ToString(_TimeFormatPatten)}]E[{_tag}]\t{message}");
    }

    public void Info(string message)
    {
        if (IsDebugEnabled)
            Debug.Log($"[{DateTime.Now.ToString(_TimeFormatPatten)}]I[{_tag}]\t{message}");
    }

    public void Warn(string message)
    {
        if (IsDebugEnabled)
            Debug.LogWarning($"[{DateTime.Now.ToString(_TimeFormatPatten)}]W[{_tag}]\t{message}");
    }
}
public class MainCommand : MonoBehaviour
{
    public Context MainContext;
    private void Awake()
    {
        MainContext = new Context(Context.CLIENT, new LiteNetworkClient(), new UnityLogger("Unity"));
        MainContext.SetModule(new GateServiceModule())
                    .SetModule(new RoomServiceModule());

    }
    [TerminalCommand("setuid","setuid(id)")]
    public void SetUserId(string uid)
    {
        MainContext.SetMeta(ContextMetaId.UserId, uid);
    }

    [TerminalCommand("connect", "connect(ip,port,key)")]
    public void Connect(string ip,int port,string key)
    {
        Debug.LogWarning($"Connect {ip} {port} {key}");
        MainContext.Client.Connect(ip,port,key);
    }

    [TerminalCommand("connect-default","connect default")]
    public void ConnectDefault()
    {
        Connect("127.0.0.1",9030,"GS");
    }
}
