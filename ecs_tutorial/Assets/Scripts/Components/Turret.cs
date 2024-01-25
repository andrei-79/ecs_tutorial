using Unity.Collections;
using Unity.Entities;
public struct Turret : IComponentData
{
    public Entity CannonBallSpawn;
    public Entity CannonBallPrefab;
}