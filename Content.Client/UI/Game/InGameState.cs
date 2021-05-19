using Content.Client.UI.Lobby;
using Content.Shared.Game;
using Robust.Client.Player;
using Robust.Client.State;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.IoC;
using Robust.Shared.Timing;

namespace Content.Client.UI.Game
{
    public class InGameState : State
    {
        [Dependency] private readonly IUserInterfaceManager _userInterface = default!;
        [Dependency] private readonly IPlayerManager _playerManager = default!;

        private GameHud _gameHud = default!;
        private LobbyHud _lobbyHud = default!;

        public override void Startup()
        {
            _gameHud = new GameHud() {Visible = false};
            _lobbyHud = new LobbyHud() {Visible = true};

            LayoutContainer.SetAnchorAndMarginPreset(_gameHud, LayoutContainer.LayoutPreset.Wide);
            LayoutContainer.SetAnchorAndMarginPreset(_lobbyHud, LayoutContainer.LayoutPreset.Wide);

            _userInterface.StateRoot.AddChild(_gameHud);
            _userInterface.StateRoot.AddChild(_lobbyHud);
        }

        public override void Shutdown()
        {
            // ReSharper disable ConstantConditionalAccessQualifier
            _gameHud?.Dispose();
            _lobbyHud?.Dispose();
            // ReSharper restore ConstantConditionalAccessQualifier
        }

        public override void FrameUpdate(FrameEventArgs e)
        {
            base.FrameUpdate(e);

            var playerEntity = _playerManager.LocalPlayer?.ControlledEntity;

            if (playerEntity == null ||
                !playerEntity.Transform.GetMapTransform().Owner.TryGetComponent(out GameComponent? game))
            {
                return;
            }

            _gameHud.Visible = game.State != GameState.Start;
            _lobbyHud.Visible = game.State == GameState.Start;
        }
    }
}
