/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System.Collections.Generic;
using Solitaire.Classes.Theme;
using Solitaire.Classes.Theme.ColorTables;
using Solitaire.Classes.Theme.Presets;

namespace Solitaire.Classes.Helpers.Management
{
    public static class ThemeManager
    {
        /* A class that doesn't really do much except keeping the renderer and the preset list in one place */
        public static BaseRenderer Renderer;

        public static List<PresetColorTable> Presets;

        static ThemeManager()
        {
            Renderer = new BaseRenderer {RoundedEdges = true};

            Presets = new List<PresetColorTable>
                          {
                              new DefaultPreset(),
                              new OfficeClassicPreset(),
                              new Office2003BluePreset(),
                              new DarkPreset(),
                              new SystemPreset()
                          };            
        }

        public static void SetTheme(int index)
        {
            Renderer.ColorTable.InitFrom(Presets[index], true);
            Renderer.RefreshToolStrips();
        }        
    }
}
