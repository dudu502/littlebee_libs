namespace Engine.Client.Ecsr.Renders
{
    public struct CreateEntityRendererRequest
    {
        public string EntityId;
        public string ResourcePath;
    }
    public interface IEntityRenderSpawner
    {
        void CreateEntityRenderer(CreateEntityRendererRequest request);
    }
}
