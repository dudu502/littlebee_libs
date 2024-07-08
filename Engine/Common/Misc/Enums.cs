namespace Engine.Common.Misc
{
    public enum UserState : byte
    {
        None = 0,
        EnteredRoom = 1,
        BeReadyToEnterScene = 2,

        Re_EnteredRoom = 3,
        Re_BeReadyToEnterScene = 4,
    }
}
