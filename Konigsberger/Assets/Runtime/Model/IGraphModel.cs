public interface IGraphModel
{
    void Initialize(GraphContext context);
    void Tick(float dt, GraphContext context);
    void Uninitialize();
}