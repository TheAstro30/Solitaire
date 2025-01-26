﻿/* Object List View
 * Copyright (C) 2006-2012 Phillip Piper
 * Refactored by Jason James Newland - 2014/January 2025; C# v7.0
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 *
 * If you wish to use this code in a closed source application, please contact phillip_piper@bigfoot.com.
 */
using System.Collections.Generic;
using System.Drawing;

namespace libolv.Filtering.TextMatch
{
    internal class TextBeginsMatchingStrategy : TextMatchingStrategy
    {
        public TextBeginsMatchingStrategy(TextMatchFilter filter, string text)
        {
            TextFilter = filter;
            Text = text;
        }

        public override bool MatchesText(string cellText)
        {
            return cellText.StartsWith(Text, StringComparison);
        }

        public override IEnumerable<CharacterRange> FindAllMatchedRanges(string cellText)
        {
            var ranges = new List<CharacterRange>();
            if (cellText.StartsWith(Text, StringComparison))
            {
                ranges.Add(new CharacterRange(0, Text.Length));
            }
            return ranges;
        }

    }
}
