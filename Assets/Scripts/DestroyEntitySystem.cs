using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace THPS.CombatSystem
{
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    public partial struct DestroyEntitySystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            var spawnEntityOnDestroyLookup = SystemAPI.GetComponentLookup<SpawnEntityOnDestroy>();
            var transformLookup = SystemAPI.GetComponentLookup<LocalTransform>();
            
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

                if (SystemAPI.HasComponent<GameOverOnDestroy>(entity))
                {
                    var gameOverEntity = ecb.CreateEntity();
                    ecb.AddComponent<GameOverTag>(gameOverEntity);
                }

                if (spawnEntityOnDestroyLookup.TryGetComponent(entity, out var spawnEntityOnDestroy))
                {
                    var spawnedEntity = ecb.Instantiate(spawnEntityOnDestroy.Value);
                    if (transformLookup.TryGetComponent(entity, out var transform))
                    {
                        ecb.SetComponent(spawnedEntity, transform);
                    }
                }

                if (SystemAPI.ManagedAPI.HasComponent<SpawnGameObjectOnDestroy>(entity))
                {
                    var spawnObjectPrefab = SystemAPI.ManagedAPI.GetComponent<SpawnGameObjectOnDestroy>(entity).Value;
                    var spawnPosition = Vector3.zero;
                    var spawnRotation = Quaternion.identity;
                    if (transformLookup.TryGetComponent(entity, out var transform))
                    {
                        spawnPosition = transform.Position;
                        spawnRotation = transform.Rotation;
                    }

                    Object.Instantiate(spawnObjectPrefab, spawnPosition, spawnRotation);
                }
            }
            ecb.Playback(state.EntityManager);
        }
    }
}