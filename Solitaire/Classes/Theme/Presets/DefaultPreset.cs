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

        public override Color ButtonSelectedHighlight
        {
            get { return ButtonSelectedGradientMiddle; }
        }

        public override Color ButtonSelectedHighlightBorder
        {
            get { return ButtonSelectedBorder; }
        }

        public override Color ButtonPressedHighlight
        {
            get { return ButtonPressedGradientMiddle; }
        }

        public override Color ButtonPressedHighlightBorder
        {
            get { return ButtonPressedBorder; }
        }

        public override Color ButtonCheckedHighlight
        {
            get { return ButtonCheckedGradientMiddle; }
        }

        public override Color ButtonCheckedHighlightBorder
        {
            get { return ButtonSelectedBorder; }
        }

        public override Color ButtonPressedBorder
        {
            get { return ButtonSelectedBorder; }
        }

        public override Color ButtonSelectedBorder
        {
            get { return Color.FromArgb(255, 51, 94, 168); }
        }

        public override Color ButtonCheckedGradientBegin
        {
            get { return Color.FromArgb(255, 226, 229, 238); }
        }

        public override Color ButtonCheckedGradientMiddle
        {
            get { return Color.FromArgb(255, 226, 229, 238); }
        }

        public override Color ButtonCheckedGradientEnd
        {
            get { return Color.FromArgb(255, 226, 229, 238); }
        }

        public override Color ButtonSelectedGradientBegin
        {
            get { return Color.FromArgb(255, 194, 207, 229); }
        }

        public override Color ButtonSelectedGradientMiddle
        {
            get { return Color.FromArgb(255, 194, 207, 229); }
        }

        public override Color ButtonSelectedGradientEnd
        {
            get { return Color.FromArgb(255, 194, 207, 229); }
        }

        public override Color ButtonPressedGradientBegin
        {
            get { return Color.FromArgb(255, 153, 175, 212); }
        }

        public override Color ButtonPressedGradientMiddle
        {
            get { return Color.FromArgb(255, 153, 175, 212); }
        }

        public override Color ButtonPressedGradientEnd
        {
            get { return Color.FromArgb(255, 153, 175, 212); }
        }

        public override Color CheckBackground
        {
            get { return Color.FromArgb(255, 226, 229, 238); }
        }

        public override Color CheckSelectedBackground
        {
            get { return Color.FromArgb(255, 51, 94, 168); }
        }

        public override Color CheckPressedBackground
        {
            get { return Color.FromArgb(255, 51, 94, 168); }
        }

        public override Color GripDark
        {
            get { return Color.FromArgb(255, 189, 188, 191); }
        }

        public override Color GripLight
        {
            get { return Color.FromArgb(255, 255, 255, 255); }
        }

        public override Color ImageMarginGradientBegin
        {
            get { return Color.FromArgb(255, 252, 252, 252); }
        }

        public override Color ImageMarginGradientMiddle
        {
            get { return Color.FromArgb(255, 245, 244, 246); }
        }

        public override Color ImageMarginGradientEnd
        {
            get { return Color.FromArgb(255, 235, 233, 237); }
        }

        public override Color ImageMarginRevealedGradientBegin
        {
            get { return Color.FromArgb(255, 247, 246, 248); }
        }

        public override Color ImageMarginRevealedGradientMiddle
        {
            get { return Color.FromArgb(255, 241, 240, 242); }
        }

        public override Color ImageMarginRevealedGradientEnd
        {
            get { return Color.FromArgb(255, 228, 226, 230); }
        }

        public override Color MenuStripGradientBegin
        {
            get { return Color.FromArgb(255, 235, 233, 237); }
        }

        public override Color MenuStripGradientEnd
        {
            get { return Color.FromArgb(255, 251, 250, 251); }
        }

        public override Color MenuItemSelected
        {
            get { return Color.FromArgb(255, 194, 207, 229); }
        }

        public override Color MenuItemBorder
        {
            get { return Color.FromArgb(255, 51, 94, 168); }
        }

        public override Color MenuBorder
        {
            get { return Color.FromArgb(255, 134, 133, 136); }
        }

        public override Color MenuItemSelectedGradientBegin
        {
            get { return Color.FromArgb(255, 194, 207, 229); }
        }

        public override Color MenuItemSelectedGradientEnd
        {
            get { return Color.FromArgb(255, 194, 207, 229); }
        }

        public override Color MenuItemPressedGradientBegin
        {
            get { return Color.FromArgb(255, 252, 252, 252); }
        }

        public override Color MenuItemPressedGradientMiddle
        {
            get { return Color.FromArgb(255, 241, 240, 242); }
        }

        public override Color MenuItemPressedGradientEnd
        {
            get { return Color.FromArgb(255, 245, 244, 246); }
        }

        public override Color RaftingContainerGradientBegin
        {
            get { return Color.FromArgb(255, 235, 233, 237); }
        }

        public override Color RaftingContainerGradientEnd
        {
            get { return Color.FromArgb(255, 251, 250, 251); }
        }

        public override Color SeparatorDark
        {
            get { return Color.FromArgb(255, 193, 193, 196); }
        }

        public override Color SeparatorLight
        {
            get { return Color.FromArgb(255, 255, 255, 255); }
        }

        public override Color StatusStripGradientBegin
        {
            get { return Color.FromArgb(255, 235, 233, 237); }
        }

        public override Color StatusStripGradientEnd
        {
            get { return Color.FromArgb(255, 251, 250, 251); }
        }

        public override Color ToolStripBorder
        {
            get { return Color.FromArgb(255, 238, 237, 240); }
        }

        public override Color ToolStripDropDownBackground
        {
            get { return Color.FromArgb(255, 252, 252, 252); }
        }

        public override Color ToolStripGradientBegin
        {
            get { return Color.FromArgb(255, 252, 252, 252); }
        }

        public override Color ToolStripGradientMiddle
        {
            get { return Color.FromArgb(255, 245, 244, 246); }
        }

        public override Color ToolStripGradientEnd
        {
            get { return Color.FromArgb(255, 235, 233, 237); }
        }

        public override Color ToolStripContentPanelGradientBegin
        {
            get { return Color.FromArgb(255, 235, 233, 237); }
        }

        public override Color ToolStripContentPanelGradientEnd
        {
            get { return Color.FromArgb(255, 251, 250, 251); }
        }

        public override Color ToolStripPanelGradientBegin
        {
            get { return Color.FromArgb(255, 235, 233, 237); }
        }

        public override Color ToolStripPanelGradientEnd
        {
            get { return Color.FromArgb(255, 251, 250, 251); }
        }

        public override Color OverflowButtonGradientBegin
        {
            get { return Color.FromArgb(255, 242, 242, 242); }
        }

        public override Color OverflowButtonGradientMiddle
        {
            get { return Color.FromArgb(255, 224, 224, 225); }
        }

        public override Color OverflowButtonGradientEnd
        {
            get { return Color.FromArgb(255, 167, 166, 170); }
        }
    }
}
