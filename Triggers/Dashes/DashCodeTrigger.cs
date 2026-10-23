using System.Collections.Generic;
using System.Numerics;
using Celeste.Core.Components;
using Celeste.Core.Mathematics.Extrema;
using Celeste.Core.Scheduling;
using Celeste.Core.Reflection.Runtime;
using Celeste.Everest.Modules.Entities.Metadata;
using Celeste.Formats.Map.Entities.Structure;
using Celeste.Game.Execution.Levels;
using Celeste.Simulation;
using Celeste.Simulation.Gameplay.Player.Dash;
using Celeste.Simulation.Gameplay.Triggers.Foundation;

namespace Celeste.Mods.Sardine7.Triggers.Dashes
{
    [CustomEntity("Sardine7/DashCodeTrigger")]
    [RuntimeTypeAlias("Celeste.Mod.Sardine7.Triggers.DashCodeTrigger")]
    public class DashCodeTrigger : SourceTriggerEntityState
    {
        public bool ResetOnLeave;
        public bool DisableOnLeave;
        private readonly string[] code;
        private readonly string flag;
        private readonly bool flagValue;
        private bool enabled;
        private List<string> currentInputs = new();
        private readonly DashListenerState dashListener;

        public DashCodeTrigger(ISourceEngineContext engine, EntityData data, Vector2 offset)
            : base(((ISourceEntitySourceDataEnvironment)engine).EntitySourceDataRegistration, data.Position + offset,
                data.Width, data.Height, ((ISourceExtremaEnvironment)engine).ExtremaArithmetic)
        {
            code = data.Attr("code", "").Split(',');
            flag = data.Attr("flag", "");
            flagValue = data.Bool("flagValue", true);
            ResetOnLeave = data.Bool("resetOnLeave", false);
            DisableOnLeave = data.Bool("disableOnLeave", true);
            Add(dashListener = new DashListenerState());
            dashListener.OnDash = OnDash;
        }

        private void OnDash(Vector2 direction)
        {
            string input = direction.Y < 0f
                ? direction.X < 0f ? "UL" : direction.X > 0f ? "UR" : "U"
                : direction.Y > 0f
                    ? direction.X < 0f ? "DL" : direction.X > 0f ? "DR" : "D"
                    : direction.X < 0f
                        ? "L"
                        : direction.X > 0f
                            ? "R"
                            : "";
            currentInputs.Add(input);
            if (currentInputs.Count > code.Length) currentInputs.RemoveAt(0);
            if (enabled && currentInputs.Count == code.Length)
            {
                bool matches = true;
                for (int i = 0; i < code.Length; i++)
                    if (!currentInputs[i].Equals(code[i]))
                        matches = false;
                if (matches)
                {
                    (Scene as OriginalLevelSceneRoot).Session.SetFlag(flag, flagValue);
                    RemoveSelf();
                }
            }
        }

        public override void OnEnter(RoomPlayerSession player) => enabled = true;

        public override void OnLeave(RoomPlayerSession player)
        {
            if (DisableOnLeave) enabled = false;
            if (ResetOnLeave) currentInputs = new List<string>();
        }
    }
}
