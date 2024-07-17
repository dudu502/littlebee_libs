using System;
namespace Engine.Client.Lockstep.Behaviours.Data
{
    public interface IInputRecord
    {
        Guid EntityId { set; get; }
        int GetRecordType();
        bool IsDirty(IInputRecord record);
        void CopyFrom(IInputRecord record);
    }
}
