using Unity.Entities;

class ConfigAuthoring : UnityEngine.MonoBehaviour {
    public UnityEngine.GameObject TankPrefab;
    public int TankCount;
    public float SafeZoneRadius;
}
class ConfigBaker : Baker<ConfigAuthoring> {
    public override void Bake(ConfigAuthoring authoring) {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new Config {
            TankPrefab = GetEntity(authoring.TankPrefab, TransformUsageFlags.Dynamic),
            TankCount = authoring.TankCount,
            SafeZoneRadius = authoring.SafeZoneRadius
        });
    }
}