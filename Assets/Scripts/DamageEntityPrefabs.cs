using Unity.Entities;
using UnityEngine;

namespace THPS.CombatSystem
{
    public struct DamageEntityPrefabs : IComponentData
    {
        public Entity DamageOverTime;
    }

    public class DamageGameObjectPrefabs : IComponentData
    {
        public GameObject HealthBarUIPrefab;
    }
}