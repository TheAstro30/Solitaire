/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System.Reflection;
using System.Windows.Forms;
using Solitaire.Classes.UI;
using Solitaire.Properties;

namespace Solitaire.Forms
{
    /* Show info about the game - Captain James T. Kirk, of the Starship Enterprise... */
    public partial class FrmAbout : FormEx
    {
        public FrmAbout()
        {
            /* I'm afraid, Captain, that's just illogical */
            InitializeComponent();

            pnlIcon.BackgroundImageLayout = ImageLayout.None;
            pnlIcon.BackgroundImage = Resources.aboutIcon.ToBitmap();

            var version = Assembly.GetExecutingAssembly().GetName().Version;
            lblVersion.Text = string.Format("Version: {0}.{1}.{2} (Build: {3})", version.Major, version.Minor, version.Build, version.MinorRevision);
        }
    }
}
