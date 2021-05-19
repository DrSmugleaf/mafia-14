using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Shared.Roles
{
    [Prototype("role")]
    public class RolePrototype : IPrototype
    {
        [DataField("id", required: true)]
        public string ID { get; } = default!;
    }
}
