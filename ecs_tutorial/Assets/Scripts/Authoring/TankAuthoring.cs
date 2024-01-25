using UnityEngine;
using Unity.Entities;

class TankAuthoring : MonoBehaviour {
}

class TankBaker : Baker<TankAuthoring> {
    public override void Bake(TankAuthoring authoring) {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        Debug.Log("bake Tank");
        AddComponent(entity, new Tank {
            TankSize = 1.0f
        });
    }
}