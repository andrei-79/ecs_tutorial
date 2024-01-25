using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Collections;
[BurstCompile]
partial struct TankSpawningSystem : ISystem {
    EntityQuery m_BaseColorQuery;
    [BurstCompile]
    public void OnCreate(ref SystemState state) {
        state.RequireForUpdate<Config>();
        m_BaseColorQuery = state.GetEntityQuery(ComponentType.ReadOnly<URPMaterialPropertyBaseColor>());
    }
    [BurstCompile]
    public void OnDestroy(ref SystemState state) {
    }
    [BurstCompile]

    public void OnUpdate(ref SystemState state) {
        var config = SystemAPI.GetSingleton<Config>();
        var random = Unity.Mathematics.Random.CreateFromIndex(1234);
        var hue = random.NextFloat();
        URPMaterialPropertyBaseColor RandomColor() {
            hue = (hue + 0.618034005f) % 1f;
            var color = Color.HSVToRGB(hue, 1f, 1f);
            return new URPMaterialPropertyBaseColor {
                Value = (Vector4)color
            };
        }
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        var vehicles = CollectionHelper.CreateNativeArray<Entity>(config.TankCount, Allocator.Temp);
        ecb.Instantiate(config.TankPrefab, vehicles);
        var queryMask = m_BaseColorQuery.GetEntityQueryMask();
        foreach (var vehicle in vehicles) {
            ecb.SetComponentForLinkedEntityGroup(vehicle, queryMask, RandomColor());
        }
        state.Enabled = false;
    }
}