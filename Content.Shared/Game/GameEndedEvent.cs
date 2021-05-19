using Robust.Shared.GameObjects;

namespace Content.Shared.Game
{
    public class GameEndedEvent : EntityEventArgs
    {
        public GameEndedEvent(GameComponent game)
        {
            Game = game;
        }

        public GameComponent Game { get; }
    }
}
