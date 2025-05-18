using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
public partial class DebugSystem : SystemBase
{
    protected override void OnUpdate()
    {
        EntityCommandBuffer ecb = SystemAPI.GetSingletonRW<EndSimulationEntityCommandBufferSystem.Singleton>().ValueRW.CreateCommandBuffer(World.Unmanaged);
        foreach (var (debug, entity) in SystemAPI.Query<RefRO<DebugComponent>>().WithEntityAccess())
        {
            Debug.Log(debug.ValueRO.Value);
            ecb.RemoveComponent<DebugComponent>(entity);
        }
    }
}
