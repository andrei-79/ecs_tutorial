using UnityEngine;
using Unity.Entities;
using Unity.Rendering;

class CannonBallAuthoring : MonoBehaviour {
}

class CannonBallBaker : Baker<CannonBallAuthoring> {
    public override void Bake(CannonBallAuthoring authoring) {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        Debug.Log("bake Turret");
        AddComponent<CannonBall>(entity);
        AddComponent<URPMaterialPropertyBaseColor>(entity);
    }
}