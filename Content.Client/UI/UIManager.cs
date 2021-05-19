using Content.Client.UI.Connecting;
using Content.Client.UI.Game;
using Content.Client.UI.MainMenu;
using Robust.Client;
using Robust.Client.State;
using Robust.Shared.GameStates;
using Robust.Shared.IoC;

namespace Content.Client.UI
{
    public class UiManager : IUIManager
    {
        [Dependency] private readonly IGameController _gameController = default!;
        [Dependency] private readonly IBaseClient _client = default!;
        [Dependency] private readonly IStateManager _stateManager = default!;

        public void Initialize()
        {
            _client.RunLevelChanged += OnClientRunLevelChanged;
        }

        public void Shutdown()
        {
            _client.RunLevelChanged -= OnClientRunLevelChanged;
        }

        public void OnClientRunLevelChanged(object? sender, RunLevelChangedEventArgs args)
        {
            switch (args.NewLevel)
            {
                case ClientRunLevel.InGame:
                case ClientRunLevel.Connected:
                    _stateManager.RequestStateChange<InGameState>();
                    break;

                case ClientRunLevel.Initialize when args.OldLevel < ClientRunLevel.Connected:
                    _stateManager.RequestStateChange<MainMenuState>();
                    break;

                // When we disconnect from the server.
                case ClientRunLevel.Error:
                case ClientRunLevel.Initialize when args.OldLevel >= ClientRunLevel.Connected:
                    if (_gameController.LaunchState.FromLauncher)
                    {
                        _stateManager.RequestStateChange<ConnectingState>();
                        break;
                    }

                    _stateManager.RequestStateChange<MainMenuState>();
                    break;

                case ClientRunLevel.Connecting:
                    _stateManager.RequestStateChange<ConnectingState>();
                    break;
            }
        }
    }
}
