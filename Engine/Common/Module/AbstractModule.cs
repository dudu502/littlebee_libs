using Engine.Common.Event;
using Engine.Common.Misc;

namespace Engine.Common.Module
{
    public abstract class AbstractModule
    {
        public AbstractModule() 
        {
            
        }

        public virtual void Dispose()
        {

        }

        protected void DispatchHandlerEvent<T,P>(T type,P paramObject)
        {
            Handler.Run(item => EventDispatcher<T, P>.DispatchEvent(type, paramObject),null);
        }
    }
}
