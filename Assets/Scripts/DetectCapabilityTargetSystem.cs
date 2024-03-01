using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

namespace THPS.CombatSystem
{
    [UpdateInGroup(typeof(PhysicsSystemGroup))]
    [UpdateAfter(typeof(PhysicsSimulationGroup))]
    public partial struct DetectCapabilityTargetSystem : ISystem
    {
        private CollisionFilter _detectionFilter;
        
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PhysicsWorldSingleton>();
            _detectionFilter = new CollisionFilter
            {
                BelongsTo = 1 << 1, // Raycast
                CollidesWith = 1 << 2 // Character
            };
        }

        public void OnUpdate(ref SystemState state)
        {
            var collisionWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;
            var hits = new NativeList<DistanceHit>(state.WorldUpdateAllocator);
            var teamLookup = SystemAPI.GetComponentLookup<EntityTeam>(true);
            
            foreach (var (target, transform, entity) in SystemAPI.Query<RefRW<MoveToTarget>, LocalTransform>().WithEntityAccess())
            {
                hits.Clear();
                if (collisionWorld.OverlapSphere(transform.Position, target.ValueRO.DetectionRadius, ref hits,
                        _detectionFilter))
                {
                    var closestDistance = float.MaxValue;
                    var closestEntity = Entity.Null;
                    
                    foreach (var hit in hits)
                    {
                        if(!teamLookup.TryGetComponent(hit.Entity, out var hitTeam)) continue;
                        if (teamLookup[entity].Value == hitTeam.Value) continue;
                        if (hit.Distance < closestDistance)
                        {
                            closestDistance = hit.Distance;
                            closestEntity = hit.Entity;
                        }
                    }

                    target.ValueRW.Target = closestEntity;
                }
            }
        }
    }
}