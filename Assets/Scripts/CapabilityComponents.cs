using Unity.Entities;
using Unity.Mathematics;

namespace THPS.CombatSystem
{
    public struct HitBufferElement : IBufferElementData
    {
        public bool IsHandled;
        public float3 Position;
        public float3 Normal;
        public Entity HitEntity;
    }
}