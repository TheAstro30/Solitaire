/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System.Drawing;
using Solitaire.Classes.Theme.ColorTables;

namespace Solitaire.Classes.Theme.Presets
{
    public sealed class DefaultPreset : PresetColorTable
    {
        public DefaultPreset() : base("Default")
        {
            /* Empty by default */
        }

        public override Color ButtonSelectedHighlight => ButtonSelectedGradientMiddle;

        public override Color ButtonSelectedHighlightBorder => ButtonSelectedBorder;

        public override Color ButtonPressedHighlight => ButtonPressedGradientMiddle;

        public override Color ButtonPressedHighlightBorder => ButtonPressedBorder;

        public override Color ButtonCheckedHighlight => ButtonCheckedGradientMiddle;

        public override Color ButtonCheckedHighlightBorder => ButtonSelectedBorder;

        public override Color ButtonPressedBorder => ButtonSelectedBorder;

        public override Color ButtonSelectedBorder => Color.FromArgb(255, 51, 94, 168);

        public override Color ButtonCheckedGradientBegin => Color.FromArgb(255, 226, 229, 238);

        public override Color ButtonCheckedGradientMiddle => Color.FromArgb(255, 226, 229, 238);

        public override Color ButtonCheckedGradientEnd => Color.FromArgb(255, 226, 229, 238);

        public override Color ButtonSelectedGradientBegin => Color.FromArgb(255, 194, 207, 229);

        public override Color ButtonSelectedGradientMiddle => Color.FromArgb(255, 194, 207, 229);

        public override Color ButtonSelectedGradientEnd => Color.FromArgb(255, 194, 207, 229);

        public override Color ButtonPressedGradientBegin => Color.FromArgb(255, 153, 175, 212);

        public override Color ButtonPressedGradientMiddle => Color.FromArgb(255, 153, 175, 212);

        public override Color ButtonPressedGradientEnd => Color.FromArgb(255, 153, 175, 212);

        public override Color CheckBackground => Color.FromArgb(255, 226, 229, 238);

        public override Color CheckSelectedBackground => Color.FromArgb(255, 51, 94, 168);

        public override Color CheckPressedBackground => Color.FromArgb(255, 51, 94, 168);

        public override Color GripDark => Color.FromArgb(255, 189, 188, 191);

        public override Color GripLight => Color.FromArgb(255, 255, 255, 255);

        public override Color ImageMarginGradientBegin => Color.FromArgb(255, 252, 252, 252);

        public override Color ImageMarginGradientMiddle => Color.FromArgb(255, 245, 244, 246);

        public override Color ImageMarginGradientEnd => Color.FromArgb(255, 235, 233, 237);

        public override Color ImageMarginRevealedGradientBegin => Color.FromArgb(255, 247, 246, 248);

        public override Color ImageMarginRevealedGradientMiddle => Color.FromArgb(255, 241, 240, 242);

        public override Color ImageMarginRevealedGradientEnd => Color.FromArgb(255, 228, 226, 230);

        public override Color MenuStripGradientBegin => Color.FromArgb(255, 235, 233, 237);

        public override Color MenuStripGradientEnd => Color.FromArgb(255, 251, 250, 251);

        public override Color MenuItemSelected => Color.FromArgb(255, 194, 207, 229);

        public override Color MenuItemBorder => Color.FromArgb(255, 51, 94, 168);

        public override Color MenuBorder => Color.FromArgb(255, 134, 133, 136);

        public override Color MenuItemSelectedGradientBegin => Color.FromArgb(255, 194, 207, 229);

        public override Color MenuItemSelectedGradientEnd => Color.FromArgb(255, 194, 207, 229);

        public override Color MenuItemPressedGradientBegin => Color.FromArgb(255, 252, 252, 252);

        public override Color MenuItemPressedGradientMiddle => Color.FromArgb(255, 241, 240, 242);

        public override Color MenuItemPressedGradientEnd => Color.FromArgb(255, 245, 244, 246);

        public override Color RaftingContainerGradientBegin => Color.FromArgb(255, 235, 233, 237);

        public override Color RaftingContainerGradientEnd => Color.FromArgb(255, 251, 250, 251);

        public override Color SeparatorDark => Color.FromArgb(255, 193, 193, 196);

        public override Color SeparatorLight => Color.FromArgb(255, 255, 255, 255);

        public override Color StatusStripGradientBegin => Color.FromArgb(255, 235, 233, 237);

        public override Color StatusStripGradientEnd => Color.FromArgb(255, 251, 250, 251);

        public override Color ToolStripBorder => Color.FromArgb(255, 238, 237, 240);

        public override Color ToolStripDropDownBackground => Color.FromArgb(255, 252, 252, 252);

        public override Color ToolStripGradientBegin => Color.FromArgb(255, 252, 252, 252);

        public override Color ToolStripGradientMiddle => Color.FromArgb(255, 245, 244, 246);

        public override Color ToolStripGradientEnd => Color.FromArgb(255, 235, 233, 237);

        public override Color ToolStripContentPanelGradientBegin => Color.FromArgb(255, 235, 233, 237);

        public override Color ToolStripContentPanelGradientEnd => Color.FromArgb(255, 251, 250, 251);

        public override Color ToolStripPanelGradientBegin => Color.FromArgb(255, 235, 233, 237);

        public override Color ToolStripPanelGradientEnd => Color.FromArgb(255, 251, 250, 251);

        public override Color OverflowButtonGradientBegin => Color.FromArgb(255, 242, 242, 242);

        public override Color OverflowButtonGradientMiddle => Color.FromArgb(255, 224, 224, 225);

        public override Color OverflowButtonGradientEnd => Color.FromArgb(255, 167, 166, 170);
    }
}
