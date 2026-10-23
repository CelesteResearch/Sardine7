using Celeste.Core.Rendering;
using Celeste.Game.Components.Talking;

namespace Celeste.Mods.Sardine7.Entities.Lookouts
{
    internal class UnsociableTalkComponentUi : OriginalTalkUiEntity
    {
        public UnsociableTalkComponentUi(OriginalTalkComponent handler) : base(handler)
        {
        }

        // The published original method is non-virtual and hides Render; do not activate it as an override.
        public new void Render(ISourceRenderContext context)
        {
        }
    }
}
