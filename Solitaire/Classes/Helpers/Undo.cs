/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */

using System;
using System.Collections.Generic;
using Solitaire.Classes.Data;

namespace Solitaire.Classes.Helpers
{
    public static class Undo
    {
        private static readonly Stack<GameData> Data = new Stack<GameData>();

        public static int Count { get { return Data.Count; } }

        public static void AddMove(GameData data)
        {
            /* Add a move to undo history */
            var d = new GameData(data);
            Data.Push(d);
        }

        public static void RemoveLastEntry()
        {
            Data.Pop();
        }

        public static GameData UndoLastMove()
        {
            return Data.Count == 0 ? null : Data.Pop();
        }

        public static GameData GetRestartPoint()
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
