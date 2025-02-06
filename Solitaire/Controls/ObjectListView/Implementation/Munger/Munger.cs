/* Object List View
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
using System.Diagnostics;
using System.Linq;

namespace Solitaire.Controls.ObjectListView.Implementation.Munger
{
    public class Munger
    {
        private string _aspectName;
        private IList<SimpleMunger> _aspectParts;

        static Munger()
        {
            if (Debugger.IsAttached)
            {
                IgnoreMissingAspects = true;
            }
        }

        public Munger()
        {
            /* Empty */
        }

        public Munger(string aspectName)
        {
            AspectName = aspectName;
        }

        /* Static utility methods */
        public static bool PutProperty(object target, string propertyName, object value)
        {
            try
            {
                var munger = new Munger(propertyName);
                return munger.PutValue(target, value);
            }
            catch (MungerException)
            {                
                /* Not a lot we can do about  Something went wrong in the bowels
                 * of the property. Let's take the ostrich approach and just ignore it :-)
                 * Normally, we would never just silently ignore an exception.
                 * However, in this case, this is a utility method that explicitly 
                 * contracts to catch and ignore errors. If this is not acceptible,
                 * the programmer should not use this method. */
            }
            return false;
        }

        public static bool IgnoreMissingAspects { get; set; }

        /* Public properties */
        public string AspectName
        {
            get => _aspectName;
            set
            {
                _aspectName = value;
                /* Clear any cache */
                _aspectParts = null;
            }
        }

        /* Public interface */
        public object GetValue(object target)
        {
            if (Parts.Count == 0)
            {
                return null;
            }
            try
            {
                return EvaluateParts(target, Parts);
            }
            catch (MungerException ex)
            {
                return IgnoreMissingAspects ? null : $"'{ex.Munger.AspectName}' is not a parameter-less method, property or field of type '{ex.Target.GetType()}'";
            }
        }
        
        public object GetValueEx(object target)
        {
            if (Parts.Count == 0)
            {
                return null;
            }
            return EvaluateParts(target, Parts);
        }

        public bool PutValue(object target, object value)
        {
            if (Parts.Count == 0)
            {
                return false;
            }
            var lastPart = Parts[Parts.Count - 1];
            if (Parts.Count > 1)
            {
                var parts = new List<SimpleMunger>(Parts);
                parts.RemoveAt(parts.Count - 1);
                try
                {
                    target = EvaluateParts(target, parts);
                }
                catch (MungerException ex)
                {
                    ReportPutValueException(ex);
                    return false;
                }
            }
            if (target == null)
            {
                return false;
            }
            {
                try
                {
                    return lastPart.PutValue(target, value);
                }
                catch (MungerException ex)
                {
                    ReportPutValueException(ex);
                }
            }
            return false;
        }

        /* Implementation */
        private IList<SimpleMunger> Parts => _aspectParts ?? (_aspectParts = BuildParts(AspectName));

        private static IList<SimpleMunger> BuildParts(string aspect)
        {
            var parts = new List<SimpleMunger>();
            if (!string.IsNullOrEmpty(aspect))
            {
                parts.AddRange(aspect.Split('.').Select(part => new SimpleMunger(part.Trim())));
            }
            return parts;
        }

        private static object EvaluateParts(object target, IEnumerable<SimpleMunger> parts)
        {
            var t = target;
            return parts.TakeWhile(part => t != null).Aggregate(target, (current, part) => part.GetValue(current));
        }

        private static void ReportPutValueException(MungerException ex)
        {
            //TODO: How should we report this error?
            Debug.WriteLine("PutValue failed");
            Debug.WriteLine($"- Culprit aspect: {ex.Munger.AspectName}");
            Debug.WriteLine($"- Target: {ex.Target} of type {ex.Target.GetType()}");
            Debug.WriteLine($"- Inner exception: {ex.InnerException}");
        }
    }
}
