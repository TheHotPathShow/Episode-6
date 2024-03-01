using Unity.Entities;

namespace THPS.CombatSystem
{
    [UpdateInGroup(typeof(CapabilitySystemGroup), OrderLast = true)]
    public partial struct HandleBufferSystem : ISystem
    {
        private EntityQuery _hitBufferQuery;

        public void OnCreate(ref SystemState state)
        {
            _hitBufferQuery = state.GetEntityQuery(ComponentType.ReadWrite<HitBufferElement>());
        }

        public void OnUpdate(ref SystemState state)
        {
            var triggerEntities = _hitBufferQuery.ToEntityArray(state.WorldUpdateAllocator);
            var hitBufferLookup = SystemAPI.GetBufferLookup<HitBufferElement>();

            foreach (var triggerEntity in triggerEntities)
            {
                var hitBuffer = hitBufferLookup[triggerEntity];
                for (var i = 0; i < hitBuffer.Length; i++)
                {
                    var hit = hitBuffer[i];
                    hit.IsHandled = true;
                    hitBuffer[i] = hit;
                }
            }
        }
    }
}