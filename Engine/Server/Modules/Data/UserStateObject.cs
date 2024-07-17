using Engine.Common.Misc;

namespace Engine.Server.Modules.Data
{
    public class UserStateObject
    {
        public UserState StateFlag = UserState.EnteredRoom;
        public int PeerId;
        public string UserId;
        public string UserEntityId;
        public bool IsOnline = true;

        public void Update(int peerId, UserState state)
        {
            PeerId = peerId;
            StateFlag = state;
        }
        public UserStateObject(int peerId, UserState state, string userId, string userEntityId)
        {
            PeerId = peerId;
            StateFlag = state;
            UserId = userId;
            UserEntityId = userEntityId;
        }
    }
}
