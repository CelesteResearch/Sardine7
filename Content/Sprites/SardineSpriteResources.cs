using System;
using System.Runtime.CompilerServices;
using Celeste.Core.Scheduling;
using Celeste.Formats.Graphics.Atlas;
using Celeste.Formats.Graphics.Sprites;
using Celeste.Graphics.Content;

namespace Celeste.Mods.Sardine7.Content.Sprites
{
    public sealed class SardineSpriteResources
    {
        private static readonly ConditionalWeakTable<ISourceEngineContext, SardineSpriteResources> Resources = new();
        private readonly ISourceEngineContext engine;
        private SpriteBank spriteBank;

        private SardineSpriteResources(ISourceEngineContext engine) => this.engine = engine;

        public static SardineSpriteResources For(ISourceEngineContext engine)
            => Resources.GetValue(engine, owner => new SardineSpriteResources(owner));

        public SpriteBank SpriteBank =>
            spriteBank ?? throw new InvalidOperationException("Sardine7 content has not been loaded.");

        public void LoadContent(bool firstLoad)
        {
            if (firstLoad)
            {
                var graphics = ((ISourceGameGraphicsEnvironment)engine).GameGraphics;
                var xml = ((ISourceAtlasLoaderEnvironment)engine).LoadContentXml("Graphics/Sardine7/Sprites.xml");
                spriteBank = new SpriteBank(engine, graphics.GameAtlas, xml);
            }
        }
    }
}
