using Engine.Client.Ecsr.Components;
using Engine.Client.Ecsr.Entitas;
using Engine.Common.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Client.Ecsr.Systems
{
    public class AppearanceSystem : IEntitySystem
    {
       
        public EntityWorld World { get; set; }

        public void Execute()
        {
            World.ForEach<Appearance>((id, component) => 
            {
                if(component.Status == Appearance.StatusDefault)
                {
                    component.Status = Appearance.StatusStartLoading;
                    World.GetRenderSpawner().CreateEntityRenderer(new Renders.CreateEntityRendererRequest(id,component.Status,component.Resource));
                }
            });
        }
    }
}
