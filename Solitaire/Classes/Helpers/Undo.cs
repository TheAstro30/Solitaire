/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System.Collections.Generic;
using Solitaire.Classes.Data;

namespace Solitaire.Classes.Helpers
{
    public static class Undo
    {
        private static readonly Stack<UndoData> Data = new Stack<UndoData>();

        public static void AddMove(GameData data)
        {
            /* Add a move to undo history - seems a stupid way to do this, but seems to be better than relying on IClonable
             * (which I couldn't get to work) */
            var d = new UndoData();
            /* Main deck */
            foreach (var c in data.GameDeck)
            {
                d.GameDeck.Add(new Card(c));
            }
            /* Dealt cards */
            foreach (var c in data.DealtCards)
            {
                d.DealtCards.Add(new Card(c));
            }
            /* Home stacks */
            foreach (var s in data.HomeStacks)
            {
                d.HomeStacks.Add(new StackData(s));
            }
            /* Playing stacks */
            foreach (var s in data.PlayingStacks)
            {
                d.PlayingStacks.Add(new StackData(s));
            }
            Data.Push(d);
        }

        public static void RemoveLastEntry()
        {
            Data.Pop();
        }

        public static UndoData UndoLastMove()
        {
            return Data.Count == 0 ? null : Data.Pop();
        }

        public static UndoData GetRestartPoint()
        {
            /* Not the most elegant way to do it... */
            for (var i = Data.Count - 1; i >= 0; i--)
            {
                var d = Data.Pop();
                if (i == 0)
                {
                    return d;
                }
            }
            return null;
        }

        public static void Clear()
        {
            Data.Clear();
        }
    }
}
