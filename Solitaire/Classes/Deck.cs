/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Collections;
using System.Collections.Generic;
using Solitaire.Classes.Helpers;

namespace Solitaire.Classes
{
    [Serializable]
    public sealed class Deck : IList<Card>
    {
        /* Penis */
        private readonly List<Card> _cards = new List<Card>();

        public Deck()
        {
            /* Default empty constructor */
        }

        public Deck(IEnumerable<Card> deck)
        {
            _cards.AddRange(new List<Card>(deck));
        }

        public void Add(Card item)
        {
            _cards.Add(item);
        }

        public void AddRange(IList<Card> cards)
        {
            _cards.AddRange(cards);
        }

        public void Clear()
        {
            _cards.Clear();
        }

        public bool Contains(Card item)
        {
            return _cards.Contains(item);
        }

        public void CopyTo(Card[] array, int arrayIndex)
        {
            _cards.CopyTo(array, arrayIndex);
        }

        public bool Remove(Card item)
        {
            return _cards.Remove(item);
        }

        public int Count
        {
            get { return _cards.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public int IndexOf(Card item)
        {
            return _cards.IndexOf(item);
        }

        public void Insert(int index, Card item)
        {
            _cards.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _cards.RemoveAt(index);
        }

        public Card this[int index]
        {
            get { return _cards[index]; }
            set { _cards[index] = value; }
        }

        public IEnumerator<Card> GetEnumerator()
        {
            return _cards.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Shuffle()
        {
            _cards.Shuffle();
        }
    }
}
