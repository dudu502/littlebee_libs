using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Client.Lockstep.Behaviours.Data
{
    public enum ButtonFunction
    {
        None,
        Fire,
    }
    public class ButtonInputRecord : IInputRecord
    {
        public static ButtonFunction CurrentFunc = ButtonFunction.None;
        public Guid EntityId { set; get; }
        public ButtonFunction Func { set; get; }
        public void CopyFrom(IInputRecord record)
        {
            ButtonInputRecord result = record as ButtonInputRecord;
            if (result != null)
            {
                EntityId = result.EntityId;
                Func = result.Func;
            }
        }

        public int GetRecordType()
        {
            return 0;
        }

        public bool IsDirty(IInputRecord record)
        {
            ButtonInputRecord result = (ButtonInputRecord)record;
            if (result == null) return false;
            return result.EntityId == EntityId && result.Func != Func;
        }
    }
}
