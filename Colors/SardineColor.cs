// SPDX-License-Identifier: MIT
// Original named/hex color helper by Cruor.

using System;
using System.Reflection;
using Celeste.Core.Display;
using Celeste.Core.Scheduling;
using Celeste.Everest.Diagnostics.Logging;
using Celeste.Graphics.Display.Colors;

namespace Celeste.Mods.Sardine7.Colors
{
    internal static class SardineColor
    {
        public static PackedColor Parse(ISourceEngineContext engine, string color)
        {
            foreach (PropertyInfo property in colorProps)
            {
                if (color.Equals(property.Name, StringComparison.OrdinalIgnoreCase))
                    return (PackedColor)property.GetValue(default(PackedColor));
            }

            try
            {
                return SourceHexColor.HexToColor(color.Replace("#", ""));
            }
            catch
            {
                ((IEverestLoggerProcessEnvironment)engine).Logger.Log("ColorHelper",
                    "Failed to transform color " + color + ", returning Color.White");
            }

            return PackedColor.White;
        }

        private static PropertyInfo[] colorProps = typeof(PackedColor).GetProperties();
    }
}
