using Engine.Client.Lockstep;
using Engine.Client.Lockstep.Behaviours;
using Engine.Client.Modules;
using Engine.Client.Protocol.Pt;
using Engine.Common;
using Engine.Common.Event;
using Engine.Common.Lockstep;
using Engine.Common.Misc;
using Engine.Common.Protocol.Pt;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Sample : MonoBehaviour
{
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

    protected Context MainContext;

    public Transform GameContainer;
    public uint MapId;

    protected virtual void Awake()
    {

    }
    protected virtual void Start()
    {
        EventDispatcher<SimulationEventId, int>.AddListener(SimulationEventId.TheLastFrameHasBeenPlayed, OnLastFrameHasBeenPlayed);
    }

    protected virtual void OnLastFrameHasBeenPlayed(int frameIndex)
    {
        Debug.LogWarning("THE LAST FRAME HAS BEEN PLAYED @"+frameIndex);
    }

    [TerminalCommand("setuid", "setuid(id)")]
    public void SetUserId(string uid)
    {
        MainContext.SetMeta(ContextMetaId.USER_ID, uid);
    }

    [TerminalCommand("connect-ip-port-key", "connect(ip,port,key)")]
    public void ConnectIP_Port_Key(string ip, int port, string key)
    {
        MainContext.Client.Connect(ip, port, key);
    }

    [TerminalCommand("connect", "connect default")]
    public void Connect()
    {
        ConnectIP_Port_Key("127.0.0.1", 9030, "gate-room");
    }

    [TerminalCommand("create", "create")]
    public void CreateRoom()
    {
        MainContext.GetModule<GateServiceModule>().RequestCreateRoom(MapId);
    }

    [TerminalCommand("join", "join(roomId)")]
    public void JoinRoom(uint roomId)
    {
        MainContext.GetModule<GateServiceModule>().RequestJoinRoom(roomId);
    }

    [TerminalCommand("leave", "leave_room()")]
    public void LeaveRoom()
    {
        MainContext.GetModule<GateServiceModule>().RequestLeaveRoom();
    }

    [TerminalCommand("roomlist", "fetch all rooms")]
    public void RoomList()
    {
        MainContext.GetModule<GateServiceModule>().RequestRoomList();
    }
    [TerminalCommand("updateteam", "update-team(roomId,userId,teamId)")]
    public void UpdateTeam(uint roomId, string userId, byte teamId)
    {
        MainContext.GetModule<GateServiceModule>().RequestUpdatePlayerTeam(roomId, userId, teamId);
    }
    [TerminalCommand("updatemap", "update-map(roomId,mapId,maxPlayerCount)")]
    public void UpdateMap(uint roomId, uint mapId, byte maxPlayerCount)
    {
        MainContext.GetModule<GateServiceModule>().RequestUpdateMap(roomId, mapId, maxPlayerCount);
    }

    [TerminalCommand("saverep","saverep(repname)")]
    public void SaveReplay(string name)
    {
        string path = Path.Combine(Application.persistentDataPath, name + ".rep");
        PtReplay rep = new PtReplay();
        rep.MapId = MapId;
        rep.Version = "0.0.1";
        rep.InitEntities = MainContext.GetSimulationController().GetSimulation<DefaultSimulation>().GetEntityWorld().GetEntityInitializer().InitEntities;
        rep.Frames = MainContext.GetSimulationController().GetSimulation<DefaultSimulation>().GetBehaviour<LogicFrameBehaviour>().GetFrames();
        File.WriteAllBytes(path,SevenZip.Helper.CompressBytesAsync(PtReplay.Write(rep)).Result);
    }
    [TerminalCommand("drawmap", "save map asset")]
    public virtual void DrawMap()
    {
        var mapDirectoryPath = Path.Combine(Application.persistentDataPath, "map");
        if (!Directory.Exists(mapDirectoryPath))
        {
            Directory.CreateDirectory(mapDirectoryPath);
        }
    }
    [TerminalCommand("playrep", "playrep(name)")]
    public virtual void PlayReplay(string name)
    {

    }
    [TerminalCommand("stop","stop game or replay")]
    public void Stop()
    {
        SimulationController simulationController = MainContext.GetSimulationController();
        if (simulationController.State == SimulationController.RunState.Running)
        {
            simulationController.Stop();
            Task task = Task.Run(() =>
            {
                while (simulationController.State != SimulationController.RunState.Stopped)
                    Thread.Yield();
            });
            task.Wait();
            simulationController.DisposeSimulation();
            //
            foreach (Transform child in GameContainer.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    protected GUIContent content = new GUIContent();
    protected bool toggleValue;
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(Screen.width-120-24,24, 120, 24));
        toggleValue = GUILayout.Toggle(toggleValue, "Enable Gui Info?");
        GUILayout.EndArea(); 
        if (toggleValue)
        {
            if (MainContext != null)
            {
                content.text = MainContext.ToString();
                GUI.skin.label.fontSize = 16;
                Vector2 size = GUI.skin.label.CalcSize(content);
                float xPosition = Screen.width - size.x - 10;
                float yPosition = 10;
                GUI.Label(new Rect(xPosition, yPosition, size.x, size.y), content);
            }
        }
    }


    [TerminalCommand("launch", "launch game")]
    public void Launch()
    {
        MainContext.GetModule<GateServiceModule>().RequestLaunchGame();
    }

    protected virtual void Update()
    {
        Handler.Update();
    }
}
