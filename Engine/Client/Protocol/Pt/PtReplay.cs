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
        public List<PtFrames> Frames;

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

                buffer.WriteCollection(info.Frames, value => PtFrames.Write(value));
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
                info.Frames = buffer.ReadCollection(raw => PtFrames.Read(raw));
                return info;
            }
        }
    }
}
