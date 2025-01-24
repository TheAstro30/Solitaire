/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System.Drawing;
using Solitaire.Classes.Theme.ColorTables;

namespace Solitaire.Classes.Theme.Presets
{
    public sealed class OfficeClassicPreset : PresetColorTable
    {
        public OfficeClassicPreset() : base("Office Classic")
        {
            /* Empty by default */
        }

        public override Color ButtonSelectedBorder => Color.FromArgb(255, 10, 36, 106);

        public override Color ButtonCheckedGradientBegin => Color.FromArgb(255, 131, 144, 179);

        public override Color ButtonCheckedGradientMiddle => Color.FromArgb(255, 131, 144, 179);

        public override Color ButtonCheckedGradientEnd => Color.FromArgb(255, 182, 189, 209);

        public override Color ButtonSelectedGradientBegin => Color.FromArgb(255, 182, 189, 210);

        public override Color ButtonSelectedGradientMiddle => Color.FromArgb(255, 182, 189, 210);

        public override Color ButtonSelectedGradientEnd => Color.FromArgb(255, 182, 189, 210);

        public override Color ButtonPressedGradientBegin => Color.FromArgb(255, 133, 146, 181);

        public override Color ButtonPressedGradientMiddle => Color.FromArgb(255, 133, 146, 181);

        public override Color ButtonPressedGradientEnd => Color.FromArgb(255, 133, 146, 181);

        public override Color CheckBackground => Color.FromArgb(255, 210, 214, 236);

        public override Color CheckSelectedBackground => Color.FromArgb(255, 133, 146, 181);

        public override Color CheckPressedBackground => Color.FromArgb(255, 133, 146, 181);

        public override Color GripDark => Color.FromArgb(255, 160, 160, 160);

        public override Color GripLight => Color.FromArgb(255, 255, 255, 255);

        public override Color ImageMarginGradientBegin => Color.FromArgb(255, 245, 244, 242);

        public override Color ImageMarginGradientMiddle => Color.FromArgb(255, 234, 232, 228);

        public override Color ImageMarginGradientEnd => Color.FromArgb(255, 212, 208, 200);

        public override Color ImageMarginRevealedGradientBegin => Color.FromArgb(255, 238, 236, 233);

        public override Color ImageMarginRevealedGradientMiddle => Color.FromArgb(255, 225, 222, 217);

        public override Color ImageMarginRevealedGradientEnd => Color.FromArgb(255, 216, 213, 206);

        public override Color MenuStripGradientBegin => Color.FromArgb(255, 212, 208, 200);

        public override Color MenuStripGradientEnd => Color.FromArgb(255, 246, 245, 244);

        public override Color MenuItemSelected => Color.FromArgb(255, 210, 214, 236);

        public override Color MenuItemBorder => Color.FromArgb(255, 10, 36, 106);

        public override Color MenuBorder => Color.FromArgb(255, 102, 102, 102);

        public override Color MenuItemSelectedGradientBegin => Color.FromArgb(255, 182, 189, 210);

        public override Color MenuItemSelectedGradientEnd => Color.FromArgb(255, 182, 189, 210);

        public override Color MenuItemPressedGradientBegin => Color.FromArgb(255, 245, 244, 242);

        public override Color MenuItemPressedGradientMiddle => Color.FromArgb(255, 225, 222, 217);

        public override Color MenuItemPressedGradientEnd => Color.FromArgb(255, 234, 232, 228);

        public override Color RaftingContainerGradientBegin => Color.FromArgb(255, 212, 208, 200);

        public override Color RaftingContainerGradientEnd => Color.FromArgb(255, 246, 245, 244);

        public override Color SeparatorDark => Color.FromArgb(255, 166, 166, 166);

        public override Color SeparatorLight => Color.FromArgb(255, 255, 255, 255);

        public override Color StatusStripGradientBegin => Color.FromArgb(255, 212, 208, 200);

        public override Color StatusStripGradientEnd => Color.FromArgb(255, 246, 245, 244);

        public override Color ToolStripBorder => Color.FromArgb(255, 219, 216, 209);

        public override Color ToolStripDropDownBackground => Color.FromArgb(255, 249, 248, 247);

        public override Color ToolStripGradientBegin => Color.FromArgb(255, 245, 244, 242);

        public override Color ToolStripGradientMiddle => Color.FromArgb(255, 234, 232, 228);

        public override Color ToolStripGradientEnd => Color.FromArgb(255, 212, 208, 200);

        public override Color ToolStripContentPanelGradientBegin => Color.FromArgb(255, 212, 208, 200);

        public override Color ToolStripContentPanelGradientEnd => Color.FromArgb(255, 246, 245, 244);

        public override Color ToolStripPanelGradientBegin => Color.FromArgb(255, 212, 208, 200);

        public override Color ToolStripPanelGradientEnd => Color.FromArgb(255, 246, 245, 244);

        public override Color OverflowButtonGradientBegin => Color.FromArgb(255, 225, 222, 217);

        public override Color OverflowButtonGradientMiddle => Color.FromArgb(255, 216, 213, 206);

        public override Color OverflowButtonGradientEnd => Color.FromArgb(255, 128, 128, 128);
    }
}