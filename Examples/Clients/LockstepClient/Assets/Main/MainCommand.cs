using Engine.Client.Modules;
using Engine.Client.Network;
using Engine.Common;
using Engine.Common.Log;
using Engine.Common.Misc;
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

    public void Info(object message)
    {
        Info(LitJson.JsonMapper.ToJson(message));
    }

    public void Warn(object message)
    {
        Warn(LitJson.JsonMapper.ToJson(message));
    }

    public void Error(object message)
    {
        Error(LitJson.JsonMapper.ToJson(message));
    }
}
public class MainCommand : MonoBehaviour
{
    public Context MainContext;
    private void Awake()
    {
        Handler.Initialize();
        MainContext = new Context(Context.CLIENT, new LiteNetworkClient(), new UnityLogger("Unity"));
        MainContext.SetMeta(ContextMetaId.StandaloneModePort, "50000");
        MainContext.SetModule(new GateServiceModule())
                    .SetModule(new BattleServiceModule());

    }

    private void Start()
    {
        SetUserId("test1");
        Connect();
    }
    [TerminalCommand("setuid","setuid(id)")]
    public void SetUserId(string uid)
    {
        MainContext.SetMeta(ContextMetaId.UserId, uid);
    }

    [TerminalCommand("connect-ip-port-key", "connect(ip,port,key)")]
    public void ConnectIP_Port_Key(string ip,int port,string key)
    {
        MainContext.Client.Connect(ip,port,key);
    }

    [TerminalCommand("connect","connect default")]
    public void Connect()
    {
        ConnectIP_Port_Key("127.0.0.1",9030,"gate-room");
    }

    [TerminalCommand("create","create_room()")]
    public void CreateRoom()
    {
        MainContext.GetModule<GateServiceModule>().RequestCreateRoom(1);
    }

    [TerminalCommand("join","join(roomId)")]
    public void JoinRoom(uint roomId)
    {
        MainContext.GetModule<GateServiceModule>().RequestJoinRoom(roomId);
    }

    [TerminalCommand("leave", "leave_room()")]
    public void LeaveRoom()
    {
        MainContext.GetModule<GateServiceModule>().RequestLeaveRoom();
    }

    [TerminalCommand("roomlist","fetch all rooms")]
    public void RoomList()
    {
        MainContext.GetModule<GateServiceModule>().RequestRoomList();
    }
    [TerminalCommand("update-team", "update-team(roomId,userId,teamId)")]
    public void UpdateTeam(uint roomId,string userId,byte teamId)
    {
        MainContext.GetModule<GateServiceModule>().RequestUpdatePlayerTeam(roomId,userId,teamId);
    }
    [TerminalCommand("update-map", "update-map(roomId,mapId,maxPlayerCount)")]
    public void UpdateMap(uint roomId, uint mapId, byte maxPlayerCount)
    {
        MainContext.GetModule<GateServiceModule>().RequestUpdateMap(roomId, mapId, maxPlayerCount);
    }

    [TerminalCommand("launch","launch game")]
    public void Launch()
    {
        MainContext.GetModule<GateServiceModule>().RequestLaunchGame();
    }
    private void Update()
    {
        Handler.Update();
    }
}
