using Engine.Common.Protocol;
using Engine.Common.Protocol.Pt;
using System.Collections.Generic;
namespace Engine.Client.Protocol.Pt
{
    public class PtReplay
    {
        public string Version;
        public uint MapId = 0;
        public List<EntityList> InitEntities;
        public List<List<PtFrame>> Frames;

        public static byte[] Write(PtReplay info)
        {
            using (ByteBuffer buffer = new ByteBuffer())
            {
                buffer.WriteString(info.Version);
                buffer.WriteUInt32(info.MapId);

                bool hasInitEntities = info.InitEntities != null;
                buffer.WriteBool(hasInitEntities);
                if (hasInitEntities)                
                    buffer.WriteCollection(info.InitEntities, value => EntityList.Write(value));
   
                int framesCount = info.Frames.Count;
                buffer.WriteInt32(framesCount);
                for (int i = 0; i < framesCount; ++i)
                {
                    List<PtFrame> frames = info.Frames[i];
                    int fcount = frames.Count;
                    buffer.WriteInt32(fcount);
                    for (int j = 0; j < fcount; ++j)
                    {
                        PtFrame frame = frames[j];
                        buffer.WriteBytes(PtFrame.Write(frame));
                    }
                }
                return buffer.GetRawBytes();
            }
        }
        public static PtReplay Read(byte[] bytes)
        {
            using (ByteBuffer buffer = new ByteBuffer(bytes))
            {
                PtReplay info = new PtReplay();
                info.Version = buffer.ReadString();
                info.MapId = buffer.ReadUInt32();
                bool hasInitEntities = buffer.ReadBool();
                if (hasInitEntities)
                    info.InitEntities = buffer.ReadCollection(raw => EntityList.Read(raw));

                info.Frames = new List<List<PtFrame>>();
                int framesCount = buffer.ReadInt32();
                for (int i = 0; i < framesCount; ++i)
                {
                    List<PtFrame> frames = new List<PtFrame>();
                    int fcount = buffer.ReadInt32();
                    for (int j = 0; j < fcount; ++j)
                    {
                        PtFrame frame = PtFrame.Read(buffer.ReadBytes());
                        frames.Add(frame);
                    }
                    info.Frames.Add(frames);
                }
                return info;
            }
        }
    }
}
