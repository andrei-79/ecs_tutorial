using UnityEngine;
using Unity.Entities;

class TurretAuthoring : MonoBehaviour {
    public UnityEngine.GameObject CannonBallPrefab;
    public UnityEngine.Transform CannonBallSpawn;
}

class TurretBaker : Baker<TurretAuthoring> {
    public override void Bake(TurretAuthoring authoring) {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        Debug.Log("bake Turret");
        AddComponent(entity, new Turret {
            CannonBallPrefab = GetEntity(authoring.CannonBallPrefab, TransformUsageFlags.Dynamic),
            CannonBallSpawn = GetEntity(authoring.CannonBallSpawn, TransformUsageFlags.Dynamic),
        });
        AddComponent<Shooting>(entity);
    }
}