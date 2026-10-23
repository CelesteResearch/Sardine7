using System.Numerics;
using Celeste.Core.Components;
using Celeste.Core.Mathematics.Extrema;
using Celeste.Core.Mathematics.Framework;
using Celeste.Core.Mathematics.LinearAlgebra;
using Celeste.Core.Reflection.Runtime;
using Celeste.Core.Scheduling;
using Celeste.Everest.Modules.Entities.Metadata;
using Celeste.Formats.Map.Entities.Structure;
using Celeste.Game.Entities.Triggers.Camera;
using Celeste.Game.Execution.Levels;
using Celeste.Scene.Tracking;
using Celeste.Simulation;
using Celeste.Simulation.Gameplay.Triggers.Foundation;

namespace Celeste.Mods.Sardine7.Triggers.Camera
{
    [Celeste.Scene.Tracking.Attributes.SceneTracked(false)]
    [RuntimeTypeAlias("Celeste.Mod.Sardine7.Triggers.SmoothieCameraTargetTrigger")]
    [CustomEntity("Sardine7/SmoothieCameraTargetTrigger")]
    public sealed class SmoothieCameraTargetTrigger : SourceTriggerEntityState
    {
        public Vector2 Target;
        public float xLerpStrength;
        public float yLerpStrength;
        public PositionModes PositionMode;
        public bool XOnly;
        public bool YOnly;
        public string DeleteFlag;

        public SmoothieCameraTargetTrigger(ISourceEngineContext engine, EntityData data, Vector2 offset)
            : base(((ISourceEntitySourceDataEnvironment)engine).EntitySourceDataRegistration, data.Position + offset,
                data.Width, data.Height, ((ISourceExtremaEnvironment)engine).ExtremaArithmetic)
        {
            Target = SourceVector2Math.Subtract(SourceVector2Math.Add(data.Nodes[0], offset),
                SourceVector2Math.Multiply(new Vector2(320f, 180f), .5f));
            xLerpStrength = data.Float("xLerpStrength", 0f);
            yLerpStrength = data.Float("yLerpStrength", 0f);
            PositionMode = data.Enum("positionMode", PositionModes.NoEffect);
            XOnly = data.Bool("xOnly", false);
            YOnly = data.Bool("yOnly", false);
            DeleteFlag = data.Attr("deleteFlag", "");
        }

        public override void OnStay(RoomPlayerSession player)
        {
            if (string.IsNullOrEmpty(DeleteFlag) || !((OriginalLevelSceneRoot)Scene).Session.GetFlag(DeleteFlag))
            {
                player.CameraAnchor = Target;
                player.CameraAnchorLerp = SourceVector2Math.Add(
                    SourceVector2Math.Multiply(Vector2.UnitX,
                        SourceFrameworkMath.Clamp(xLerpStrength * GetPositionLerp(player, PositionMode), 0f, 1f)),
                    SourceVector2Math.Multiply(Vector2.UnitY,
                        SourceFrameworkMath.Clamp(yLerpStrength * GetPositionLerp(player, PositionMode), 0f, 1f)));
                player.CameraAnchorIgnoreX = YOnly;
                player.CameraAnchorIgnoreY = XOnly;
            }
        }

        public override void OnLeave(RoomPlayerSession player)
        {
            base.OnLeave(player);
            bool inside = false;
            foreach (var item in ((ISourceScene)Scene).SourceScene.Entities.GetTrackedEntities(
                         "Celeste.CameraTargetTrigger"))
            {
                if (((OriginalCameraTargetTriggerState)item).PlayerIsInside)
                {
                    inside = true;
                    break;
                }
            }

            if (!inside)
            {
                foreach (var item in ((ISourceScene)Scene).SourceScene.Entities.GetTrackedEntities(
                             "Celeste.CameraAdvanceTargetTrigger"))
                {
                    if (((OriginalCameraAdvanceTargetTriggerState)item).PlayerIsInside)
                    {
                        inside = true;
                        break;
                    }
                }
            }

            if (!inside) player.CameraAnchorLerp = Vector2.Zero;
        }
    }
}
