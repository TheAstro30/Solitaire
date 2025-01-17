/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Solitaire.Classes.Data;

namespace Solitaire.Classes.UI
{
    public sealed class TipProvider
    {
        /* Simple class to provide visual handy tips in the statusbar */
        private readonly List<string> _tips;
        private int _tipIndex;

        public Action<string> OnTipChange;

        public TipProvider()
        {
            /* Build tips list */
            _tips = new List<string>();
            _tips.AddRange(new[]
            {
                "Right-click anywhere to automatically complete cards to home cells.",
                "Press the Escape key to send the game to the system tray.",
                "Stuck? Press Ctrl+H to get a hint.",
                "You can save the current game to be recalled later.",
                "Challenge yourself further by playing in Draw Three mode.",
                "Build tableau columns from King to Ace in alternating suits.",
                "To complete the game, move all cards of same suit from Ace to King to home cells.",
                "Take your time: Avoid rushing and consider all possible moves and their consequences.",
                "You can use the undo feature (Ctrl+Z) to correct mistakes.",
                "Build foundation piles evenly: Building piles too quickly or unevenly can make it difficult to work the tableau.",
                "Create empty columns: Move cards out of a tableau column to create space to place Kings.",
                "Prioritize moves that reveal facedown cards: Move a card to flip over a facedown card so you can play it.",
                "Assess the tableau: Look at the tableau to see what moves make the most sense.",
                "Move aces: Move aces and twos to home cells to see how to move the other cards.",
                "Play regularly: Playing solitaire regularly can help improve your skills.",
                "Avoid emptying a slot without a King: These spaces will remain empty as only Kings go there."
            });
            _tips.Shuffle();
            /* Timer */
            var timerTip = new Timer
            {
                Interval = 10000,
                Enabled = true
            };
            timerTip.Tick += OnTimerTip;
            OnTimerTip(timerTip, new EventArgs());
        }

        /* Timer callback */
        private void OnTimerTip(object sender, EventArgs e)
        {
            _tipIndex++;
            if (_tipIndex > _tips.Count - 1)
            {
                _tipIndex = 0;
                _tips.Shuffle();
            }
            if (OnTipChange == null)
            {
                return;
            }
            OnTipChange(_tips[_tipIndex]);
        }
    }
}
