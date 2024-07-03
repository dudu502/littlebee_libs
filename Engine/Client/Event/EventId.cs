using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Client.Event
{
    public enum MainLoopGateEvent
    {
        RoomListUpdated,
        RoomCreated,
        RoomUpdate,
        RoomJoined,
    }
    public enum MainLoopLoadingEvent
    {
        UpdateLoading,
    }
    public enum LoadingType
    {
        Loading,
        CreateRoomServiceComplete,
        SynchronizingKeyFrames,
        ConnectingToRoomServer,
        Initializing,
        LoadComplete,
    }
}
