namespace Engine.Client.Ecsr.Components
{
    public abstract class AbstractComponent
    {
        public abstract AbstractComponent Clone();
        public abstract void CopyFrom(AbstractComponent component);
        public abstract byte[] Serialize();
        public abstract AbstractComponent Deserialize(byte[] bytes);
        public virtual ushort GetCommand() { return 0; }
        public virtual void UpdateParams(byte[] content) { }
    }
}
