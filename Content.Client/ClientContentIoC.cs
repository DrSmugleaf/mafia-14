using Content.Client.UI;
using Content.Client.UI.StyleSheets;
using Robust.Shared.IoC;

namespace Content.Client
{
    internal static class ClientContentIoC
    {
        public static void Register()
        {
            IoCManager.Register<IUIManager, UiManager>();
            IoCManager.Register<IStyleSheetManager, StyleSheetManager>();
        }
    }
}
