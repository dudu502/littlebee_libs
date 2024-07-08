using Engine.Common.Protocol.Pt;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Engine.Client.Modules.Data
{
    public class RoomSession
    {
        public string EntityId;
        public string UserId;
        public Dictionary<int, PtFrames> DictKeyFrames;
        public int InitIndex = -1;
        public int WriteKeyFrameIndex = -1;
        public ConcurrentQueue<PtFrame> QueueKeyFrames = new ConcurrentQueue<PtFrame>();
        private PtFrames keyFrameCached = new PtFrames().SetKeyFrames(new List<PtFrame>());

        public void AddCurrentFrameCommand(int curFrameIdx,ushort cmd,string entityId, byte[] paramsContent)
        {
            PtFrame frame = new PtFrame().SetCmd(cmd).SetEntityId(entityId).SetParamContent(paramsContent);
            keyFrameCached.SetFrameIdx(curFrameIdx);
            keyFrameCached.KeyFrames.Add(frame);
        }

        public PtFrames GetKeyFrameCached() { return keyFrameCached; }

        public void ClearKeyFrameCached()
        {
            keyFrameCached.SetFrameIdx(0);
            keyFrameCached.KeyFrames.Clear();
        }
        public void Clear()
        {
            EntityId = string.Empty;
            UserId = string.Empty;
            DictKeyFrames = null;
            InitIndex = -1;
            WriteKeyFrameIndex = -1;
            QueueKeyFrames = new ConcurrentQueue<PtFrame>();
            ClearKeyFrameCached();
        }
    }
}
