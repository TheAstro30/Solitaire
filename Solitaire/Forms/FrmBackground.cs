/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Drawing;
using System.Windows.Forms;
using libolv;
using libolv.Rendering.Styles;
using Solitaire.Classes.Data;
using Solitaire.Classes.Helpers.Management;
using Solitaire.Classes.UI;
using Solitaire.Controls;
using Solitaire.Properties;

namespace Solitaire.Forms
{
    public partial class FrmBackground : FormEx
    {
        /* When was the last time this place was cleaned? - Gordon Ramsay */
        public BackgroundImageData SelectedBackground { get; private set; }

        public FrmBackground(Game ctl)
        {
            InitializeComponent();

            var images = new ImageList {ColorDepth = ColorDepth.Depth32Bit};
            images.Images.AddRange(new Image[]
                {
                    Resources.picture.ToBitmap(),
                    Resources.color.ToBitmap()
                }
            );

            var header = new HeaderFormatStyle();
            lvFiles.HeaderFormatStyle = header;
            lvFiles.HeaderStyle = ColumnHeaderStyle.None;
            lvFiles.SmallImageList = images;

            var tvFiles = new OlvColumn(@"Backgrounds:", "Name")
            {
                Groupable = false,
                Hideable = false,
                IsEditable = false,
                Searchable = false,
                Width = 161
            };

            lvFiles.AllColumns.Add(tvFiles);
            lvFiles.Columns.Add(tvFiles);

            lvFiles.SelectedIndexChanged += OnListSelectedIndexChanged;
            lvFiles.MouseDoubleClick += OnListDoubleClick;

            for (var i = 0; i <= ctl.ObjectData.Backgrounds.Count - 1; i++)
            {
                var bg = ctl.ObjectData.Backgrounds[i];
                lvFiles.AddObject(bg);
                switch (bg.ImageLayout)
                {
                    case BackgroundImageDataLayout.Tile:
                    case BackgroundImageDataLayout.Stretch:
                        lvFiles.Items[i].ImageIndex = 0;
                        break;

                    case BackgroundImageDataLayout.Color:
                        lvFiles.Items[i].ImageIndex = 1;
                        break;
                }
                if (i == SettingsManager.Settings.Options.Background)
                {
                    lvFiles.SelectedIndex = i;
                }
            }

            if (lvFiles.SelectedObject != null)
            {
                lvFiles.EnsureModelVisible(lvFiles.SelectedObject);
            }

            AcceptButton = btnOk;
        }

        private void OnListSelectedIndexChanged(object sender, EventArgs e)
        {
            var o = (ObjectListView) sender;
            if (o == null)
            {
                return;
            }
            /* Generate the preview image */
            SelectedBackground = (BackgroundImageData)o.SelectedObject;
            if (SelectedBackground == null)
            {
                pbPreview.BackgroundImage = null;
            }
            else
            {
                switch (SelectedBackground.ImageLayout)
                {
                    case BackgroundImageDataLayout.Tile:
                        pbPreview.BackgroundImage = SelectedBackground.Image;
                        pbPreview.BackgroundImageLayout = ImageLayout.Tile;
                        break;

                    case BackgroundImageDataLayout.Stretch:
                        pbPreview.BackgroundImage = SelectedBackground.Image;
                        pbPreview.BackgroundImageLayout = ImageLayout.Stretch;
                        break;

                    case BackgroundImageDataLayout.Color:
                        /* Not implemented yet */
                        break;
                }
            }
        }

        private void OnListDoubleClick(object sender, MouseEventArgs e)
        {
            var o = (ObjectListView) sender;
            if (o == null || e.Button != MouseButtons.Left || o.SelectedObject == null)
            {
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
