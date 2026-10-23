using Celeste.Core.Scheduling;
using Celeste.Everest.Modules.Runtime;
using Celeste.Mods.Sardine7.Content.Sprites;

namespace Celeste.Mods.Sardine7.Modules
{
    public sealed class Sardine7Module(ISourceEngineContext engine) : SourceEverestModule(engine)
    {
        public override void Load()
        {
        }

        public override void LoadContent(bool firstLoad)
            => SardineSpriteResources.For(SourceEngine).LoadContent(firstLoad);

        public override void Unload()
        {
        }
    }
}
