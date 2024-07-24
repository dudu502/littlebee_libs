using Engine.Common.Misc;
using System;

namespace Engine.Client.Ecsr.Renders
{
    public struct CreateEntityRendererRequest
    {
        public Guid EntityId;
        public string ResourcePath;
        public byte Status;
        public CreateEntityRendererRequest(Guid id,byte status,string resource)
        {
            EntityId = id;
            ResourcePath = resource;
            Status = status;
        }

        public override string ToString()
        {
            return $"EntityId:{EntityId} ResourcePath:{ResourcePath} Status:{Status}";
        }
    }

    public class EntityRenderSpawner
    {
        /// <summary>
        /// This method is called in Simulation.Run's Thread.
        /// </summary>
        /// <param name="request"></param>
        public void CreateEntityRenderer(CreateEntityRendererRequest request)
        {
            Handler.Run((obj) =>
            {
                CreateEntityRendererImpl((CreateEntityRendererRequest)obj);
            }, request);
        }
        protected virtual void CreateEntityRendererImpl(CreateEntityRendererRequest request)
        {
            
        }
    }
}
