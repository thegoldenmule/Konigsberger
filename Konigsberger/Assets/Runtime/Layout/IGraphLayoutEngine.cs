﻿public interface IGraphLayoutEngine
{
    void Initialize(GraphContext context);
    void Tick(float dt, GraphContext context);
    void Uninitialize();
}