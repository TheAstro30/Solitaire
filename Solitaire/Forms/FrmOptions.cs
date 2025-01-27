/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Drawing;
using System.Windows.Forms;
using Solitaire.Classes.Helpers.Management;
using Solitaire.Controls;
using Solitaire.Controls.TrackBar;
using Solitaire.Properties;

namespace Solitaire.Forms
{
    public sealed class FrmOptions : FormEx
    {
        private bool _init;

        private readonly CheckBox _chkAutoTurn;
        private readonly CheckBox _chkProgress;
        private readonly CheckBox _chkTips;
        private readonly CheckBox _chkSave;
        private readonly CheckBox _chkHighlight;

        private readonly CheckBox _chkEffects;
        private readonly TrackBarEx _tbEffects;
        private readonly TrackBarEx _tbMusic;

        private readonly CheckBox _chkNew;
        private readonly CheckBox _chkExit;

        public FrmOptions()
        {
            _init = true;

            ClientSize = new Size(298, 368);
            Font = new Font("Segoe UI Semibold", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = @"Options";

            var gbGeneral = new GroupBox
            {
                BackColor = Color.Transparent,
                Location = new Point(12, 12),
                Size = new Size(274, 141),
                TabStop = false,
                Text = @"General:"
            };

            _chkAutoTurn = new CheckBox
            {
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(6, 21),
                Size = new Size(187, 17),
                TabIndex = 0,
                Text = @"Automatically turn hidden cards",
                UseVisualStyleBackColor = false
            };

            _chkProgress = new CheckBox
            {
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(6, 44),
                Size = new Size(221, 17),
                TabIndex = 1,
                Text = @"Show completed progress in statusbar",
                UseVisualStyleBackColor = false
            };

            _chkTips = new CheckBox
            {
                AutoSize = true,
                Location = new Point(6, 67),
                Size = new Size(137, 17),
                TabIndex = 2,
                Text = @"Show tips in statusbar",
                UseVisualStyleBackColor = true
            };

            _chkSave = new CheckBox
            {
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(6, 90),
                Size = new Size(262, 17),
                TabIndex = 3,
                Text = @"Save current game on exit and resume on start",
                UseVisualStyleBackColor = false
            };

            _chkHighlight = new CheckBox
            {
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(6, 113),
                Size = new Size(205, 17),
                TabIndex = 4,
                Text = @"Show card highlight when dragging",
                UseVisualStyleBackColor = false
            };

            gbGeneral.Controls.AddRange(new Control[]
            {
                _chkAutoTurn, _chkProgress, _chkTips, _chkSave, _chkHighlight
            });

            var gbSound = new GroupBox
            {
                BackColor = Color.Transparent,
                Location = new Point(12, 159),
                Size = new Size(274, 78),
                TabStop = false,
                Text = @"Sound:"
            };

            _chkEffects = new CheckBox
            {
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(6, 21),
                Size = new Size(118, 17),
                TabIndex = 5,
                Text = @"Play sound effects",
                UseVisualStyleBackColor = false
            };

            var lblFxVol = new Label
            {
                AutoSize = true,
                Location = new Point(3, 46),
                Size = new Size(48, 13),
                Text = @"Volume:"
            };

            _tbEffects = new TrackBarEx
            {
                Location = new Point(50, 44),
                Maximum = 100,
                Size = new Size(76, 45),
                TabIndex = 6,
                Tag = "EFFECTS",
                TickStyle = TickStyle.None
            };

            var chkMusic = new CheckBox
            {
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(148, 21),
                Size = new Size(79, 17),
                TabIndex = 7,
                Text = @"Play music",
                UseVisualStyleBackColor = false
            };

            var lblMusicVol = new Label
            {
                AutoSize = true,
                Location = new Point(145, 46),
                Size = new Size(48, 13),
                Text = @"Volume:"
            };

            _tbMusic = new TrackBarEx
            {
                Location = new Point(192, 44),
                Maximum = 100,
                Size = new Size(76, 45),
                TabIndex = 8,
                Tag = "MUSIC",
                TickStyle = TickStyle.None
            };

            gbSound.Controls.AddRange(new Control[]
            {
                _chkEffects, lblFxVol, _tbEffects, chkMusic, lblMusicVol, _tbMusic
            });

            var gbConfirm = new GroupBox
            {
                BackColor = Color.Transparent,
                Location = new Point(12, 243),
                Size = new Size(274, 73),
                TabStop = false,
                Text = @"Confirm:"
            };

            _chkNew = new CheckBox
            {
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(6, 21),
                Size = new Size(258, 17),
                TabIndex = 9,
                Text = @"When starting a new game or loading a game",
                UseVisualStyleBackColor = false
            };

            _chkExit = new CheckBox
            {
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(6, 44),
                Size = new Size(143, 17),
                TabIndex = 10,
                Text = @"Closing the application",
                UseVisualStyleBackColor = false
            };

            gbConfirm.Controls.AddRange(new Control[]
            {
                _chkNew, _chkExit
            });

            var btnOk = new Button
            {
                DialogResult = DialogResult.OK,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0),
                Location = new Point(176, 334),
                Size = new Size(110, 28),
                TabIndex = 11,
                Text = @"Ok",
                UseVisualStyleBackColor = false,
                BackgroundImage = Resources.button_ok,
                BackgroundImageLayout = ImageLayout.Tile,
                ForeColor = Color.White
            };

            Controls.AddRange(new Control[]
            {
                gbGeneral, gbSound, gbConfirm, btnOk
            });

            chkMusic.CheckedChanged += MusicCheckChanged;

            _tbEffects.ValueChanged += TrackBarValueChanged;
            _tbMusic.ValueChanged += TrackBarValueChanged;

            _chkAutoTurn.Checked = SettingsManager.Settings.Options.AutoTurn;
            _chkProgress.Checked = SettingsManager.Settings.Options.ShowProgress;
            _chkTips.Checked = SettingsManager.Settings.Options.ShowTips;
            _chkSave.Checked = SettingsManager.Settings.Options.SaveRecover;
            _chkHighlight.Checked = SettingsManager.Settings.Options.ShowHighlight;

            _chkEffects.Checked = SettingsManager.Settings.Options.Sound.EnableEffects;
            _tbEffects.Value = SettingsManager.Settings.Options.Sound.EffectsVolume;

            chkMusic.Checked = SettingsManager.Settings.Options.Sound.EnableMusic;
            _tbMusic.Value = SettingsManager.Settings.Options.Sound.MusicVolume;

            _chkExit.Checked = SettingsManager.Settings.Options.Confirm.OnExit;
            _chkNew.Checked = SettingsManager.Settings.Options.Confirm.OnNewLoad;

            AcceptButton = btnOk;
        }

        protected override void OnLoad(EventArgs e)
        {
            _init = false;
            base.OnLoad(e);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SettingsManager.Settings.Options.AutoTurn = _chkAutoTurn.Checked;
            SettingsManager.Settings.Options.ShowProgress = _chkProgress.Checked;
            SettingsManager.Settings.Options.ShowTips = _chkTips.Checked;
            SettingsManager.Settings.Options.SaveRecover = _chkSave.Checked;
            SettingsManager.Settings.Options.ShowHighlight = _chkHighlight.Checked;

            SettingsManager.Settings.Options.Sound.EnableEffects = _chkEffects.Checked;
            SettingsManager.Settings.Options.Sound.EffectsVolume = _tbEffects.Value;
            SettingsManager.Settings.Options.Sound.MusicVolume = _tbMusic.Value;

            SettingsManager.Settings.Options.Confirm.OnExit = _chkExit.Checked;
            SettingsManager.Settings.Options.Confirm.OnNewLoad = _chkNew.Checked;
            base.OnFormClosing(e);
        }

        private void MusicCheckChanged(object sender, EventArgs e)
        {
            if (_init)
            {
                return;
            }
            var o = (CheckBox) sender;
            SettingsManager.Settings.Options.Sound.EnableMusic = o.Checked;
            if (o.Checked)
            {
                AudioManager.PlayMusic(true);
            }
            else
            {
                AudioManager.StopMusic();
            }
        }

        private void TrackBarValueChanged(object sender, EventArgs e)
        {
            var o = (TrackBarEx) sender;
            switch (o.Tag.ToString())
            {
                case "EFFECTS":
                    AudioManager.SetEffectsVolume(o.Value);
                    break;

                case "MUSIC":
                    AudioManager.SetMusicVolume(o.Value);
                    break;
            }
        }
    }
}
