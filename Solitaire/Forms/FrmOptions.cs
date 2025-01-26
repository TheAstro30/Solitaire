/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Drawing;
using System.Windows.Forms;
using Solitaire.Classes.Helpers.Management;
using Solitaire.Classes.UI;
using Solitaire.Controls;
using Solitaire.Controls.TrackBar;

namespace Solitaire.Forms
{
    public partial class FrmOptions : FormEx
    {
        private bool _init;

        public FrmOptions(Game ctl)
        {
            _init = true;
            InitializeComponent();

            btnOk.BackgroundImage = ctl.ObjectData.ButtonOk;
            btnOk.BackgroundImageLayout = ImageLayout.Tile;
            btnOk.BackColor = Color.White;

            chkMusic.CheckedChanged += MusicCheckChanged;

            tbEffects.ValueChanged += TrackBarValueChanged;
            tbMusic.ValueChanged += TrackBarValueChanged;

            chkAutoTurn.Checked = SettingsManager.Settings.Options.AutoTurn;
            chkProgress.Checked = SettingsManager.Settings.Options.ShowProgress;
            chkTips.Checked = SettingsManager.Settings.Options.ShowTips;
            chkSave.Checked = SettingsManager.Settings.Options.SaveRecover;
            chkHighlight.Checked = SettingsManager.Settings.Options.ShowHighlight;

            chkEffects.Checked = SettingsManager.Settings.Options.Sound.EnableEffects;
            tbEffects.Value = SettingsManager.Settings.Options.Sound.EffectsVolume;

            chkMusic.Checked = SettingsManager.Settings.Options.Sound.EnableMusic;
            tbMusic.Value = SettingsManager.Settings.Options.Sound.MusicVolume;

            chkExit.Checked = SettingsManager.Settings.Options.Confirm.OnExit;
            chkNew.Checked = SettingsManager.Settings.Options.Confirm.OnNewLoad;
        }

        protected override void OnLoad(EventArgs e)
        {
            _init = false;
            base.OnLoad(e);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SettingsManager.Settings.Options.AutoTurn = chkAutoTurn.Checked;
            SettingsManager.Settings.Options.ShowProgress = chkProgress.Checked;
            SettingsManager.Settings.Options.ShowTips = chkTips.Checked;
            SettingsManager.Settings.Options.SaveRecover = chkSave.Checked;
            SettingsManager.Settings.Options.ShowHighlight = chkHighlight.Checked;

            SettingsManager.Settings.Options.Sound.EnableEffects = chkEffects.Checked;
            SettingsManager.Settings.Options.Sound.EffectsVolume = tbEffects.Value;
            SettingsManager.Settings.Options.Sound.MusicVolume = tbMusic.Value;

            SettingsManager.Settings.Options.Confirm.OnExit = chkExit.Checked;
            SettingsManager.Settings.Options.Confirm.OnNewLoad = chkNew.Checked;
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
