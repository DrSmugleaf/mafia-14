using Robust.Shared.GameObjects;

namespace Content.Shared.Roles
{
    public class RoleComponentState : ComponentState
    {
        public RoleComponentState(string? roleId = null) : base(ContentNetIDs.ROLE)
        {
            RoleId = roleId;
        }

        public string? RoleId { get; }
    }
}
