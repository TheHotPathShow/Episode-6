using Unity.Entities;
using UnityEngine.InputSystem;

namespace THPS.DamageSystem
{
    // This system would be replaced with multiple systems through your codebase that apply damage, healing, etc.
    public partial struct QueueDamageSystem : ISystem
    {
        private ComponentLookup<DamageProperties> _damagePropertiesLookup;

        public void OnCreate(ref SystemState state)
        {
            _damagePropertiesLookup = SystemAPI.GetComponentLookup<DamageProperties>();
        }

        public void OnUpdate(ref SystemState state)
        {
            if (Keyboard.current[Key.Digit1].wasPressedThisFrame)
            {
                var damageEntity = state.EntityManager.CreateEntity(stackalloc ComponentType[]
                {
                    ComponentType.ReadOnly<DestroyEntityTag>(),
                    ComponentType.ReadOnly<DamageProperties>()
                });
                SystemAPI.SetComponent(damageEntity, new DamageProperties
                {
                    DamageType = DamageType.Physical,
                    DamageTeam = TeamType.Red,
                    HitPoints = 10
                });
            }

            if (Keyboard.current[Key.Digit2].wasPressedThisFrame)
            {
                var damageEntity = state.EntityManager.CreateEntity(stackalloc ComponentType[]
                {
                    ComponentType.ReadOnly<DestroyEntityTag>(),
                    ComponentType.ReadOnly<DamageProperties>()
                });
                SystemAPI.SetComponent(damageEntity, new DamageProperties
                {
                    DamageType = DamageType.Physical,
                    DamageTeam = TeamType.Red,
                    HitPoints = 10
                });
                
                var damageEntity2 = state.EntityManager.CreateEntity(stackalloc ComponentType[]
                {
                    ComponentType.ReadOnly<DestroyEntityTag>(),
                    ComponentType.ReadOnly<DamageProperties>()
                });
                SystemAPI.SetComponent(damageEntity2, new DamageProperties
                {
                    DamageType = DamageType.Physical,
                    DamageTeam = TeamType.Red,
                    HitPoints = 10
                });
            }
            
            if (Keyboard.current[Key.Digit3].wasPressedThisFrame)
            {
                var damageEntity = state.EntityManager.CreateEntity(stackalloc ComponentType[]
                {
                    ComponentType.ReadOnly<DestroyEntityTag>(),
                    ComponentType.ReadOnly<DamageProperties>()
                });
                SystemAPI.SetComponent(damageEntity, new DamageProperties
                {
                    DamageType = DamageType.Magic,
                    DamageTeam = TeamType.Red,
                    HitPoints = 10
                });
            }
            
            if (Keyboard.current[Key.R].wasPressedThisFrame)
            {
                var healingEntityRed = state.EntityManager.CreateEntity(stackalloc ComponentType[]
                {
                    ComponentType.ReadOnly<DestroyEntityTag>(),
                    ComponentType.ReadOnly<DamageProperties>()
                });
                SystemAPI.SetComponent(healingEntityRed, new DamageProperties
                {
                    DamageType = DamageType.Healing,
                    DamageTeam = TeamType.Red,
                    HitPoints = 100
                });
                
                var healingEntityBlue = state.EntityManager.CreateEntity(stackalloc ComponentType[]
                {
                    ComponentType.ReadOnly<DestroyEntityTag>(),
                    ComponentType.ReadOnly<DamageProperties>()
                });
                SystemAPI.SetComponent(healingEntityBlue, new DamageProperties
                {
                    DamageType = DamageType.Healing,
                    DamageTeam = TeamType.Blue,
                    HitPoints = 100
                });
                
                var healingEntityGreen = state.EntityManager.CreateEntity(stackalloc ComponentType[]
                {
                    ComponentType.ReadOnly<DestroyEntityTag>(),
                    ComponentType.ReadOnly<DamageProperties>()
                });
                SystemAPI.SetComponent(healingEntityGreen, new DamageProperties
                {
                    DamageType = DamageType.Healing,
                    DamageTeam = TeamType.Green,
                    HitPoints = 100
                });
            }
            
            _damagePropertiesLookup.Update(ref state);

            foreach (var curDamageProperties in SystemAPI.Query<DamageProperties>())
            {
                foreach (var damageBuffer in SystemAPI.Query<DynamicBuffer<DamageBufferElement>>())
                {
                    damageBuffer.Add(new DamageBufferElement
                    {
                        DamageType = curDamageProperties.DamageType,
                        DamageTeam = curDamageProperties.DamageTeam,
                        HitPoints = curDamageProperties.HitPoints
                    });
                }
            }
        }
    }
}