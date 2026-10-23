// SPDX-License-Identifier: MIT
// Original light source by WoofWoofDoggo and 0x0ade; Sardine7 changes by fishtank_underflow.

using System.Numerics;
using Celeste.Core.Display;
using Celeste.Core.Reflection.Runtime;
using Celeste.Core.Components;
using Celeste.Core.Scheduling;
using Celeste.Everest.Modules.Entities.Metadata;
using Celeste.Formats.Map.Entities.Structure;
using Celeste.Mods.Sardine7.Colors;
using Celeste.Scene.Entities;
using Celeste.Simulation.Presentation.Lighting;

namespace Celeste.Mods.Sardine7.Entities.Lighting
{
    [CustomEntity("Sardine7/LightSource")]
    [RuntimeTypeAlias("Celeste.Mod.Sardine7.Entities.LightSource")]
    public sealed class LightSource : SceneEntityState
    {
        public LightSource(ISourceEngineContext engine, EntityData data, Vector2 offset)
            : base(((ISourceEntitySourceDataEnvironment)engine).EntitySourceDataRegistration,
                data.Position + offset + new Vector2(4, 4))
        {
            alpha = data.Float("alpha", 1f);
            color = SardineColor.Parse(engine, data.Attr("color", "White"));
            Add(light = new VertexLightState(engine, color, alpha, data.Int("startFade", 24), data.Int("endFade", 48)));
        }

        private VertexLightState light;
        private float alpha;
        private PackedColor color;
    }
}
