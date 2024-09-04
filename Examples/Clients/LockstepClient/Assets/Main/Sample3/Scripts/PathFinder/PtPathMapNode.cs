using Engine.Common.Protocol;

public class PtPathMapNode : IHeapItem<PtPathMapNode>
{
    public int HeapIndex { set; get; }
    public int X { set; get; }
    public int Y { set; get; }
    public bool Walkable { set; get; }
    public int H { set; get; }
    public int G { set; get; }
    public int F { set; get; }
    public int LastNodeX { set; get; }
    public int LastNodeY { set; get; }

    public static byte[] Write(PtPathMapNode node)
    {
        using(ByteBuffer buffer = new ByteBuffer())
        {
            return buffer.WriteInt32(node.X)
                    .WriteInt32(node.Y)
                    .WriteBool(node.Walkable)
                    .WriteInt32(node.H)
                    .WriteInt32(node.G)
                    .WriteInt32(node.F)
                    .WriteInt32(node.LastNodeX)
                    .WriteInt32(node.LastNodeY)
                    .GetRawBytes();
        }
    }
    public static PtPathMapNode Read(byte[] bytes)
    {
        using(ByteBuffer buffer = new ByteBuffer(bytes))
        {
            PtPathMapNode node = new PtPathMapNode();
            node.X = buffer.ReadInt32();
            node.Y = buffer.ReadInt32();
            node.Walkable = buffer.ReadBool();
            node.H = buffer.ReadInt32();
            node.G = buffer.ReadInt32();
            node.F = buffer.ReadInt32();
            node.LastNodeX = buffer.ReadInt32();
            node.LastNodeY = buffer.ReadInt32();
            return node;
        }
    }
    public int CompareTo(PtPathMapNode other)
    {
        throw new System.NotImplementedException();
    }
}

