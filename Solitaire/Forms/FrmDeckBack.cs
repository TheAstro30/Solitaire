/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Drawing;
using System.Windows.Forms;
using Solitaire.Classes.UI;
using Solitaire.Controls;

namespace Solitaire.Forms
{
    public sealed class FrmDeckBack : FormEx
    {
        private readonly ListView _lvImages;
        private readonly Button _btnOk;
        
        public int SelectedImage { get; private set; }

        public FrmDeckBack(Game game, int imageIndex)
        {
            ClientSize = new Size(424, 324);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = @"Choose Deck Image";

            var images = new ImageList
            {
                ColorDepth = ColorDepth.Depth32Bit,
                ImageSize = new Size((int)(game.CardSize.Width / 1.5), (int)(game.CardSize.Height / 1.5))
            };
            images.Images.AddRange(game.ObjectData.CardBacks.ToArray());

            _lvImages = new ListView
            {
                Location = new Point(12, 12),
                MultiSelect = false,
                Size = new Size(400, 260),
                TabIndex = 0,
                UseCompatibleStateImageBehavior = false,
                LargeImageList = images
            };

            /* Add images to listview */
            for (var i = 0; i <= images.Images.Count - 1; i++)
            {
                _lvImages.Items.Add(new ListViewItem { ImageIndex = i, Selected = imageIndex == i });
            }

            _btnOk = new Button
            {
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0),
                DialogResult = DialogResult.OK,
                Enabled = false,
                Location = new Point(302, 289),
                Size = new Size(110, 28),
                TabIndex = 1,
                Text = @"Ok",
                BackgroundImage = game.ObjectData.ButtonOk,
                BackgroundImageLayout = ImageLayout.Tile,
                ForeColor = Color.White
            };

            var btnCancel = new Button
            {
                DialogResult = DialogResult.Cancel,
                Location = new Point(186, 289),
                Size = new Size(110, 28),
                TabIndex = 2,
                Text = @"Cancel",
                BackgroundImage = game.ObjectData.ButtonCancel,
                BackgroundImageLayout = ImageLayout.Tile,
                ForeColor = Color.White
            };                    

            Controls.AddRange(new Control[]
            {
                _lvImages, _btnOk, btnCancel
            });

            AcceptButton = _btnOk;

            _lvImages.SelectedIndexChanged += OnListViewSelectionChanged;
            _lvImages.MouseDoubleClick += OnListViewItemDoubleClick;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                SelectedImage = _lvImages.SelectedItems[0].Index;
            }
            else
            {
                SelectedImage = -1;
            }
            base.OnFormClosing(e);
        }

        private void OnListViewSelectionChanged(object sender, EventArgs e)
        {
            var o = (ListView) sender;
            if (o.SelectedItems.Count == 0)
            {
                return;
            }
            _btnOk.Enabled = o.SelectedItems.Count > 0;
            /* Bring selection into view */
            o.EnsureVisible(o.SelectedItems[0].Index);
        }

        private void OnListViewItemDoubleClick(object sender, EventArgs e)
        {
            var o = (ListView)sender;
            if (o.SelectedItems.Count == 0)
            {
                return;
            }
            SelectedImage = _lvImages.SelectedItems[0].Index;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
