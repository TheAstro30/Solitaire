/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Windows.Forms;

namespace Solitaire.Forms
{
    public partial class FrmSplash : Form
    {
        private int _count;
        private readonly Timer _timer;

        public FrmSplash()
        {
            InitializeComponent();

            _timer = new Timer
            {
                Interval = 1000,
                Enabled = true
            };
            _timer.Tick += OnTimer;
        }

        private void OnTimer(object sender, EventArgs e)
        {
            //_count++;
            //if (_count > 5)
            //{
            //    Hide();
            //    _timer.Enabled = false;
            //    var game = new FrmGame();
                
            //        game.ShowDialog(this);
                
            //}
        }
    }
}
