/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Solitaire.Classes.Theme.ColorTables
{
    public abstract class PresetColorTable : ProfessionalColorTable
    {
        protected PresetColorTable(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; protected set; }

        public override Color ButtonSelectedHighlight => ButtonSelectedGradientMiddle;

        public override Color ButtonSelectedHighlightBorder => ButtonSelectedBorder;

        public override Color ButtonPressedHighlight => ButtonPressedGradientMiddle;

        public override Color ButtonPressedHighlightBorder => ButtonPressedBorder;

        public override Color ButtonCheckedHighlight => ButtonCheckedGradientMiddle;

        public override Color ButtonCheckedHighlightBorder => ButtonSelectedBorder;

        public override Color ButtonPressedBorder => ButtonSelectedBorder;
    }
}
