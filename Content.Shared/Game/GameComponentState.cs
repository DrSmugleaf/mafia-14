using Robust.Shared.GameObjects;

namespace Content.Shared.Game
{
    public class GameComponentState : ComponentState
    {
        public GameComponentState(GameState state) : base(ContentNetIDs.GAME)
        {
            State = state;
        }

        public GameState State { get; }
    }
}
