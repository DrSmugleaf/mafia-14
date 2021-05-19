using Robust.Shared.GameObjects;

namespace Content.Shared.Game
{
    public partial class GameSystem : EntitySystem
    {
        public void StartGame(GameComponent game)
        {
            game.State = GameState.Start;

            var args = new GameStartedEvent(game);
            EntityManager.EventBus.RaiseLocalEvent(game.Owner.Uid, args);
        }

        public void EndGame(GameComponent game)
        {
            game.State = GameState.End;

            var args = new GameEndedEvent(game);
            EntityManager.EventBus.RaiseLocalEvent(game.Owner.Uid, args);
        }
    }
}
