/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Drawing;
using System.Windows.Forms;
using Solitaire.Classes.UI;

namespace Solitaire.Forms
{
    public sealed class FrmDeckBack : FormEx
    {
        private readonly ListView _lvImages;
        private readonly Button _btnOk;

        public int SelectedImage { get; private set; }

        public FrmDeckBack(Game game, int imageIndex)
        {
            AcceptButton = _btnOk;
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
                ImageSize = new Size((int) (game.CardSize.Width/1.5), (int) (game.CardSize.Height/1.5))
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

            _btnOk = new Button
            {
                DialogResult = DialogResult.OK,
                Enabled = false,
                Location = new Point(337, 289),
                Size = new Size(75, 23),
                TabIndex = 1,
                Text = @"Ok",
                UseVisualStyleBackColor = true
            };

            var btnCancel = new Button
            {
                DialogResult = DialogResult.Cancel,
                Location = new Point(256, 289),
                Size = new Size(75, 23),
                TabIndex = 2,
                Text = @"Cancel",
                UseVisualStyleBackColor = true
            };
                     
            /* Add images to listview */
            for (var i = 0; i <= images.Images.Count - 1; i++)
            {
                _lvImages.Items.Add(new ListViewItem {ImageIndex = i, Selected = imageIndex == i});
            }

            Controls.AddRange(new Control[]
            {
                _lvImages, _btnOk, btnCancel
            });

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
            _btnOk.Enabled = o.SelectedItems.Count > 0;
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
