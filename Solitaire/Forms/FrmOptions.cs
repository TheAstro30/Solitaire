/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System.Windows.Forms;
using Solitaire.Classes.Helpers.Management;
using Solitaire.Classes.UI;

namespace Solitaire.Forms
{
    public partial class FrmOptions : FormEx
    {
        public FrmOptions(Game ctl)
        {
            InitializeComponent();

            btnOk.BackgroundImage = ctl.ObjectData.ButtonOk;
            btnOk.BackgroundImageLayout = ImageLayout.Tile;

            chkSound.Checked = SettingsManager.Settings.Options.PlaySounds;
            chkSave.Checked = SettingsManager.Settings.Options.SaveRecover;
            chkProgress.Checked = SettingsManager.Settings.Options.ShowProgress;
            chkExit.Checked = SettingsManager.Settings.Options.Confirm.OnExit;
            chkNew.Checked = SettingsManager.Settings.Options.Confirm.OnNewLoad;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SettingsManager.Settings.Options.PlaySounds = chkSound.Checked;
            SettingsManager.Settings.Options.SaveRecover = chkSave.Checked;
            SettingsManager.Settings.Options.ShowProgress = chkProgress.Checked;
            SettingsManager.Settings.Options.Confirm.OnExit = chkExit.Checked;
            SettingsManager.Settings.Options.Confirm.OnNewLoad = chkNew.Checked;
            base.OnFormClosing(e);
        }
    }
}
