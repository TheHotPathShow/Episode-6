using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace THPS.CombatSystem
{
    public struct MaxHitPoints : IComponentData
    {
        public int Value;
    }

    public struct CurrentHitPoints : IComponentData
    {
        public int Value;
    }
    
    public enum DamageType : byte
    {
        None = 0,
        Physical = 1,
        Magic = 2,
        Poison = 3,
        Healing = 4,
    }

    public enum TeamType : byte
    {
        None = 0,
        Red = 1,
        Blue = 2,
        Green = 3,
    }

    public struct EntityTeam : IComponentData
    {
        public TeamType Value;
    }
    
    // Queues damage elements throughout duration of the frame so they can all be applied once at the end of the frame.
    // Avoids the problem where multiple damage elements are applied to a single entity and only one actually applies.
    [InternalBufferCapacity(1)]
    public struct DamageBufferElement : IBufferElementData
    {
        public DamageType DamageType;
        public TeamType DamageTeam;
        public int HitPoints;
    }

    // This component will go on the damaging entity to tell the system what type/amount of damage to queue for an entity.
    public struct DamageProperties : IComponentData
    {
        public DamageType DamageType;
        public TeamType DamageTeam;
        public int HitPoints;
    }

    public struct DamageOverTimeProperties : IComponentData
    {
        public DamageType DamageType;
        public TeamType DamageTeam;
        public int HitPointsPerHit;
        public float HitInterval;
        public float Timer;
        public float NextHitTime;
    }
    
    // Multiplier components allow additional modification of the final damage applied to an entity.
    // i.e. armor could reduce the physical damage taken by 50% or an entity could be affected 2x to magic damage
    public struct PhysicalDamageMultiplier : IComponentData
    {
        public float Value;
    }

    public struct MagicDamageMultiplier : IComponentData
    {
        public float Value;
    }

    public struct PoisonDamageMultiplier : IComponentData
    {
        public float Value;
    }
    
    // Managed Cleanup Component used to reference the world-space Unity UI health bar slider associated with an entity
    public class HealthBarUI : ICleanupComponentData
    {
        public GameObject Value;
    }

    public struct HealthBarOffset : IComponentData
    {
        public float3 Value;
    }

    public struct UpdateHealthBarUI : IComponentData, IEnableableComponent {}
    
    // We can apply a red visual flash to the entity when it gets hit, this timer allows us to configure 
    public struct DamageFlashTimer : IComponentData, IEnableableComponent
    {
        public float FlashTime;
        public float CurrentTime;
    }
    
    [WriteGroup(typeof(CurrentHitPoints))]
    public struct IgnoreDamageMultiplicationTag : IComponentData {}
    
    public struct DestroyEntityTag : IComponentData {}
}