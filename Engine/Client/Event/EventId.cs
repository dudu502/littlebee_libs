
namespace Engine.Client.Event
{
    public struct LoadingEventId
    {
        public const byte StartLoading = 0;
        public const byte CreateRoomServiceComplete = 1;
        public const byte SynchronizingKeyFrames = 3;
        public const byte ConnectingToRoomServer = 4;
        public const byte Initializing = 5;
        public const byte LoadComplete = 6;

        public const byte SynchronizingKeyFramesCompleted = 7;
        public const byte BeReadyToEnterScene = 8;
        public byte LoadingType;
        public float Progress;
        public LoadingEventId(byte type,float progress)
        {
            LoadingType = type;
            Progress = progress;
        }
    }
    public enum LoadingType
    {
        Loading,
    }
}
