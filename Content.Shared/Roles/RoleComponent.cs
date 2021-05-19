using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Players;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Shared.Roles
{
    [RegisterComponent]
    public class RoleComponent : Component
    {
        [Dependency] private readonly IPrototypeManager _prototypeManager = default!;

        public override string Name => "Role";

        public override uint? NetID => ContentNetIDs.ROLE;

        private string? _roleId;

        [DataField("role", required: true)]
        private string? RoleId
        {
            get => _roleId;
            set
            {
                if (_roleId == value)
                {
                    return;
                }

                _roleId = value;
                Dirty();
            }
        }

        public RolePrototype? Role
        {
            get => RoleId == null ? null : _prototypeManager.Index<RolePrototype>(RoleId);
            set => RoleId = value?.ID;
        }

        public override ComponentState GetComponentState(ICommonSession player)
        {
            if (player.AttachedEntity == Owner)
            {
                return new RoleComponentState(RoleId);
            }
            else
            {
                return new RoleComponentState();
            }
        }

        public override void HandleComponentState(ComponentState? curState, ComponentState? nextState)
        {
            if (curState is not RoleComponentState state)
            {
                return;
            }

            _roleId = state.RoleId;
        }
    }
}
