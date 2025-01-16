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
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            Name = name;
        }

        public string Name { get; protected set; }

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
    }
}
