/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System.Drawing;
using Solitaire.Classes.Theme.ColorTables;

namespace Solitaire.Classes.Theme.Presets
{
    public sealed class Office2003BluePreset : PresetColorTable
    {
        public Office2003BluePreset() : base("Office 2003 Blue")
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

        public override Color ButtonSelectedBorder => Color.FromArgb(255, 0, 0, 128);

        public override Color ButtonCheckedGradientBegin => Color.FromArgb(255, 255, 223, 154);

        public override Color ButtonCheckedGradientMiddle => Color.FromArgb(255, 255, 195, 116);

        public override Color ButtonCheckedGradientEnd => Color.FromArgb(255, 255, 166, 76);

        public override Color ButtonSelectedGradientBegin => Color.FromArgb(255, 255, 255, 222);

        public override Color ButtonSelectedGradientMiddle => Color.FromArgb(255, 255, 225, 172);

        public override Color ButtonSelectedGradientEnd => Color.FromArgb(255, 255, 203, 136);

        public override Color ButtonPressedGradientBegin => Color.FromArgb(255, 254, 128, 62);

        public override Color ButtonPressedGradientMiddle => Color.FromArgb(255, 255, 177, 109);

        public override Color ButtonPressedGradientEnd => Color.FromArgb(255, 255, 223, 154);

        public override Color CheckBackground => Color.FromArgb(255, 255, 192, 111);

        public override Color CheckSelectedBackground => Color.FromArgb(255, 254, 128, 62);

        public override Color CheckPressedBackground => Color.FromArgb(255, 254, 128, 62);

        public override Color GripDark => Color.FromArgb(255, 39, 65, 118);

        public override Color GripLight => Color.FromArgb(255, 255, 255, 255);

        public override Color ImageMarginGradientBegin => Color.FromArgb(255, 227, 239, 255);

        public override Color ImageMarginGradientMiddle => Color.FromArgb(255, 203, 225, 252);

        public override Color ImageMarginGradientEnd => Color.FromArgb(255, 123, 164, 224);

        public override Color ImageMarginRevealedGradientBegin => Color.FromArgb(255, 203, 221, 246);

        public override Color ImageMarginRevealedGradientMiddle => Color.FromArgb(255, 161, 197, 249);

        public override Color ImageMarginRevealedGradientEnd => Color.FromArgb(255, 114, 155, 215);

        public override Color MenuStripGradientBegin => Color.FromArgb(255, 158, 190, 245);

        public override Color MenuStripGradientEnd => Color.FromArgb(255, 196, 218, 250);

        public override Color MenuItemSelected => Color.FromArgb(255, 255, 238, 194);

        public override Color MenuItemBorder => Color.FromArgb(255, 0, 0, 128);

        public override Color MenuBorder => Color.FromArgb(255, 0, 45, 150);

        public override Color MenuItemSelectedGradientBegin => Color.FromArgb(255, 255, 255, 222);

        public override Color MenuItemSelectedGradientEnd => Color.FromArgb(255, 255, 203, 136);

        public override Color MenuItemPressedGradientBegin => Color.FromArgb(255, 227, 239, 255);

        public override Color MenuItemPressedGradientMiddle => Color.FromArgb(255, 161, 197, 249);

        public override Color MenuItemPressedGradientEnd => Color.FromArgb(255, 123, 164, 224);

        public override Color RaftingContainerGradientBegin => Color.FromArgb(255, 158, 190, 245);

        public override Color RaftingContainerGradientEnd => Color.FromArgb(255, 196, 218, 250);

        public override Color SeparatorDark => Color.FromArgb(255, 106, 140, 203);

        public override Color SeparatorLight => Color.FromArgb(255, 241, 249, 255);

        public override Color StatusStripGradientBegin => Color.FromArgb(255, 158, 190, 245);

        public override Color StatusStripGradientEnd => Color.FromArgb(255, 196, 218, 250);

        public override Color ToolStripBorder => Color.FromArgb(255, 59, 97, 156);

        public override Color ToolStripDropDownBackground => Color.FromArgb(255, 246, 246, 246);

        public override Color ToolStripGradientBegin => Color.FromArgb(255, 227, 239, 255);

        public override Color ToolStripGradientMiddle => Color.FromArgb(255, 203, 225, 252);

        public override Color ToolStripGradientEnd => Color.FromArgb(255, 123, 164, 224);

        public override Color ToolStripContentPanelGradientBegin => Color.FromArgb(255, 158, 190, 245);

        public override Color ToolStripContentPanelGradientEnd => Color.FromArgb(255, 196, 218, 250);

        public override Color ToolStripPanelGradientBegin => Color.FromArgb(255, 158, 190, 245);

        public override Color ToolStripPanelGradientEnd => Color.FromArgb(255, 196, 218, 250);

        public override Color OverflowButtonGradientBegin => Color.FromArgb(255, 127, 177, 250);

        public override Color OverflowButtonGradientMiddle => Color.FromArgb(255, 82, 127, 208);

        public override Color OverflowButtonGradientEnd => Color.FromArgb(255, 0, 53, 145);
    }
}
