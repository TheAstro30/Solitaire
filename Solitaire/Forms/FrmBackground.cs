/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
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
    public sealed class FrmBackground : FormEx
    {
        /* When was the last time this place was cleaned? - Gordon Ramsay */
        private Bitmap _previewBitmap;
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

            var images = new ImageList {ColorDepth = ColorDepth.Depth32Bit};
            images.Images.AddRange(new Image[]
                {
                    Resources.picture.ToBitmap(),
                    Resources.color.ToBitmap()
                }
            );

            var header = new HeaderFormatStyle();
            var lvBackgrounds = new ObjectListView
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
                Width = 161
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
                switch (bg.ImageLayout)
                {
                    case BackgroundImageDataLayout.Tile:
                    case BackgroundImageDataLayout.Stretch:
                        lvBackgrounds.Items[i].ImageIndex = 0;
                        break;

                    case BackgroundImageDataLayout.Color:
                        lvBackgrounds.Items[i].ImageIndex = 1;
                        break;
                }
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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _previewBitmap?.Dispose();
            _previewBitmap = null;
            base.OnFormClosing(e);
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
                _pbPreview.BackgroundImage = null;
            }
            else
            {
                var bg = SelectedBackground;
                switch (bg.ImageLayout)
                {
                    case BackgroundImageDataLayout.Tile:
                        _pbPreview.BackgroundImage = bg.Image;
                        _pbPreview.BackgroundImageLayout = ImageLayout.Tile;
                        break;

                    case BackgroundImageDataLayout.Stretch:
                        _pbPreview.BackgroundImage = bg.Image;
                        _pbPreview.BackgroundImageLayout = ImageLayout.Stretch;
                        break;

                    case BackgroundImageDataLayout.Color:
                        /* Draw color to a bitmap and set it to pbPreview background image */
                        _previewBitmap = new Bitmap(_pbPreview.Width, _pbPreview.Height);
                        var bgColors = bg.BackgroundColor;
                        if (bgColors.Count == 0)
                        {
                            return;
                        }
                        using (var g = Graphics.FromImage(_previewBitmap))
                        {
                            if (bgColors.Count < 2)
                            {
                                /* Single color */
                                using (var brush = new SolidBrush(bgColors[0]))
                                {
                                    g.FillRectangle(brush, new Rectangle(0, 0, _previewBitmap.Width, _previewBitmap.Height));
                                }
                            }
                            else
                            {
                                /* Gradient */
                                var rect = new Rectangle(0, 0, _previewBitmap.Width, _previewBitmap.Height);
                                using (var gradient = new LinearGradientBrush(rect, Color.White, Color.White, LinearGradientMode.Vertical))
                                {
                                    var cb = new ColorBlend();
                                    var colors = bgColors.ToArray();
                                    cb.Colors = colors;
                                    cb.Positions = colors.Select((t, i) => (float) i / (colors.Length - 1)).ToArray();
                                    gradient.InterpolationColors = cb;
                                    g.FillRectangle(gradient, new Rectangle(0, 0, _previewBitmap.Width, _previewBitmap.Height));
                                }
                            }
                        }
                        _pbPreview.BackgroundImage = _previewBitmap;
                        _pbPreview.BackgroundImageLayout = ImageLayout.Center;
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
            /* Penis */
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
