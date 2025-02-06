/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Drawing;
using System.Windows.Forms;
using Solitaire.Classes.Data;
using Solitaire.Classes.Helpers.Management;
using Solitaire.Classes.Helpers.UI;
using Solitaire.Classes.UI;
using Solitaire.Controls;
using Solitaire.Controls.ObjectListView;
using Solitaire.Controls.ObjectListView.Rendering.Styles;
using Solitaire.Properties;
using ObjectListView = Solitaire.Controls.ObjectListView.ObjectListView;

namespace Solitaire.Forms
{
    public sealed class FrmBackground : FormEx
    {
        /* When was the last time this place was cleaned? - Gordon Ramsay */
        private readonly PictureBox _pbPreview;

        public BackgroundImageData SelectedBackground { get; private set; }

        public FrmBackground(Game ctl)
        {
            ClientSize = new Size(559, 332);
            Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = @"Choose Background";

            var images = new ImageList {ColorDepth = ColorDepth.Depth32Bit, ImageSize = new Size(16, 16)};
            images.Images.AddRange(new Image[]
                {
                    Resources.picture.ToBitmap(),
                    Resources.color.ToBitmap()
                }
            );

            var header = new HeaderFormatStyle();
            var lvBackgrounds = new FastObjectListView
            {
                HideSelection = false,
                Location = new Point(12, 12),
                MultiSelect = false,
                Size = new Size(161, 267),
                TabIndex = 0,
                UseCompatibleStateImageBehavior = false,
                View = View.Details,
                HeaderFormatStyle = header,
                HeaderStyle = ColumnHeaderStyle.None,
                SmallImageList = images,
                ShowItemToolTips = true
            };

            var colBackgrounds = new OlvColumn(@"Backgrounds:", "Name")
            {
                Groupable = false,
                Hideable = false,
                IsEditable = false,
                Searchable = false,
                Width = 161,
                /* Used for setting individual icons against items */
                ImageGetter = delegate(object row)
                {
                    switch (((BackgroundImageData) row).ImageLayout)
                    {
                        case BackgroundImageDataLayout.Stretch:
                        case BackgroundImageDataLayout.Tile:
                            return 0;

                        default:
                            return 1;
                    }
                }
            };

            lvBackgrounds.AllColumns.Add(colBackgrounds);
            lvBackgrounds.Columns.Add(colBackgrounds);

            lvBackgrounds.SelectedIndexChanged += OnListSelectedIndexChanged;
            lvBackgrounds.MouseDoubleClick += OnListDoubleClick;

            _pbPreview = new PictureBox
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(179, 12),
                Size = new Size(368, 267),
                TabStop = false
            };
            _pbPreview.Paint += OnPreviewPaint;

            var btnCancel = new Button
            {
                BackgroundImage = Resources.button_cancel,
                DialogResult = DialogResult.Cancel,
                ForeColor = Color.White,
                Location = new Point(321, 298),
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
                Location = new Point(437, 298),
                Size = new Size(110, 28),
                TabIndex = 2,
                Text = @"Ok",
                UseVisualStyleBackColor = false
            };

            Controls.AddRange(new Control[] {lvBackgrounds, _pbPreview, btnCancel, btnOk});

            /* Build the list of current available backgrounds - it's a for/next loop as I need to change the icon between image and color */
            for (var i = 0; i <= ctl.ObjectData.Backgrounds.Count - 1; i++)
            {
                var bg = ctl.ObjectData.Backgrounds[i];
                lvBackgrounds.AddObject(bg);
                if (i == SettingsManager.Settings.Options.Background)
                {
                    lvBackgrounds.SelectedIndex = i;
                }
            }

            /* Ensure the selected object (if any) is in view */
            if (lvBackgrounds.SelectedObject != null)
            {
                lvBackgrounds.EnsureModelVisible(lvBackgrounds.SelectedObject);
            }

            AcceptButton = btnOk;
            lvBackgrounds.Focus();
        }

        private void OnListSelectedIndexChanged(object sender, EventArgs e)
        {
            var o = (ObjectListView) sender;
            if (o == null)
            {
                SelectedBackground = null;
                return;
            }
            SelectedBackground = (BackgroundImageData) o.SelectedObject;
            _pbPreview.Invalidate();
        }

        private void OnListDoubleClick(object sender, MouseEventArgs e)
        {
            var o = (ObjectListView) sender;
            if (o == null || e.Button != MouseButtons.Left || o.SelectedObject == null)
            {
                return;
            }
            /* Penis */
            DialogResult = DialogResult.OK;
            Close();
        }

        private void OnPreviewPaint(object sender, PaintEventArgs e)
        {
            if (SelectedBackground == null)
            {
                e.Graphics.Clear(Color.White);
                return;
            }
            /* Generate the preview image */
            var bg = SelectedBackground;
            switch (bg.ImageLayout)
            {
                case BackgroundImageDataLayout.Tile:
                    e.Graphics.DrawImageTiled(bg.Image, _pbPreview.ClientSize);
                    break;

                case BackgroundImageDataLayout.Stretch:
                    e.Graphics.DrawImageStretched(bg.Image, _pbPreview.ClientSize);
                    break;

                case BackgroundImageDataLayout.Color:
                    /* Draw color to a bitmap and set it to pbPreview background image */
                    var bgColors = bg.BackgroundColor;
                    if (bgColors.Count < 2)
                    {
                        /* Single color */
                        using (var brush = new SolidBrush(bgColors[0]))
                        {
                            var rect = new Rectangle(0, 0, _pbPreview.ClientSize.Width, _pbPreview.ClientSize.Height);
                            e.Graphics.FillRectangle(brush, rect);
                        }
                    }
                    else
                    {
                        /* Gradient */
                        e.Graphics.DrawGradient(bgColors.ToArray(), _pbPreview.ClientSize);
                    }
                    break;
            }
        }
    }
}
