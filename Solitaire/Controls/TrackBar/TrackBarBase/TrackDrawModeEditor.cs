/* xsMedia - xsTrackBar
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Solitaire.Controls.TrackBar.TrackBarBase
{
    [PermissionSet(SecurityAction.LinkDemand, Unrestricted=true), PermissionSet(SecurityAction.InheritanceDemand, Unrestricted=true)]
    public class TrackDrawModeEditor : UITypeEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            var parts = TrackBarOwnerDrawParts.None;
            IEnumerator enumerator = null;
            if (!(value is TrackBarOwnerDrawParts))
            {
                if (value != null)
                {
                    return value;
                }
            }
            if (provider == null)
            {
                return parts;
            }
            var service = (IWindowsFormsEditorService) provider.GetService(typeof(IWindowsFormsEditorService));
            if (service == null)
            {
                if (value != null)
                {
                    return value;
                }
            }
            var control = new CheckedListBox
            {
                BorderStyle = BorderStyle.None,
                CheckOnClick = true
            };
            control.Items.Add("Ticks", context != null && (((TrackBarEx)context.Instance).OwnerDrawParts & TrackBarOwnerDrawParts.Ticks) == TrackBarOwnerDrawParts.Ticks);
            control.Items.Add("Thumb", context != null && (((TrackBarEx)context.Instance).OwnerDrawParts & TrackBarOwnerDrawParts.Thumb) == TrackBarOwnerDrawParts.Thumb);
            control.Items.Add("Channel", context != null && (((TrackBarEx)context.Instance).OwnerDrawParts & TrackBarOwnerDrawParts.Channel) == TrackBarOwnerDrawParts.Channel);
            service?.DropDownControl(control);
            try
            {
                enumerator = control.CheckedItems.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var objectValue = RuntimeHelpers.GetObjectValue(enumerator.Current);
                    parts |= (TrackBarOwnerDrawParts) Enum.Parse(typeof(TrackBarOwnerDrawParts), objectValue.ToString());
                }
            }
            finally
            {
                if (enumerator is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
            control.Dispose();
            service?.CloseDropDown();

            return parts;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }
    }
}

