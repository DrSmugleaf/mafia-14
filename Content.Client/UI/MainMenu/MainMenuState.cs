using System;
using System.Text.RegularExpressions;
using Robust.Client;
using Robust.Client.State;
using Robust.Client.UserInterface;
using Robust.Shared;
using Robust.Shared.Configuration;
using Robust.Shared.IoC;
using Robust.Shared.Network;
using Robust.Shared.Utility;
using static Robust.Client.UserInterface.Controls.BaseButton;
using static Robust.Client.UserInterface.Controls.LayoutContainer;
using UsernameHelpers = Robust.Shared.AuthLib.UsernameHelpers;

namespace Content.Client.UI.MainMenu
{
    public class MainMenuState : State
    {
        [Dependency] private readonly IBaseClient _client = default!;
        [Dependency] private readonly IClientNetManager _netManager = default!;
        [Dependency] private readonly IGameController _gameController = default!;
        [Dependency] private readonly IConfigurationManager _cfgManager = default!;
        [Dependency] private readonly IUserInterfaceManager _userInterface = default!;

        // ReSharper disable once InconsistentNaming
        private static readonly Regex IPv6Regex = new(@"\[(.*:.*:.*)](?::(\d+))?");

        private MainMenu _mainMenu = default!;

        public override void Startup()
        {
            _mainMenu = new MainMenu();

            _mainMenu.Username.Editable = !_gameController.LaunchState.FromLauncher;
            _mainMenu.Username.Text = _cfgManager.GetCVar(CVars.PlayerName);

            _mainMenu.Connect.OnPressed += OnConnectPressed;
            _netManager.ConnectFailed += OnConnectFailed;

            SetAnchorAndMarginPreset(_mainMenu, LayoutPreset.Wide);

            _userInterface.StateRoot.AddChild(_mainMenu);
        }

        public override void Shutdown()
        {
            _netManager.ConnectFailed -= OnConnectFailed;

            // ReSharper disable once ConstantConditionalAccessQualifier
            _mainMenu?.Dispose();
        }

        private void OnConnectFailed(object? sender, NetConnectFailArgs e)
        {
            _userInterface.Popup($"Disconnected: {e.Reason}", "Disconnected");
        }

        private void OnConnectPressed(ButtonEventArgs args)
        {
            if (!_gameController.LaunchState.FromLauncher)
            {
                if (!UsernameHelpers.IsNameValid(_mainMenu.Username.Text, out var usernameReason))
                {
                    _userInterface.Popup($"Invalid username:\n{usernameReason.ToText()}", "Invalid Username");
                    return;
                }

                _cfgManager.SetCVar(CVars.PlayerName, _mainMenu.Username.Text);
                _cfgManager.SaveToFile();
            }

            if (!TryParseAddress(_mainMenu.Address.Text, out var ip, out var port, out var connectReason))
            {
                _userInterface.Popup($"Invalid address:\n{connectReason}", "Invalid Address");
            }

            try
            {
                _client.ConnectToServer(ip, port);
            }
            catch (ArgumentException e)
            {
                _userInterface.Popup($"Unable to connect: {e.Message}", "Connection Error");
            }
        }

        private bool TryParseAddress(string address, out string ip, out ushort port, out string reason)
        {
            var match6 = IPv6Regex.Match(address);
            reason = "";

            if (match6 != Match.Empty)
            {
                ip = match6.Groups[1].Value;

                if (!match6.Groups[2].Success)
                {
                    port = _client.DefaultPort;
                }
                else if (!ushort.TryParse(match6.Groups[2].Value, out port))
                {
                    reason = "Not a valid port.";
                    return false;
                }

                return true;
            }

            // See if the IP includes a port.
            var split = address.Split(':');
            ip = address;
            port = _client.DefaultPort;

            if (split.Length > 2)
            {
                reason = "Not a valid Address.";
                return false;
            }

            // IP:port format.
            if (split.Length == 2)
            {
                ip = split[0];
                if (!ushort.TryParse(split[1], out port))
                {
                    reason = "Not a valid port.";
                    return false;
                }
            }

            return true;
        }
    }
}
