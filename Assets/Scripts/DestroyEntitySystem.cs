﻿using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace THPS.DamageSystem
{
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    public partial struct DestroyEntitySystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var (_, entity) in SystemAPI.Query<DestroyEntityTag>().WithEntityAccess())
            {
                ecb.DestroyEntity(entity);

                if (state.EntityManager.HasBuffer<Child>(entity))
                {
                    var children = SystemAPI.GetBuffer<Child>(entity);
                    foreach (var child in children)
                    {
                        ecb.AddComponent<DestroyEntityTag>(child.Value);
                    }
                }
            }
            ecb.Playback(state.EntityManager);
        }
    }
}