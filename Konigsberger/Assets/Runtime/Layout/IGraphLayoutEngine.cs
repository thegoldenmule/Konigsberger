public interface IGraphLayoutEngine
{
    void Initialize(Graph graph, GraphContext context);
    void Tick(float dt, GraphContext context);
    void Uninitialize();
}