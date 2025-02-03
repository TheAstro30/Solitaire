/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using libolv;
using libolv.Rendering.Styles;
using Solitaire.Classes;
using Solitaire.Classes.Data;
using Solitaire.Classes.Helpers;
using Solitaire.Classes.Helpers.Management;
using Solitaire.Classes.Helpers.UI;
using Solitaire.Classes.Serialization;
using Solitaire.Classes.Settings.SettingsData;
using Solitaire.Controls;
using Solitaire.Properties;

namespace Solitaire.Forms
{
    public sealed class FrmCards : FormEx
    {
        private readonly UiSynchronize _sync;
        private FolderSearch _files;
        private readonly List<CardSetData> _installedSets;
        private readonly ImageList _images;

        private readonly ObjectListView _lvCards;
        private readonly PictureBox _pbPreview;

        public CardSetData Cards { get; private set; }

        public FrmCards()
        {
            _sync = new UiSynchronize(this);

            ClientSize = new Size(443, 318);
            Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmCards";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = @"Choose Card Set";

            var smallImages = new ImageList {ColorDepth = ColorDepth.Depth32Bit, ImageSize = new Size(16, 16)};
            smallImages.Images.Add(Resources.picture);

            _images = new ImageList {ColorDepth = ColorDepth.Depth32Bit, ImageSize = new Size(157, 162)};

            _installedSets = new List<CardSetData>();

            var lvHeader = new HeaderFormatStyle();

            _lvCards = new ObjectListView
            {
                HideSelection = false,
                Location = new Point(12, 12),
                MultiSelect = false,
                Size = new Size(161, 252),
                TabIndex = 0,
                UseCompatibleStateImageBehavior = false,
                HeaderFormatStyle = lvHeader,
                HeaderStyle = ColumnHeaderStyle.None,
                View = View.Details,
                SmallImageList = smallImages
            };

            var lvColumn = new OlvColumn(@"Installed card sets:", "Name")
            {
                Groupable = false,
                Hideable = false,
                IsEditable = false,
                Searchable = false,
                ImageIndex = 0,
                Width = 161
            };
            _lvCards.AllColumns.Add(lvColumn);
            _lvCards.Columns.Add(lvColumn);

            _lvCards.SelectedIndexChanged += OnListViewSelectedIndexChanged;
            _lvCards.MouseDoubleClick += OnListViewDoubleClick;

            _pbPreview = new PictureBox
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(179, 12),
                Size = new Size(252, 252),
                TabStop = false,
                BackgroundImageLayout = ImageLayout.Center
            };

            var btnCancel = new Button
            {
                BackgroundImage = Resources.button_cancel,
                DialogResult = DialogResult.Cancel,
                ForeColor = Color.White,
                Location = new Point(205, 284),
                Size = new Size(110, 28),
                TabIndex = 1,
                Text = @"Cancel",
                UseVisualStyleBackColor = false
            };

            var btnOk = new Button
            {
                BackgroundImage = Resources.button_ok,
                DialogResult = DialogResult.OK,
                ForeColor = Color.White,
                Location = new Point(321, 284),
                Size = new Size(110, 28),
                TabIndex = 2,
                Text = @"Ok",
                UseVisualStyleBackColor = false
            };

            Controls.AddRange(new Control[] {_lvCards, _pbPreview, btnCancel, btnOk});

            AcceptButton = btnOk;
        }

        protected override void OnLoad(EventArgs e)
        {
            /* Wait for form handle to be created before calling file search */
            _files = new FolderSearch();

            _files.OnFileFound += OnCardSetFound;
            _files.OnFileSearchCompleted += OnCardSetFoundComplete;

            var d = new DirectoryInfo(Utils.MainDir(@"data\gfx\cards\", false));
            _files.BeginSearch(d, "*.dat", "*", false);
            base.OnLoad(e);
        }
        
        private void OnCardSetFound(string s)
        {
            /* Seems kind of stupid to load all in to memory... but, it's only being used on this form */
            var c = new Cards();
            if (!BinarySerialize<Cards>.Load(s, ref c))
            {
                return;
            }
            /* We don't want to store entire class of 52 images, we just want the preview image */
            _images.Images.Add(c.PreviewImage);
            var set = new CardSetData
            {
                Name = c.Name,
                FilePath = Path.GetFileName(s)
            };
            _installedSets.Add(set);
        }

        private void OnCardSetFoundComplete(FolderSearch obj)
        {
            /* Build listview items */
            if (_lvCards.InvokeRequired)
            {
                _sync.Execute(() => OnCardSetFoundComplete(obj));
                return;
            }
            GC.Collect(); /* Call this to get rid of junk in memory from above method */
            for (var i = 0; i <= _installedSets.Count - 1; i++)
            {
                var o = _installedSets[i];
                _lvCards.AddObject(o);
                if (!o.Name.Equals(SettingsManager.Settings.Options.CardSet.Name, StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }
                _lvCards.SelectedObject = o;
                _lvCards.EnsureModelVisible(o);
            }
        }

        private void OnListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            var o = (ObjectListView)sender;
            if (o == null)
            {
                return;
            }
            Cards = (CardSetData)o.SelectedObject;
            /* Update preview */
            if (o.SelectedObject == null)
            {
                _pbPreview.BackgroundImage = null;
                return;
            }
            _pbPreview.BackgroundImage = _images.Images[_lvCards.SelectedIndex];
        }

        private void OnListViewDoubleClick(object sender, MouseEventArgs e)
        {
            var o = (ObjectListView)sender;
            if (o?.SelectedObject == null || e.Button != MouseButtons.Left)
            {
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
