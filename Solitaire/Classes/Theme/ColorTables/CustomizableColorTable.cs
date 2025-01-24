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
                if (prop != null)
                {
                    _colors[colorTableGroup] = (Color) prop.GetValue(colorTable, null);
                }
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
                if (prop != null)
                {
                    _colors[colorTableGroup] = (Color) prop.GetValue(ProfessionalColorTable, null);
                }
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
            get => _colors[colorGroup];
            set => _colors[colorGroup] = value;
        }

        public override Color ButtonSelectedBorder => _colors[ColorTableGroup.ButtonSelectedBorder];

        public override Color ButtonCheckedGradientBegin => _colors[ColorTableGroup.ButtonCheckedGradientBegin];

        public override Color ButtonCheckedGradientMiddle => _colors[ColorTableGroup.ButtonCheckedGradientMiddle];

        public override Color ButtonCheckedGradientEnd => _colors[ColorTableGroup.ButtonCheckedGradientEnd];

        public override Color ButtonSelectedGradientBegin => _colors[ColorTableGroup.ButtonSelectedGradientBegin];

        public override Color ButtonSelectedGradientMiddle => _colors[ColorTableGroup.ButtonSelectedGradientMiddle];

        public override Color ButtonSelectedGradientEnd => _colors[ColorTableGroup.ButtonSelectedGradientEnd];

        public override Color ButtonPressedGradientBegin => _colors[ColorTableGroup.ButtonPressedGradientBegin];

        public override Color ButtonPressedGradientMiddle => _colors[ColorTableGroup.ButtonPressedGradientMiddle];

        public override Color ButtonPressedGradientEnd => _colors[ColorTableGroup.ButtonPressedGradientEnd];

        public override Color CheckBackground => _colors[ColorTableGroup.CheckBackground];

        public override Color CheckSelectedBackground => _colors[ColorTableGroup.CheckSelectedBackground];

        public override Color CheckPressedBackground => _colors[ColorTableGroup.CheckPressedBackground];

        public override Color GripDark => _colors[ColorTableGroup.GripDark];

        public override Color GripLight => _colors[ColorTableGroup.GripLight];

        public override Color ImageMarginGradientBegin => _colors[ColorTableGroup.ImageMarginGradientBegin];

        public override Color ImageMarginGradientMiddle => _colors[ColorTableGroup.ImageMarginGradientMiddle];

        public override Color ImageMarginGradientEnd => _colors[ColorTableGroup.ImageMarginGradientEnd];

        public override Color ImageMarginRevealedGradientBegin => _colors[ColorTableGroup.ImageMarginRevealedGradientBegin];

        public override Color ImageMarginRevealedGradientMiddle => _colors[ColorTableGroup.ImageMarginRevealedGradientMiddle];

        public override Color ImageMarginRevealedGradientEnd => _colors[ColorTableGroup.ImageMarginRevealedGradientEnd];

        public override Color MenuStripGradientBegin => _colors[ColorTableGroup.MenuStripGradientBegin];

        public override Color MenuStripGradientEnd => _colors[ColorTableGroup.MenuStripGradientEnd];

        public override Color MenuItemSelected => _colors[ColorTableGroup.MenuItemSelected];

        public override Color MenuItemBorder => _colors[ColorTableGroup.MenuItemBorder];

        public override Color MenuBorder => _colors[ColorTableGroup.MenuBorder];

        public override Color MenuItemSelectedGradientBegin => _colors[ColorTableGroup.MenuItemSelectedGradientBegin];

        public override Color MenuItemSelectedGradientEnd => _colors[ColorTableGroup.MenuItemSelectedGradientEnd];

        public override Color MenuItemPressedGradientBegin => _colors[ColorTableGroup.MenuItemPressedGradientBegin];

        public override Color MenuItemPressedGradientMiddle => _colors[ColorTableGroup.MenuItemPressedGradientMiddle];

        public override Color MenuItemPressedGradientEnd => _colors[ColorTableGroup.MenuItemPressedGradientEnd];

        public override Color RaftingContainerGradientBegin => _colors[ColorTableGroup.RaftingContainerGradientBegin];

        public override Color RaftingContainerGradientEnd => _colors[ColorTableGroup.RaftingContainerGradientEnd];

        public override Color SeparatorDark => _colors[ColorTableGroup.SeparatorDark];

        public override Color SeparatorLight => _colors[ColorTableGroup.SeparatorLight];

        public override Color StatusStripGradientBegin => _colors[ColorTableGroup.StatusStripGradientBegin];

        public override Color StatusStripGradientEnd => _colors[ColorTableGroup.StatusStripGradientEnd];

        public override Color ToolStripBorder => _colors[ColorTableGroup.ToolStripBorder];

        public override Color ToolStripDropDownBackground => _colors[ColorTableGroup.ToolStripDropDownBackground];

        public override Color ToolStripGradientBegin => _colors[ColorTableGroup.ToolStripGradientBegin];

        public override Color ToolStripGradientMiddle => _colors[ColorTableGroup.ToolStripGradientMiddle];

        public override Color ToolStripGradientEnd => _colors[ColorTableGroup.ToolStripGradientEnd];

        public override Color ToolStripContentPanelGradientBegin => _colors[ColorTableGroup.ToolStripContentPanelGradientBegin];

        public override Color ToolStripContentPanelGradientEnd => _colors[ColorTableGroup.ToolStripContentPanelGradientEnd];

        public override Color ToolStripPanelGradientBegin => _colors[ColorTableGroup.ToolStripPanelGradientBegin];

        public override Color ToolStripPanelGradientEnd => _colors[ColorTableGroup.ToolStripPanelGradientEnd];

        public override Color OverflowButtonGradientBegin => _colors[ColorTableGroup.OverflowButtonGradientBegin];

        public override Color OverflowButtonGradientMiddle => _colors[ColorTableGroup.OverflowButtonGradientMiddle];

        public override Color OverflowButtonGradientEnd => _colors[ColorTableGroup.OverflowButtonGradientEnd];

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