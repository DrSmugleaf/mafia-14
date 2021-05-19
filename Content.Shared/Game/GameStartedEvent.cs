using Robust.Shared.GameObjects;

namespace Content.Shared.Game
{
    public class GameStartedEvent : EntityEventArgs
    {
        public GameStartedEvent(GameComponent game)
        {
            Game = game;
        }

        public GameComponent Game { get; }
    }
}
