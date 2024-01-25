using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
[BurstCompile]
partial struct TurretRotationSystem : ISystem {
    [BurstCompile]
    public void OnCreate(ref SystemState state) {

    }
    [BurstCompile]
    public void OnDestroy(ref SystemState state) {
    }
    public void OnUpdate(ref SystemState state) {
        var rotation = quaternion.RotateY(SystemAPI.Time.DeltaTime * math.PI);
        // Debug.Log("Rotation System Updating");
        foreach (var transform in SystemAPI.Query<RefRW<LocalTransform>>().WithAll<Turret>()) {
            // Debug.Log("Rotating : " + rotation.ToString());
            transform.ValueRW.Rotation = math.mul(rotation, transform.ValueRW.Rotation);
        }
    }
}