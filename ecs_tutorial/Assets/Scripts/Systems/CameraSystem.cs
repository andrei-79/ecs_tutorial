using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
partial class CameraSystem : SystemBase {
    Entity Target;
    Random Random;
    EntityQuery TanksQuery;
    protected override void OnCreate() {

        Random = Random.CreateFromIndex(1234);
        TanksQuery = GetEntityQuery(typeof(Tank));
        RequireForUpdate(TanksQuery);
    }

    protected override void OnUpdate() {
        if (Target == Entity.Null || UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Space)) {
            var tanks = TanksQuery.ToEntityArray(Allocator.Temp);
            Target = tanks[Random.NextInt(0, tanks.Length)];
        }
        var cameraTransform = CameraSingleton.Instance.transform;
        var tankTransform = SystemAPI.GetComponent<LocalToWorld>(Target);
        cameraTransform.position = tankTransform.Position - 10f * tankTransform.Forward + new float3(0, 5, 0);
        cameraTransform.LookAt(tankTransform.Position + new float3(0, 1, 0));
    }
}