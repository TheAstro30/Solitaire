﻿/* Solitaire
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

        public override Color ButtonSelectedBorder
        {
            get
            {
                return Color.FromArgb(255, 10, 36, 106);
            }
        }

        public override Color ButtonCheckedGradientBegin
        {
            get
            {
                return Color.FromArgb(255, 131, 144, 179);
            }
        }

        public override Color ButtonCheckedGradientMiddle
        {
            get
            {
                return Color.FromArgb(255, 131, 144, 179);
            }
        }

        public override Color ButtonCheckedGradientEnd
        {
            get
            {
                return Color.FromArgb(255, 182, 189, 209);
            }
        }

        public override Color ButtonSelectedGradientBegin
        {
            get
            {
                return Color.FromArgb(255, 182, 189, 210);
            }
        }

        public override Color ButtonSelectedGradientMiddle
        {
            get
            {
                return Color.FromArgb(255, 182, 189, 210);
            }
        }

        public override Color ButtonSelectedGradientEnd
        {
            get
            {
                return Color.FromArgb(255, 182, 189, 210);
            }
        }

        public override Color ButtonPressedGradientBegin
        {
            get
            {
                return Color.FromArgb(255, 133, 146, 181);
            }
        }

        public override Color ButtonPressedGradientMiddle
        {
            get
            {
                return Color.FromArgb(255, 133, 146, 181);
            }
        }

        public override Color ButtonPressedGradientEnd
        {
            get
            {
                return Color.FromArgb(255, 133, 146, 181);
            }
        }

        public override Color CheckBackground
        {
            get
            {
                return Color.FromArgb(255, 210, 214, 236);
            }
        }

        public override Color CheckSelectedBackground
        {
            get
            {
                return Color.FromArgb(255, 133, 146, 181);
            }
        }

        public override Color CheckPressedBackground
        {
            get
            {
                return Color.FromArgb(255, 133, 146, 181);
            }
        }

        public override Color GripDark
        {
            get
            {
                return Color.FromArgb(255, 160, 160, 160);
            }
        }

        public override Color GripLight
        {
            get
            {
                return Color.FromArgb(255, 255, 255, 255);
            }
        }

        public override Color ImageMarginGradientBegin
        {
            get
            {
                return Color.FromArgb(255, 245, 244, 242);
            }
        }

        public override Color ImageMarginGradientMiddle
        {
            get
            {
                return Color.FromArgb(255, 234, 232, 228);
            }
        }

        public override Color ImageMarginGradientEnd
        {
            get
            {
                return Color.FromArgb(255, 212, 208, 200);
            }
        }

        public override Color ImageMarginRevealedGradientBegin
        {
            get
            {
                return Color.FromArgb(255, 238, 236, 233);
            }
        }

        public override Color ImageMarginRevealedGradientMiddle
        {
            get
            {
                return Color.FromArgb(255, 225, 222, 217);
            }
        }

        public override Color ImageMarginRevealedGradientEnd
        {
            get
            {
                return Color.FromArgb(255, 216, 213, 206);
            }
        }

        public override Color MenuStripGradientBegin
        {
            get
            {
                return Color.FromArgb(255, 212, 208, 200);
            }
        }

        public override Color MenuStripGradientEnd
        {
            get
            {
                return Color.FromArgb(255, 246, 245, 244);
            }
        }

        public override Color MenuItemSelected
        {
            get
            {
                return Color.FromArgb(255, 210, 214, 236);
            }
        }

        public override Color MenuItemBorder
        {
            get
            {
                return Color.FromArgb(255, 10, 36, 106);
            }
        }

        public override Color MenuBorder
        {
            get
            {
                return Color.FromArgb(255, 102, 102, 102);
            }
        }

        public override Color MenuItemSelectedGradientBegin
        {
            get
            {
                return Color.FromArgb(255, 182, 189, 210);
            }
        }

        public override Color MenuItemSelectedGradientEnd
        {
            get
            {
                return Color.FromArgb(255, 182, 189, 210);
            }
        }

        public override Color MenuItemPressedGradientBegin
        {
            get
            {
                return Color.FromArgb(255, 245, 244, 242);
            }
        }

        public override Color MenuItemPressedGradientMiddle
        {
            get
            {
                return Color.FromArgb(255, 225, 222, 217);
            }
        }

        public override Color MenuItemPressedGradientEnd
        {
            get
            {
                return Color.FromArgb(255, 234, 232, 228);
            }
        }

        public override Color RaftingContainerGradientBegin
        {
            get
            {
                return Color.FromArgb(255, 212, 208, 200);
            }
        }

        public override Color RaftingContainerGradientEnd
        {
            get
            {
                return Color.FromArgb(255, 246, 245, 244);
            }
        }

        public override Color SeparatorDark
        {
            get
            {
                return Color.FromArgb(255, 166, 166, 166);
            }
        }

        public override Color SeparatorLight
        {
            get
            {
                return Color.FromArgb(255, 255, 255, 255);
            }
        }

        public override Color StatusStripGradientBegin
        {
            get
            {
                return Color.FromArgb(255, 212, 208, 200);
            }
        }

        public override Color StatusStripGradientEnd
        {
            get
            {
                return Color.FromArgb(255, 246, 245, 244);
            }
        }

        public override Color ToolStripBorder
        {
            get
            {
                return Color.FromArgb(255, 219, 216, 209);
            }
        }

        public override Color ToolStripDropDownBackground
        {
            get
            {
                return Color.FromArgb(255, 249, 248, 247);
            }
        }

        public override Color ToolStripGradientBegin
        {
            get
            {
                return Color.FromArgb(255, 245, 244, 242);
            }
        }

        public override Color ToolStripGradientMiddle
        {
            get
            {
                return Color.FromArgb(255, 234, 232, 228);
            }
        }

        public override Color ToolStripGradientEnd
        {
            get
            {
                return Color.FromArgb(255, 212, 208, 200);
            }
        }

        public override Color ToolStripContentPanelGradientBegin
        {
            get
            {
                return Color.FromArgb(255, 212, 208, 200);
            }
        }

        public override Color ToolStripContentPanelGradientEnd
        {
            get
            {
                return Color.FromArgb(255, 246, 245, 244);
            }
        }

        public override Color ToolStripPanelGradientBegin
        {
            get
            {
                return Color.FromArgb(255, 212, 208, 200);
            }
        }

        public override Color ToolStripPanelGradientEnd
        {
            get
            {
                return Color.FromArgb(255, 246, 245, 244);
            }
        }

        public override Color OverflowButtonGradientBegin
        {
            get
            {
                return Color.FromArgb(255, 225, 222, 217);
            }
        }

        public override Color OverflowButtonGradientMiddle
        {
            get
            {
                return Color.FromArgb(255, 216, 213, 206);
            }
        }

        public override Color OverflowButtonGradientEnd
        {
            get
            {
                return Color.FromArgb(255, 128, 128, 128);
            }
        }
    }
}