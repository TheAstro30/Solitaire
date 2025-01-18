/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Collections.Generic;
using System.Linq;

namespace Solitaire.Classes.Data
{
    public class MultiKeyDictionary<TKey1, TKey2, TValue> : Dictionary<TKey1, Dictionary<TKey2, TValue>>
    {
        /* Multi-key Dictionary by Herman Schoenfeld */
        public TValue this[TKey1 key1, TKey2 key2]
        {
            get
            {
                if (!ContainsKey(key1) || !this[key1].ContainsKey(key2))
                {
                    throw new ArgumentOutOfRangeException();
                }
                return base[key1][key2];
            }
            set
            {
                if (!ContainsKey(key1))
                {
                    this[key1] = new Dictionary<TKey2, TValue>();
                }
                this[key1][key2] = value;
            }
        }

        public void Add(TKey1 key1, TKey2 key2, TValue value)
        {
            if (!ContainsKey(key1))
            {
                this[key1] = new Dictionary<TKey2, TValue>();
            }
            this[key1][key2] = value;
        }

        public bool ContainsKey(TKey1 key1, TKey2 key2)
        {
            return ContainsKey(key1) && this[key1].ContainsKey(key2);
        }

        public new IEnumerable<TValue> Values
        {
            get
            {
                return from baseDict in base.Values
                       from baseKey in baseDict.Keys
                       select baseDict[baseKey];
            }
        }
    }
}
