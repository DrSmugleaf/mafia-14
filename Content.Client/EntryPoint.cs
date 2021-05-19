using Content.Client.UI;
using Content.Client.UI.StyleSheets;
using Robust.Client.Graphics;
using Robust.Shared.ContentPack;
using Robust.Shared.GameObjects;
using Robust.Shared.Prototypes;
using static Robust.Shared.IoC.IoCManager;

namespace Content.Client
{
    public class EntryPoint : GameClient
    {
        public override void PreInit()
        {
            Resolve<IClyde>().SetWindowTitle("Robust Mafia");
        }

        public override void Init()
        {
            var factory = Resolve<IComponentFactory>();
            var prototypes = Resolve<IPrototypeManager>();

            factory.DoAutoRegistrations();

            foreach (var ignoreName in IgnoredComponents.List)
            {
                factory.RegisterIgnore(ignoreName);
            }

            foreach (var ignoreName in IgnoredPrototypes.List)
            {
                prototypes.RegisterIgnore(ignoreName);
            }

            ClientContentIoC.Register();

            BuildGraph();

            Resolve<IStyleSheetManager>().Initialize();
            Resolve<IUIManager>().Initialize();
        }

        public override void PostInit()
        {
            Resolve<ILightManager>().Enabled = false;
        }
    }
}
