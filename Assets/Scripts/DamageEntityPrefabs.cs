using Unity.Entities;
using UnityEngine;

namespace THPS.DamageSystem
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