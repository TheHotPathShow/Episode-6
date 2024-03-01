using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace THPS.CombatSystem
{
    public struct HitBufferElement : IBufferElementData
    {
        public bool IsHandled;
        public float3 Position;
        public float3 Normal;
        public Entity HitEntity;
    }

    public struct CapabilityPrefab : IComponentData
    {
        public Entity Value;
    }

    public struct CapabilityMoveSpeed : IComponentData
    {
        public float Value;
    }
    
    public struct GameOverOnDestroy : IComponentData {}

    public class SpawnGameObjectOnDestroy : IComponentData
    {
        public GameObject Value;
    }

    public class SpawnEntityOnDestroy : IComponentData
    {
        public Entity Value;
    }

    public struct DamageOnTrigger : IComponentData
    {
        public int HitPoints;
        public DamageType DamageType;
    }
}