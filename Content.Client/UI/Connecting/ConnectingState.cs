using Robust.Client;
using Robust.Client.State;
using Robust.Client.UserInterface;
using Robust.Shared.IoC;
using Robust.Shared.Network;
using static Robust.Client.UserInterface.Controls.LayoutContainer;

namespace Content.Client.UI.Connecting
{
    public class ConnectingState : State
    {
        [Dependency] private readonly IGameController _gameController = default!;
        [Dependency] private readonly IUserInterfaceManager _userInterface = default!;
        [Dependency] private readonly IClientNetManager _netManager = default!;
        [Dependency] private readonly IBaseClient _baseClient = default!;

        private ConnectingHud _connectingHud = default!;

        public override void Startup()
        {
            _connectingHud = new ConnectingHud();

            SetAnchorAndMarginPreset(_connectingHud, LayoutPreset.Wide);

            _userInterface.StateRoot.AddChild(_connectingHud);

            _netManager.ClientConnectStateChanged += OnConnectStateChanged;
            _netManager.ConnectFailed += OnConnectFailed;
        }

        public override void Shutdown()
        {
            _netManager.ConnectFailed -= OnConnectFailed;
            _netManager.ClientConnectStateChanged -= OnConnectStateChanged;

            // ReSharper disable once ConstantConditionalAccessQualifier
            _connectingHud?.Dispose();
        }

        private void OnConnectFailed(object? _, NetConnectFailArgs e)
        {
            SetDisconnected();
            _connectingHud.ShowMessage($"Error:\n{e.Reason}");
        }

        private void OnConnectStateChanged(ClientConnectionState state)
        {
            switch (state)
            {
                case ClientConnectionState.NotConnecting:
                    SetDisconnected();
                    _connectingHud.ShowMessage($"Error:\n{_baseClient.LastDisconnectReason ?? "Not connecting."}");
                    break;
                case ClientConnectionState.ResolvingHost:
                    _connectingHud.ShowMessage("Connecting...\nResolving host.");
                    break;
                case ClientConnectionState.EstablishingConnection:
                    _connectingHud.ShowMessage("Connecting...\nEstablishing connection.");
                    break;
                case ClientConnectionState.Handshake:
                    _connectingHud.ShowMessage("Connecting...\nPerforming handshake.");
                    break;
                case ClientConnectionState.Connected:
                    _connectingHud.ShowMessage("Connected to the server.");
                    break;
            }
        }

        private void SetDisconnected()
        {
            _connectingHud.Reconnect.Visible = true;
            _connectingHud.Reconnect.OnPressed += _ =>
            {
                if (_gameController.LaunchState.ConnectEndpoint != null)
                    _baseClient.ConnectToServer(_gameController.LaunchState.ConnectEndpoint);

                _connectingHud.Reconnect.Visible = false;
            };
        }
    }
}
