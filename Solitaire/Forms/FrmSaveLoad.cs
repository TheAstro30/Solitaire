/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Solitaire.Classes.Helpers;
using Solitaire.Classes.Helpers.Management;
using Solitaire.Classes.Helpers.UI;
using Solitaire.Classes.Settings.SettingsData;
using Solitaire.Controls;
using Solitaire.Controls.ObjectListView;
using Solitaire.Controls.ObjectListView.Rendering.Styles;
using Solitaire.Properties;
using ObjectListView = Solitaire.Controls.ObjectListView.ObjectListView;

namespace Solitaire.Forms
{
    /* Load/save game files manager dialog */
    public enum SaveLoadType
    {
        LoadGame = 0,
        SaveGame = 1
    }

    public sealed class FrmSaveLoad : FormEx
    {
        private readonly SaveLoadType _type;

        private readonly ObjectListView _lvFiles;

        private readonly Button _btnSaveLoad;
        private readonly Button _btnDelete;
        private readonly Button _btnClear;

        public SaveLoadData SelectedFile { get; private set; }

        public FrmSaveLoad(SaveLoadType type)
        {
            _type = type;

            ClientSize = new Size(469, 387);
            Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;

            var image = new ImageList {ColorDepth = ColorDepth.Depth32Bit, ImageSize = new Size(64, 64)};
            image.Images.Add(Resources.savedGame);

            /* Object list view - large icon type Windows Explorer feel */
            var lvHeader = new HeaderFormatStyle();
            _lvFiles = new FastObjectListView
            {
                HeaderStyle = ColumnHeaderStyle.None,
                HideSelection = true,
                MultiSelect = false,
                Location = new Point(12, 12),
                Size = new Size(359, 324),
                TabIndex = 0,
                LargeImageList = image,
                UseCompatibleStateImageBehavior = false,
                HeaderFormatStyle = lvHeader,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0),
                View = View.LargeIcon,
                ShowItemToolTips = true
            };
            var lvColumn = new OlvColumn(@"Saved games:", "ToString")
            {
                Groupable = false,
                Hideable = false,
                IsEditable = false,
                Searchable = false,
                TextAlign = HorizontalAlignment.Center,
                Width = 359,
                ImageIndex = 0
            };
            _lvFiles.AllColumns.Add(lvColumn);
            _lvFiles.Columns.Add(lvColumn);

            _btnSaveLoad = new Button
            {
                DialogResult = DialogResult.OK,
                Enabled = false,
                Location = new Point(382, 12),
                Size = new Size(75, 28),
                TabIndex = 1,
                Text = @"Load",
                UseVisualStyleBackColor = false,
                BackgroundImage = Resources.button_ok,
                BackgroundImageLayout = ImageLayout.Tile,
                ForeColor = Color.White
            };

            var btnSaveAs = new Button
            {
                Location = new Point(382, 46),
                Size = new Size(75, 28),
                TabIndex = 2,
                Tag = "SAVEAS",
                Text = @"Save As",
                UseVisualStyleBackColor = false,
                Visible = false,
                BackgroundImage = Resources.button_other,
                BackgroundImageLayout = ImageLayout.Tile
            };

            _btnDelete = new Button
            {
                Enabled = false,
                Location = new Point(382, 80),
                Size = new Size(75, 28),
                TabIndex = 3,
                Tag = "DELETE",
                Text = @"Delete",
                UseVisualStyleBackColor = false,
                BackgroundImage = Resources.button_other,
                BackgroundImageLayout = ImageLayout.Tile
            };

            _btnClear = new Button
            {
                Enabled = false,
                Location = new Point(382, 309),
                Size = new Size(75, 28),
                TabIndex = 4,
                Tag = "CLEAR",
                Text = @"Clear",
                UseVisualStyleBackColor = false,
                BackgroundImage = Resources.button_other,
                BackgroundImageLayout = ImageLayout.Tile
            };

            var btnClose = new Button
            {
                DialogResult = DialogResult.Cancel,
                Location = new Point(382, 353),
                Size = new Size(75, 28),
                TabIndex = 5,
                Text = @"Close",
                UseVisualStyleBackColor = false,
                BackgroundImage = Resources.button_cancel,
                BackgroundImageLayout = ImageLayout.Tile,
                ForeColor = Color.White
            };

            switch (type)
            {
                case SaveLoadType.LoadGame:
                    Text = @"Load Saved Game";
                    _btnDelete.Location = new Point(382, 46);
                    break;

                case SaveLoadType.SaveGame:
                    Text = @"Save Current Game";
                    _btnSaveLoad.Text = @"Save";
                    btnSaveAs.Visible = true;
                    break;
            }

            Controls.AddRange(new Control[]
            {
                _lvFiles, _btnSaveLoad, btnSaveAs, _btnDelete, _btnClear, btnClose
            });

            _lvFiles.SelectionChanged += OnSelectionChanged;
            _lvFiles.MouseDoubleClick += OnDoubleClick;

            _btnSaveLoad.Click += OnButtonClick;
            btnSaveAs.Click += OnButtonClick;
            _btnDelete.Click += OnButtonClick;
            _btnClear.Click += OnButtonClick;

            _lvFiles.AddObjects(SettingsManager.Settings.SavedGames.Data);

            if (_lvFiles.Items.Count == 0)
            {
                return;
            }
            /* Enable clear button */
            _btnClear.Enabled = true;
            /* Scroll to bottom of list */
            _lvFiles.EnsureVisible(_lvFiles.Items.Count - 1);
            _lvFiles.Focus();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            /* Verify the selected file name doesn't already exist, if it does ask user if they want to overwrite */
            if (DialogResult == DialogResult.OK && _type == SaveLoadType.SaveGame)
            {
                if (SettingsManager.Settings.SavedGames.Data
                    .Where(f => f.FriendlyName.ToLower().Equals(SelectedFile.FriendlyName.ToLower())).Any(f =>
                        CustomMessageBox.Show(this,
                            $"A file name: '{SelectedFile.FriendlyName}' already exists.\r\n\r\nOverwrite?",
                            "Save As") == DialogResult.No))
                {
                    e.Cancel = true;
                    return;
                }
            }
            base.OnFormClosing(e);
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            var o = (Button) sender;
            if (o == null)
            {
                return;
            }
            /* Bollocks */
            var path = Utils.MainDir(@"\KangaSoft\Solitaire\saved", true);
            switch (o.Tag)
            {
                case "SAVEAS":
                    using (var f = new FrmInput {TitleText = "Save As", CaptionText = "Choose a name for your saved game:"})
                    {
                        if (f.ShowDialog(this) == DialogResult.OK)
                        {
                            var file = SanitizeFileName(f.InputText);
                            var data = new SaveLoadData
                            {
                                DateTime = DateTime.Now.ToString("MMM dd, yyyy"),
                                FriendlyName = file
                            };
                            SelectedFile = data;
                            DialogResult = DialogResult.OK;
                            Close();
                        }
                    }
                    break;

                case "DELETE":
                    try
                    {
                        if (CustomMessageBox.Show(this,
                            $"Are you sure you want to delete '{SelectedFile.FriendlyName}'?",
                            "Delete File") == DialogResult.No)
                        {
                            return;
                        }
                        var file = $@"{path}\{SelectedFile.FileName}";
                        if (File.Exists(file))
                        {
                            File.Delete(file);
                        }
                        /* Remove it from list */
                        SettingsManager.Settings.SavedGames.Data.Remove(SelectedFile);
                        _lvFiles.RemoveObject(SelectedFile);
                        SelectedFile = null;
                    }
                    catch
                    {
                        /* Ignore IO exceptions */
                    }
                    _btnDelete.Enabled = false;
                    break;

                case "CLEAR":
                    if (CustomMessageBox.Show(this,
                        "Are you sure you want to delete all saved games?",
                        "Delete Saved Games") == DialogResult.No)
                    {
                        return;
                    }

                    try
                    {
                        foreach (var file in SettingsManager.Settings.SavedGames.Data.Select(f => $@"{path}\{f.FileName}").Where(File.Exists))
                        {
                            File.Delete(file);
                        }
                        _lvFiles.ClearObjects();
                        SettingsManager.Settings.SavedGames.Data.Clear();
                    }
                    catch
                    {
                        /* Ignore IO exceptions */
                    }
                    _btnClear.Enabled = false;
                    break;
            }
        }

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            var o = (ObjectListView) sender;
            if (o == null)
            {
                return;
            }
            var selected = o.SelectedObject != null;

            _btnSaveLoad.Enabled = selected;
            _btnDelete.Enabled = selected;

            SelectedFile = (SaveLoadData) o.SelectedObject;
        }

        private void OnDoubleClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private static string SanitizeFileName(string name)
        {
            var c = Path.GetInvalidFileNameChars();
            return string.Join("_", name.Split(c, StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
