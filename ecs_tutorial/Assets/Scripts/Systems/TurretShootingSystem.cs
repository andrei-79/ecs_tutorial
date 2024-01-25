using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

[BurstCompile]
partial struct TurretShootingSystem : ISystem {
    ComponentLookup<LocalToWorld> m_LocalToWorldTransformsFromEntity;
    [BurstCompile]
    public void OnCreate(ref SystemState state) {
        m_LocalToWorldTransformsFromEntity = state.GetComponentLookup<LocalToWorld>(true);
    }
    [BurstCompile]
    public void OnDestroy(ref SystemState state) {
    }
    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        m_LocalToWorldTransformsFromEntity.Update(ref state);
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        var turretShootJob = new TurretShoot {
            LocalToWorldTransformFromEntity = m_LocalToWorldTransformsFromEntity,
            ECB = ecb
        };
        turretShootJob.Schedule();
    }
}
[WithAll(typeof(Shooting))]
[BurstCompile]
partial struct TurretShoot : IJobEntity {
    [ReadOnly]
    public ComponentLookup<LocalToWorld> LocalToWorldTransformFromEntity;
    public EntityCommandBuffer ECB;
    void Execute(TurretAspect turret) {
        var instance = ECB.Instantiate(turret.CannonBallPrefab);
        var spawnLocalToWorld = LocalToWorldTransformFromEntity[turret.CannonBallSpawn];

        var cannonBallTransform = LocalTransform.FromPosition(spawnLocalToWorld.Position);
        // Debug.Log("spawn ball : " + cannonBallTransform.ToString());
        cannonBallTransform.Scale = LocalToWorldTransformFromEntity[turret.CannonBallPrefab].Value.Scale().x;
        ECB.SetComponent(instance, cannonBallTransform);
        ECB.SetComponent(instance, new CannonBall {
            Speed = spawnLocalToWorld.Value.Forward() * 10f
        });
        ECB.SetComponent(instance, new URPMaterialPropertyBaseColor {
            Value = turret.Color
        });
    }
}