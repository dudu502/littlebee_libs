using Engine.Common.Protocol.Pt;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Engine.Client.Modules.Data
{
    public class RoomSession
    {
        /// <summary>
        /// User EntityId
        /// </summary>
        public string EntityId;
        /// <summary>
        /// User Id
        /// </summary>
        public string UserId;
        public int InitIndex = -1;
        public int WriteKeyFrameIndex = -1;
        public ConcurrentQueue<PtFrames> HistoryFramesList;
        public ConcurrentQueue<PtFrames> QueueKeyFrames = new ConcurrentQueue<PtFrames>();
        private PtFrames keyFrameCached = new PtFrames().SetKeyFrames(new List<PtFrame>());

        public void AddCurrentFrameCommand(PtFrame frame)
        {
            keyFrameCached.SetFrameIdx(-1);
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
            InitIndex = -1;
            WriteKeyFrameIndex = -1;
            QueueKeyFrames = new ConcurrentQueue<PtFrames>();
            ClearKeyFrameCached();
        }
    }
}
