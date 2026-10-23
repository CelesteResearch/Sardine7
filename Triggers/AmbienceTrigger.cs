using System.Numerics;
using Celeste.Core.Components;
using Celeste.Core.Mathematics.Extrema;
using Celeste.Core.Scheduling;
using Celeste.Everest.Modules.Entities.Metadata;
using Celeste.Formats.Map.Entities.Structure;
using Celeste.Game.Audio;
using Celeste.Game.Execution.Levels;
using Celeste.Game.Execution.Processes;
using Celeste.Simulation;
using Celeste.Simulation.Gameplay.Triggers.Foundation;

namespace Celeste.Mods.Sardine7.Triggers
{
    [CustomEntity("Sardine7/AmbienceTrigger")]
    [Celeste.Core.Reflection.Runtime.RuntimeTypeAlias("Celeste.Mod.Sardine7.Triggers.AmbienceTrigger")]
    public sealed class AmbienceTrigger : SourceTriggerEntityState
    {
        private readonly ISourceEngineContext engine;
        private string oldTrack;
        public string Track;
        public bool ResetOnLeave;
        private IOriginalGameplayProcess Process => (IOriginalGameplayProcess)engine;

        public AmbienceTrigger(ISourceEngineContext engine, EntityData data, Vector2 offset)
            : base(((ISourceEntitySourceDataEnvironment)engine).EntitySourceDataRegistration, data.Position + offset,
                data.Width, data.Height, ((ISourceExtremaEnvironment)engine).ExtremaArithmetic)
        {
            this.engine = engine;
            Track = data.Attr("track", "");
            ResetOnLeave = data.Bool("resetOnLeave", false);
        }

        public override void OnEnter(RoomPlayerSession player)
        {
            if (ResetOnLeave)
                oldTrack = Process.AudioSession.GetEventName(Process.AudioSession.CurrentAmbienceEventInstance);
            var session = ((OriginalLevelSceneRoot)Scene).Session;
            session.Audio.Ambience.Event = ((IOriginalSfxEnvironment)engine).Sfx.EventnameByHandle(Track);
            OriginalAudioSession.Apply(session.Audio, Process.AudioSession.ChannelOutput, Process.AudioApplyPolicy);
        }

        public override void OnLeave(RoomPlayerSession player)
        {
            if (!ResetOnLeave) return;
            var session = ((OriginalLevelSceneRoot)Scene).Session;
            session.Audio.Ambience.Event = oldTrack;
            OriginalAudioSession.Apply(session.Audio, Process.AudioSession.ChannelOutput, Process.AudioApplyPolicy);
        }
    }
}
