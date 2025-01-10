/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System.Windows.Forms;
using Solitaire.Classes.Helpers;
using Solitaire.Classes.UI;
using Solitaire.Properties;

namespace Solitaire.Forms
{
    public partial class FrmStatistics : FormEx
    {
        public FrmStatistics()
        {
            InitializeComponent();

            pnlIcon.BackgroundImage = Resources.aboutIcon.ToBitmap();
            pnlIcon.BackgroundImageLayout = ImageLayout.None;

            lblStats.Text = SettingsManager.Settings.Statistics.ToString();
        }
    }
}
