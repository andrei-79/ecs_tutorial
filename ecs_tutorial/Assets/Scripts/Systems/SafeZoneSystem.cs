using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[WithAll(typeof(Turret))]
[BurstCompile]
partial struct SafeZoneJob : IJobEntity {
    [NativeDisableParallelForRestriction]
    public ComponentLookup<Shooting> TurretActiveFromEntity;
    public float SquaredRadius;
    void Execute(Entity entity, LocalToWorld transform) {
        Debug.LogFormat(@"SafeZoneJob, enabled : {0} , math.lengthsq(transform.Position) : {1}, SquaredRadius : {2}", math.lengthsq(transform.Position) > SquaredRadius ? "true" : "false", math.lengthsq(transform.Position), SquaredRadius);
        TurretActiveFromEntity.SetComponentEnabled(entity, math.lengthsq(transform.Position) > SquaredRadius);
    }
}
[BurstCompile]
partial struct SafeZoneSystem : ISystem {
    ComponentLookup<Shooting> m_TurretActiveFromEntity;
    [BurstCompile]
    public void OnCreate(ref SystemState state) {
        state.RequireForUpdate<Config>();
        m_TurretActiveFromEntity = state.GetComponentLookup<Shooting>();
    }
    [BurstCompile]
    public void OnDestroy(ref SystemState state) {
    }
    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        var radius = SystemAPI.GetSingleton<Config>().SafeZoneRadius;
        const float debugRenderStepInDegrees = 20;
        for (float angle = 0; angle < 360; angle += debugRenderStepInDegrees) {
            var a = float3.zero;
            var b = float3.zero;
            math.sincos(math.radians(angle), out a.x, out a.z);
            math.sincos(math.radians(angle + debugRenderStepInDegrees), out b.x, out b.z);
            Debug.DrawLine(a * radius, b * radius);
        }
        m_TurretActiveFromEntity.Update(ref state);
        var safeZoneJob = new SafeZoneJob {
            TurretActiveFromEntity = m_TurretActiveFromEntity,
            SquaredRadius = radius * radius
        };
        safeZoneJob.ScheduleParallel();
    }
}