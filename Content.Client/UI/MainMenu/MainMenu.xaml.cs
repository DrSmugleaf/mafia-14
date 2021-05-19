﻿using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;

namespace Content.Client.UI.MainMenu
{
    [GenerateTypedNameReferences]
    public partial class MainMenu : Control
    {
        public MainMenu()
        {
            RobustXamlLoader.Load(this);
        }

        public Button Connect => ConnectButton;
        public LineEdit Username => UsernameLineEdit;
        public LineEdit Address => AddressLineEdit;
    }
}
