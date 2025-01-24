/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System.Drawing;
using Solitaire.Classes.Theme.ColorTables;

namespace Solitaire.Classes.Theme.Presets
{
    public class SystemPreset : PresetColorTable
    {
        public SystemPreset() : base("System Colors")
        {
            /* Empty by default */
        }

        public override Color ButtonSelectedBorder => Color.FromName("ButtonShadow");

        public override Color ButtonCheckedGradientBegin => Color.FromName("ButtonFace");

        public override Color ButtonCheckedGradientMiddle => Color.FromName("ButtonFace");

        public override Color ButtonCheckedGradientEnd => Color.FromName("ButtonHighlight");

        public override Color ButtonSelectedGradientBegin => Color.FromName("ButtonHighlight");

        public override Color ButtonSelectedGradientMiddle => Color.FromName("ButtonHighlight");

        public override Color ButtonSelectedGradientEnd => Color.FromName("ButtonFace");

        public override Color ButtonPressedGradientBegin => Color.FromName("ButtonFace");

        public override Color ButtonPressedGradientMiddle => Color.FromName("ButtonFace");

        public override Color ButtonPressedGradientEnd => Color.FromName("ButtonHighlight");

        public override Color CheckBackground => Color.FromName("Menu");

        public override Color CheckSelectedBackground => Color.FromName("Menu");

        public override Color CheckPressedBackground => Color.FromName("MenuHighlight");

        public override Color GripDark => Color.FromName("ControlDark");

        public override Color GripLight => Color.FromName("ControlLight");

        public override Color ImageMarginGradientBegin => Color.FromName("ControlLight");

        public override Color ImageMarginGradientMiddle => Color.FromName("ControlLight");

        public override Color ImageMarginGradientEnd => Color.FromName("ControlLight");

        public override Color ImageMarginRevealedGradientBegin => Color.FromName("ControlLightLight");

        public override Color ImageMarginRevealedGradientMiddle => Color.FromName("ControlLightLight");

        public override Color ImageMarginRevealedGradientEnd => Color.FromName("Menu");

        public override Color MenuStripGradientBegin => Color.FromName("ControlLight");

        public override Color MenuStripGradientEnd => Color.FromName("ControlLight");

        public override Color MenuItemSelected => Color.FromName("MenuHighlight");

        public override Color MenuItemBorder => Color.FromArgb(0, 0, 0, 0);

        public override Color MenuBorder => Color.FromName("ControlDark");

        public override Color MenuItemSelectedGradientBegin => Color.FromName("MenuHighlight");

        public override Color MenuItemSelectedGradientEnd => Color.FromName("MenuHighlight");

        public override Color MenuItemPressedGradientBegin => Color.FromName("Menu");

        public override Color MenuItemPressedGradientMiddle => Color.FromName("Menu");

        public override Color MenuItemPressedGradientEnd => Color.FromName("Menu");

        public override Color RaftingContainerGradientBegin => Color.FromName("Control");

        public override Color RaftingContainerGradientEnd => Color.FromName("Control");

        public override Color SeparatorDark => Color.FromName("ActiveBorder");

        public override Color SeparatorLight => Color.FromName("Control");

        public override Color StatusStripGradientBegin => Color.FromName("ControlLight");

        public override Color StatusStripGradientEnd => Color.FromName("Control");

        public override Color ToolStripBorder => Color.FromName("ControlDark");

        public override Color ToolStripDropDownBackground => Color.FromName("Menu");

        public override Color ToolStripGradientBegin => Color.FromName("MenuBar");

        public override Color ToolStripGradientMiddle => Color.FromName("MenuBar");

        public override Color ToolStripGradientEnd => Color.FromName("MenuBar");

        public override Color ToolStripContentPanelGradientBegin => Color.FromName("AppWorkspace");

        public override Color ToolStripContentPanelGradientEnd => Color.FromName("AppWorkspace");

        public override Color ToolStripPanelGradientBegin => Color.FromName("ControlLight");

        public override Color ToolStripPanelGradientEnd => Color.FromName("Control");

        public override Color OverflowButtonGradientBegin => Color.FromName("ControlLightLight");

        public override Color OverflowButtonGradientMiddle => Color.FromName("ControlLight");

        public override Color OverflowButtonGradientEnd => Color.FromName("Control");
    }
}
