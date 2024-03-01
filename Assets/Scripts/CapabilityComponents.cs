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

    public struct GamePlayingTag : IComponentData {};
    public struct GameOverTag : IComponentData {};
    public struct GameOverOnDestroy : IComponentData {}

    public class SpawnGameObjectOnDestroy : IComponentData
    {
        public GameObject Value;
    }

    public class SpawnGameObjectOnHit : IComponentData
    {
        public GameObject Value;
    }

    public struct SpawnEntityOnDestroy : IComponentData
    {
        public Entity Value;
    }

    public struct DamageOnTrigger : IComponentData
    {
        public int HitPoints;
        public DamageType DamageType;
    }

    public struct SpawnCapabilityTimer : IComponentData
    {
        public float Timer;
        public float Interval;
    }

    public struct DestroyOnTimer : IComponentData
    {
        public float Value;
    }

    public struct DestroyAfterHits : IComponentData
    {
        public int Value;
    }

    public struct MoveToTarget : IComponentData
    {
        public Entity Target;
        public float MoveSpeed;
        public float DetectionRadius;
    }

    public struct CastingEntity : IComponentData
    {
        public Entity Value;
    }

    public struct HealOnUse : IComponentData
    {
        public int Value;
    }
}