/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Solitaire.Controls;

namespace Solitaire.Forms
{
    public partial class FrmSaveLoad : FormEx
    {
        private readonly libolv.ObjectListView _lvFiles;

        public FrmSaveLoad()
        {
            InitializeComponent();

            _lvFiles = new libolv.ObjectListView
            {
                HeaderStyle = ColumnHeaderStyle.None,
                HideSelection = false,
                Location = new Point(12, 12),
                Name = "lvFiles",
                Size = new Size(359, 324),
                TabIndex = 0,
                UseCompatibleStateImageBehavior = false
            };

            Controls.Add(_lvFiles);
        }
    }
}
