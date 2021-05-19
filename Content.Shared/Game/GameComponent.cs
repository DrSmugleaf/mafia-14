using Robust.Shared.GameObjects;

namespace Content.Shared.Game
{
    public class GameComponent : Component
    {
        public override string Name => "MafiaGame";

        public override uint? NetID => ContentNetIDs.GAME;

        private GameState _state;

        public GameState State
        {
            get => _state;
            set
            {
                if (_state == value)
                {
                    return;
                }

                _state = value;
                Dirty();
            }
        }
    }
}
