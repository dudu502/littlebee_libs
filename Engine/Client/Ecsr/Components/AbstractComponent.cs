﻿namespace Engine.Client.Ecsr.Components
{
    public abstract class AbstractComponent
    {
        public AbstractComponent() { }
        public abstract AbstractComponent Clone();
        public abstract void CopyFrom(AbstractComponent component);
        public abstract byte[] Serialize();
        public abstract AbstractComponent Deserialize(byte[] bytes);
        public virtual void UpdateParams(byte[] content) { }
    }
}
