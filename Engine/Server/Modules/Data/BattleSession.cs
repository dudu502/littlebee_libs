using Engine.Common.Protocol.Pt;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Engine.Server.Modules.Data
{
    public class BattleSession
    {
        public readonly PtFramesList KeyFrameList = new PtFramesList();
        public readonly ConcurrentQueue<PtFrames> QueueKeyFrameCollection = new ConcurrentQueue<PtFrames>();
        public readonly Dictionary<string, UserStateObject> Users = new Dictionary<string, UserStateObject>();
        
        public BattleSession()
        {
            KeyFrameList.SetElements(new List<PtFrames>());
        }

        public List<byte[]> GetUserEntityRawBytes()
        {
            var result = new List<byte[]>();
            foreach (var user in Users.Values)
            {
                if(user.EntityRawBytes != null)
                    result.Add(user.EntityRawBytes);
            }
            return result;
        }

        public bool HasOnlinePlayer()
        {
            foreach (UserStateObject userObject in Users.Values)
            {
                if (userObject.IsOnline) return true;
            }
            return false;
        }
        public UserStateObject FindUserStateByUserId(string userId)
        {
            foreach (UserStateObject userObject in Users.Values)
            {
                if (userObject.UserId == userId)
                    return userObject;
            }
            return null;
        }
        public UserStateObject FindUserStateByPeerId(int peerId)
        {
            foreach (UserStateObject userObject in Users.Values)
            {
                if (userObject.PeerId == peerId)
                    return userObject;
            }
            return null;
        }
    }
}