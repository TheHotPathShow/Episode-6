using Unity.Entities;
using Unity.Transforms;

namespace THPS.CombatSystem
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(TransformSystemGroup))]
    public partial class CapabilitySystemGroup : ComponentSystemGroup
    {
        
    }
}