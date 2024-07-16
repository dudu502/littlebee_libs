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
    }

    public class EntityRenderSpawner
    {
        public Func<CreateEntityRendererRequest, object> LoadInitialize;
        /// <summary>
        /// This method is called in Simulation.Run's Thread.
        /// </summary>
        /// <param name="request"></param>
        public void CreateEntityRenderer(CreateEntityRendererRequest request)
        {
            if (LoadInitialize != null)
            {
                Handler.Run((obj) => 
                {
                    var result = LoadInitialize((CreateEntityRendererRequest)obj);

                },request);
            }
        }
    }
}
