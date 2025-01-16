/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Solitaire.Classes.Theme.ColorTables
{
    public enum ColorTableGroup
    {
        ButtonSelectedBorder,
        ButtonCheckedGradientBegin,
        ButtonCheckedGradientMiddle,
        ButtonCheckedGradientEnd,
        ButtonSelectedGradientBegin,
        ButtonSelectedGradientMiddle,
        ButtonSelectedGradientEnd,
        ButtonPressedGradientBegin,
        ButtonPressedGradientMiddle,
        ButtonPressedGradientEnd,
        CheckBackground,
        CheckSelectedBackground,
        CheckPressedBackground,
        GripDark,
        GripLight,
        ImageMarginGradientBegin,
        ImageMarginGradientMiddle,
        ImageMarginGradientEnd,
        ImageMarginRevealedGradientBegin,
        ImageMarginRevealedGradientMiddle,
        ImageMarginRevealedGradientEnd,
        MenuStripGradientBegin,
        MenuStripGradientEnd,
        MenuItemSelected,
        MenuItemBorder,
        MenuBorder,
        MenuItemSelectedGradientBegin,
        MenuItemSelectedGradientEnd,
        MenuItemPressedGradientBegin,
        MenuItemPressedGradientMiddle,
        MenuItemPressedGradientEnd,
        RaftingContainerGradientBegin,
        RaftingContainerGradientEnd,
        SeparatorDark,
        SeparatorLight,
        StatusStripGradientBegin,
        StatusStripGradientEnd,
        ToolStripBorder,
        ToolStripDropDownBackground,
        ToolStripGradientBegin,
        ToolStripGradientMiddle,
        ToolStripGradientEnd,
        ToolStripContentPanelGradientBegin,
        ToolStripContentPanelGradientEnd,
        ToolStripPanelGradientBegin,
        ToolStripPanelGradientEnd,
        OverflowButtonGradientBegin,
        OverflowButtonGradientMiddle,
        OverflowButtonGradientEnd,
    }

    public class CustomizableColorTable : PresetColorTable, ICloneable
    {
        private static readonly ProfessionalColorTable ProfessionalColorTable = new ProfessionalColorTable();

        private Dictionary<ColorTableGroup, Color> _defaultColors;

        private Dictionary<ColorTableGroup, Color> _colors = new Dictionary<ColorTableGroup, Color>();

        public CustomizableColorTable() : base(string.Empty)
        {
            InitFromBase(true);
        }

        public Color ResetColor(ColorTableGroup colorGroup)
        {
            var baseColor = _defaultColors[colorGroup];
            _colors[colorGroup] = baseColor;
            return baseColor;
        }

        public void InitFrom(ProfessionalColorTable colorTable, bool makeColorsDefault)
        {
            // Instead of "colors[ColorTableGroup.ButtonSelectedBorder] = colorTable.ButtonSelectedBorder"...
            // use reflection.
            var colorTableType = colorTable.GetType();
            var colorTableGroupType = typeof(ColorTableGroup);
            foreach (ColorTableGroup colorTableGroup in Enum.GetValues(colorTableGroupType))
            {
                var p = Enum.GetName(colorTableGroupType, colorTableGroup);
                if (p == null)
                {
                    continue;
                }
                var prop = colorTableType.GetProperty(p);
                _colors[colorTableGroup] = (Color)prop.GetValue(colorTable, null);
            }

            if (makeColorsDefault)
            {
                MakeColorsDefault();
            }
        }

        public void InitFromBase(bool makeColorsDefault)
        {
            // Instead of " colors[ColorTableGroup.ButtonSelectedBorder] = base.ButtonSelectedBorder"...
            // use reflection.
            var colorTableType = ProfessionalColorTable.GetType();
            var colorTableGroupType = typeof(ColorTableGroup);
            foreach (ColorTableGroup colorTableGroup in Enum.GetValues(colorTableGroupType))
            {
                var p = Enum.GetName(colorTableGroupType, colorTableGroup);
                if (p == null)
                {
                    continue;
                }
                var prop = colorTableType.GetProperty(p);
                _colors[colorTableGroup] = (Color)prop.GetValue(ProfessionalColorTable, null);
            }

            if (makeColorsDefault)
            {
                MakeColorsDefault();
            }
        }

        public void MakeColorsDefault()
        {
            _defaultColors = new Dictionary<ColorTableGroup, Color>(_colors);
        }

        public Color this[ColorTableGroup colorGroup]
        {
            get { return _colors[colorGroup]; }
            set { _colors[colorGroup] = value; }
        }

        public override Color ButtonSelectedBorder
        {
            get
            {
                return _colors[ColorTableGroup.ButtonSelectedBorder];
            }
        }

        public override Color ButtonCheckedGradientBegin
        {
            get
            {
                return _colors[ColorTableGroup.ButtonCheckedGradientBegin];
            }
        }

        public override Color ButtonCheckedGradientMiddle
        {
            get
            {
                return _colors[ColorTableGroup.ButtonCheckedGradientMiddle];
            }
        }

        public override Color ButtonCheckedGradientEnd
        {
            get
            {
                return _colors[ColorTableGroup.ButtonCheckedGradientEnd];
            }
        }

        public override Color ButtonSelectedGradientBegin
        {
            get
            {
                return _colors[ColorTableGroup.ButtonSelectedGradientBegin];
            }
        }

        public override Color ButtonSelectedGradientMiddle
        {
            get
            {
                return _colors[ColorTableGroup.ButtonSelectedGradientMiddle];
            }
        }

        public override Color ButtonSelectedGradientEnd
        {
            get
            {
                return _colors[ColorTableGroup.ButtonSelectedGradientEnd];
            }
        }

        public override Color ButtonPressedGradientBegin
        {
            get
            {
                return _colors[ColorTableGroup.ButtonPressedGradientBegin];
            }
        }

        public override Color ButtonPressedGradientMiddle
        {
            get
            {
                return _colors[ColorTableGroup.ButtonPressedGradientMiddle];
            }
        }

        public override Color ButtonPressedGradientEnd
        {
            get
            {
                return _colors[ColorTableGroup.ButtonPressedGradientEnd];
            }
        }

        public override Color CheckBackground
        {
            get
            {
                return _colors[ColorTableGroup.CheckBackground];
            }
        }

        public override Color CheckSelectedBackground
        {
            get
            {
                return _colors[ColorTableGroup.CheckSelectedBackground];
            }
        }

        public override Color CheckPressedBackground
        {
            get
            {
                return _colors[ColorTableGroup.CheckPressedBackground];
            }
        }

        public override Color GripDark
        {
            get
            {
                return _colors[ColorTableGroup.GripDark];
            }
        }

        public override Color GripLight
        {
            get
            {
                return _colors[ColorTableGroup.GripLight];
            }
        }

        public override Color ImageMarginGradientBegin
        {
            get
            {
                return _colors[ColorTableGroup.ImageMarginGradientBegin];
            }
        }

        public override Color ImageMarginGradientMiddle
        {
            get
            {
                return _colors[ColorTableGroup.ImageMarginGradientMiddle];
            }
        }

        public override Color ImageMarginGradientEnd
        {
            get
            {
                return _colors[ColorTableGroup.ImageMarginGradientEnd];
            }
        }

        public override Color ImageMarginRevealedGradientBegin
        {
            get
            {
                return _colors[ColorTableGroup.ImageMarginRevealedGradientBegin];
            }
        }

        public override Color ImageMarginRevealedGradientMiddle
        {
            get
            {
                return _colors[ColorTableGroup.ImageMarginRevealedGradientMiddle];
            }
        }

        public override Color ImageMarginRevealedGradientEnd
        {
            get
            {
                return _colors[ColorTableGroup.ImageMarginRevealedGradientEnd];
            }
        }

        public override Color MenuStripGradientBegin
        {
            get
            {
                return _colors[ColorTableGroup.MenuStripGradientBegin];
            }
        }

        public override Color MenuStripGradientEnd
        {
            get
            {
                return _colors[ColorTableGroup.MenuStripGradientEnd];
            }
        }

        public override Color MenuItemSelected
        {
            get
            {
                return _colors[ColorTableGroup.MenuItemSelected];
            }
        }

        public override Color MenuItemBorder
        {
            get
            {
                return _colors[ColorTableGroup.MenuItemBorder];
            }
        }

        public override Color MenuBorder
        {
            get
            {
                return _colors[ColorTableGroup.MenuBorder];
            }
        }

        public override Color MenuItemSelectedGradientBegin
        {
            get
            {
                return _colors[ColorTableGroup.MenuItemSelectedGradientBegin];
            }
        }

        public override Color MenuItemSelectedGradientEnd
        {
            get
            {
                return _colors[ColorTableGroup.MenuItemSelectedGradientEnd];
            }
        }

        public override Color MenuItemPressedGradientBegin
        {
            get
            {
                return _colors[ColorTableGroup.MenuItemPressedGradientBegin];
            }
        }

        public override Color MenuItemPressedGradientMiddle
        {
            get
            {
                return _colors[ColorTableGroup.MenuItemPressedGradientMiddle];
            }
        }

        public override Color MenuItemPressedGradientEnd
        {
            get
            {
                return _colors[ColorTableGroup.MenuItemPressedGradientEnd];
            }
        }

        public override Color RaftingContainerGradientBegin
        {
            get
            {
                return _colors[ColorTableGroup.RaftingContainerGradientBegin];
            }
        }

        public override Color RaftingContainerGradientEnd
        {
            get
            {
                return _colors[ColorTableGroup.RaftingContainerGradientEnd];
            }
        }

        public override Color SeparatorDark
        {
            get
            {
                return _colors[ColorTableGroup.SeparatorDark];
            }
        }

        public override Color SeparatorLight
        {
            get
            {
                return _colors[ColorTableGroup.SeparatorLight];
            }
        }

        public override Color StatusStripGradientBegin
        {
            get
            {
                return _colors[ColorTableGroup.StatusStripGradientBegin];
            }
        }

        public override Color StatusStripGradientEnd
        {
            get
            {
                return _colors[ColorTableGroup.StatusStripGradientEnd];
            }
        }

        public override Color ToolStripBorder
        {
            get
            {
                return _colors[ColorTableGroup.ToolStripBorder];
            }
        }

        public override Color ToolStripDropDownBackground
        {
            get
            {
                return _colors[ColorTableGroup.ToolStripDropDownBackground];
            }
        }

        public override Color ToolStripGradientBegin
        {
            get
            {
                return _colors[ColorTableGroup.ToolStripGradientBegin];
            }
        }

        public override Color ToolStripGradientMiddle
        {
            get
            {
                return _colors[ColorTableGroup.ToolStripGradientMiddle];
            }
        }

        public override Color ToolStripGradientEnd
        {
            get
            {
                return _colors[ColorTableGroup.ToolStripGradientEnd];
            }
        }

        public override Color ToolStripContentPanelGradientBegin
        {
            get
            {
                return _colors[ColorTableGroup.ToolStripContentPanelGradientBegin];
            }
        }

        public override Color ToolStripContentPanelGradientEnd
        {
            get
            {
                return _colors[ColorTableGroup.ToolStripContentPanelGradientEnd];
            }
        }

        public override Color ToolStripPanelGradientBegin
        {
            get
            {
                return _colors[ColorTableGroup.ToolStripPanelGradientBegin];
            }
        }

        public override Color ToolStripPanelGradientEnd
        {
            get
            {
                return _colors[ColorTableGroup.ToolStripPanelGradientEnd];
            }
        }

        public override Color OverflowButtonGradientBegin
        {
            get
            {
                return _colors[ColorTableGroup.OverflowButtonGradientBegin];
            }
        }

        public override Color OverflowButtonGradientMiddle
        {
            get
            {
                return _colors[ColorTableGroup.OverflowButtonGradientMiddle];
            }
        }

        public override Color OverflowButtonGradientEnd
        {
            get
            {
                return _colors[ColorTableGroup.OverflowButtonGradientEnd];
            }
        }

        #region Implementation of ICloneable

        public object Clone()
        {
            var clone = (CustomizableColorTable)MemberwiseClone();
            clone._colors = new Dictionary<ColorTableGroup, Color>(clone._colors);
            return clone;
        }

        #endregion
    }
}