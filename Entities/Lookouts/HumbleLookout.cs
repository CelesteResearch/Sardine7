using System.Numerics;
using Celeste.Core.Components.Animation;
using Celeste.Core.Reflection.Runtime;
using Celeste.Core.Scheduling;
using Celeste.Everest.Modules.Entities.Metadata;
using Celeste.Everest.Gameplay.Lookouts;
using Celeste.Everest.Tracking.Attributes;
using Celeste.Game.Entities.Lookouts;
using Celeste.Formats.Map.Entities.Structure;
using Celeste.Graphics.Animation;
using Celeste.Mods.Sardine7.Content.Sprites;

namespace Celeste.Mods.Sardine7.Entities.Lookouts
{
    [CustomEntity("Sardine7/HumbleLookout")]
    [RuntimeTypeAlias("Celeste.Mod.Sardine7.Entities.HumbleLookout")]
    [EverestTrackedAs(typeof(OriginalLookoutState))]
    public class HumbleLookout : EverestLookoutState
    {
        private readonly ISourceEngineContext engine;
        private readonly bool attractive;
        private readonly bool talkative;

        public HumbleLookout(ISourceEngineContext engine, EntityData data, Vector2 offset)
            : base(data, offset, (IOriginalLookoutProcess)engine)
        {
            this.engine = engine;
            attractive = data.Bool("attractive", false);
            talkative = data.Bool("talkative", false);
        }

        public override void Added(object scene)
        {
            base.Added(scene);
            if (!attractive)
            {
                var sprite = ((ILookoutInteractionState)this).Sprite;
                sprite = SardineSpriteResources.For(engine).SpriteBank.CreateOn(sprite, "humble_lookout");
                var lightTween = Get<SourceTweenState>();
                sprite.OnFrameChange = animation =>
                {
                    switch (animation)
                    {
                        case "idle":
                        case "badeline_idle":
                        case "nobackpack_idle":
                            if (sprite.CurrentAnimationFrame == sprite.CurrentAnimationTotalFrames - 1)
                                lightTween.Start();
                            break;
                    }
                };
            }

            if (!talkative) talk.UI = new UnsociableTalkComponentUi(talk);
        }
    }
}
