using System;
using System.Drawing;
using System.Windows.Forms;
using Solitaire.Classes.UI;

namespace Solitaire.Forms
{
    public partial class FrmDeckBack : FormEx
    {
        public int SelectedImage { get; private set; }

        public FrmDeckBack(Game game, int imageIndex)
        {
            InitializeComponent();

            var images = new ImageList();
            images.Images.AddRange(game.ObjectData.CardBacks.ToArray());
            images.ImageSize = new Size(80, 124);
            lvImages.LargeImageList = images;            
            /* Add images to listview */
            for (var i = 0; i <= images.Images.Count - 1; i++)
            {
                lvImages.Items.Add(new ListViewItem {ImageIndex = i, Selected = imageIndex == i});
            }

            lvImages.SelectedIndexChanged += OnListViewSelectionChanged;
            lvImages.MouseDoubleClick += OnListViewItemDoubleClick;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                SelectedImage = lvImages.SelectedItems[0].Index;
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
            btnOk.Enabled = o.SelectedItems.Count > 0;
        }

        private void OnListViewItemDoubleClick(object sender, EventArgs e)
        {
            var o = (ListView)sender;
            if (o.SelectedItems.Count == 0)
            {
                return;
            }
            SelectedImage = lvImages.SelectedItems[0].Index;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
