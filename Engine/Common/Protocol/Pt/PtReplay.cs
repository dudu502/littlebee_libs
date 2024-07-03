using Engine.Common.Protocol;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Common.Protocol.Pt
{
    public class PtReplay
    {
        private static MD5 _MD5Inst = MD5.Create();

        public string Version;

        public uint MapId = 0;
        public byte[] MapVerificationCodes;

        public List<uint> InitEntityIds;

        public List<List<PtFrame>> Frames;
        private byte[] FrameIdxInfoVerificationCodes;

        public static byte[] ComputeHash(string value)
        {
            return _MD5Inst.ComputeHash(Encoding.UTF8.GetBytes(value));
        }
        public static byte[] ComputeHash(byte[] value)
        {
            return _MD5Inst.ComputeHash(value);
        }
        public override string ToString()
        {
            string mapHash = string.Join("-", MapVerificationCodes);
            string entityIds = string.Join("-", InitEntityIds);
            string frameHash = string.Join("-", FrameIdxInfoVerificationCodes);
            return $"[Version]:{Version} [MapId]:{MapId} [MapVerificationCodes]:{mapHash} [InitEntityIds]:{entityIds} [FrameIdxInfoVerificationCodes]:{frameHash}";
        }

        public static byte[] Write(PtReplay info)
        {
            using (ByteBuffer buffer = new ByteBuffer())
            {
                buffer.WriteString(info.Version);
                buffer.WriteUInt32(info.MapId);
                buffer.WriteBytes(info.MapVerificationCodes);

                int initEntityIdsCount = info.InitEntityIds.Count;
                buffer.WriteInt32(initEntityIdsCount);
                for (int k = 0; k < initEntityIdsCount; ++k)
                    buffer.WriteUInt32(info.InitEntityIds[k]);

                using (ByteBuffer frameBuffer = new ByteBuffer())
                {
                    int count = info.Frames.Count;
                    frameBuffer.WriteInt32(count);
                    for (int i = 0; i < count; ++i)
                    {
                        List<PtFrame> frames = info.Frames[i];
                        int frameCount = frames.Count;
                        frameBuffer.WriteInt32(frameCount);
                        for (int j = 0; j < frameCount; ++j)
                        {
                            PtFrame frameInfo = frames[j];
                            frameBuffer.WriteBytes(PtFrame.Write(frameInfo));
                        }
                    }
                    byte[] frameRawBuffer = frameBuffer.GetRawBytes();
                    byte[] frameVerificationCodes = _MD5Inst.ComputeHash(frameRawBuffer);

                    buffer.WriteBytes(frameVerificationCodes);
                    buffer.WriteBytes(frameRawBuffer);
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
                info.MapVerificationCodes = buffer.ReadBytes();
                int initEntityIdsCount = buffer.ReadInt32();
                info.InitEntityIds = new List<uint>();
                for (int k = 0; k < initEntityIdsCount; ++k)
                    info.InitEntityIds.Add(buffer.ReadUInt32());

                info.FrameIdxInfoVerificationCodes = buffer.ReadBytes();

                using (ByteBuffer frameBuffer = new ByteBuffer(buffer.ReadBytes()))
                {
                    int count = frameBuffer.ReadInt32();
                    info.Frames = new List<List<PtFrame>>();
                    for (int i = 0; i < count; ++i)
                    {
                        int frameCount = frameBuffer.ReadInt32();
                        List<PtFrame> frames = new List<PtFrame>();
                        info.Frames.Add(frames);
                        for (int j = 0; j < frameCount; ++j)
                        {
                            frames.Add(PtFrame.Read(frameBuffer.ReadBytes()));
                        }
                    }
                }
                return info;
            }
        }
    }
}
