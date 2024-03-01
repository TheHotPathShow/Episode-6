using Unity.Entities;

namespace THPS.CombatSystem
{
    public partial struct GameOverSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.EntityManager.CreateSingleton<GamePlayingTag>();
            state.RequireForUpdate<GamePlayingTag>();
            state.RequireForUpdate<GameOverTag>();
        }

        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;

            var gamePlayingEntity = SystemAPI.GetSingletonEntity<GamePlayingTag>();
            state.EntityManager.DestroyEntity(gamePlayingEntity);

            GameOverUIController.Instance.ShowGameOverUI();
        }
    }
}