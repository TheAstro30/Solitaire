/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System.Drawing;
using Solitaire.Classes.Theme.ColorTables;

namespace Solitaire.Classes.Theme.Presets
{
    public sealed class DarkPreset : PresetColorTable
    {
        public DarkPreset() : base("Dark Colors")
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

        public override Color ButtonSelectedBorder => Color.FromArgb(255, 98, 98, 98);

        public override Color ButtonCheckedGradientBegin => Color.FromArgb(255, 144, 144, 144);

        public override Color ButtonCheckedGradientMiddle => Color.FromArgb(255, 170, 170, 170);

        public override Color ButtonCheckedGradientEnd => Color.FromArgb(255, 170, 170, 170);

        public override Color ButtonSelectedGradientBegin => Color.FromArgb(255, 170, 170, 170);

        public override Color ButtonSelectedGradientMiddle => Color.FromArgb(255, 170, 170, 170);

        public override Color ButtonSelectedGradientEnd => Color.FromArgb(255, 170, 170, 170);

        public override Color ButtonPressedGradientBegin => Color.FromArgb(255, 170, 170, 170);

        public override Color ButtonPressedGradientMiddle => Color.FromArgb(255, 170, 170, 170);

        public override Color ButtonPressedGradientEnd => Color.FromArgb(255, 170, 170, 170);

        public override Color CheckBackground => Color.FromArgb(255, 173, 173, 173);

        public override Color CheckSelectedBackground => Color.FromArgb(255, 173, 173, 173);

        public override Color CheckPressedBackground => Color.FromArgb(255, 140, 140, 140);

        public override Color GripDark => Color.FromArgb(255, 22, 22, 22);

        public override Color GripLight => Color.FromArgb(255, 83, 83, 83);

        public override Color ImageMarginGradientBegin => Color.FromArgb(255, 85, 85, 85);

        public override Color ImageMarginGradientMiddle => Color.FromArgb(255, 68, 68, 68);

        public override Color ImageMarginGradientEnd => Color.FromArgb(255, 68, 68, 68);

        public override Color ImageMarginRevealedGradientBegin => Color.FromArgb(255, 68, 68, 68);

        public override Color ImageMarginRevealedGradientMiddle => Color.FromArgb(255, 68, 68, 68);

        public override Color ImageMarginRevealedGradientEnd => Color.FromArgb(255, 68, 68, 68);

        public override Color MenuStripGradientBegin => Color.FromArgb(255, 138, 138, 138);

        public override Color MenuStripGradientEnd => Color.FromArgb(255, 103, 103, 103);

        public override Color MenuItemSelected => Color.FromArgb(255, 170, 170, 170);

        public override Color MenuItemBorder => Color.FromArgb(255, 170, 170, 170);

        public override Color MenuBorder => Color.FromArgb(255, 22, 22, 22);

        public override Color MenuItemSelectedGradientBegin => Color.FromArgb(255, 170, 170, 170);

        public override Color MenuItemSelectedGradientEnd => Color.FromArgb(255, 170, 170, 170);

        public override Color MenuItemPressedGradientBegin => Color.FromArgb(255, 125, 125, 125);

        public override Color MenuItemPressedGradientMiddle => Color.FromArgb(255, 125, 125, 125);

        public override Color MenuItemPressedGradientEnd => Color.FromArgb(255, 125, 125, 125);

        public override Color RaftingContainerGradientBegin => Color.FromArgb(255, 170, 170, 170);

        public override Color RaftingContainerGradientEnd => Color.FromArgb(255, 170, 170, 170);

        public override Color SeparatorDark => Color.FromArgb(255, 22, 22, 22);

        public override Color SeparatorLight => Color.FromArgb(255, 62, 62, 62);

        public override Color StatusStripGradientBegin => Color.FromArgb(255, 112, 112, 112);

        public override Color StatusStripGradientEnd => Color.FromArgb(255, 97, 97, 97);

        public override Color ToolStripBorder => Color.FromArgb(255, 22, 22, 22);

        public override Color ToolStripDropDownBackground => Color.FromArgb(255, 125, 125, 125);

        public override Color ToolStripGradientBegin => Color.FromName("DimGray");

        public override Color ToolStripGradientMiddle => Color.FromArgb(255, 89, 89, 89);

        public override Color ToolStripGradientEnd => Color.FromArgb(255, 88, 88, 88);

        public override Color ToolStripContentPanelGradientBegin => Color.FromArgb(255, 68, 68, 68);

        public override Color ToolStripContentPanelGradientEnd => Color.FromArgb(255, 68, 68, 68);

        public override Color ToolStripPanelGradientBegin => Color.FromArgb(255, 103, 103, 103);

        public override Color ToolStripPanelGradientEnd => Color.FromArgb(255, 103, 103, 103);

        public override Color OverflowButtonGradientBegin => Color.FromArgb(255, 103, 103, 103);

        public override Color OverflowButtonGradientMiddle => Color.FromArgb(255, 103, 103, 103);

        public override Color OverflowButtonGradientEnd => Color.FromArgb(255, 79, 79, 79);
    }
}