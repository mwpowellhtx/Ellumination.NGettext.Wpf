using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows.Data;

namespace NGettext.Wpf.Common
{
    // TODO: what is this here for? converting 'to'? then 'back' to what again, if possible/necessary?
    /// <inheritdoc/>
    public class GettextStringFormatConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        public string MsgId { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgId"></param>
#pragma warning disable IDE0290 // Use primary constructor
        public GettextStringFormatConverter([NotNull] string msgId)
        {
            MsgId = msgId;
        }

        // TODO: consider single responsibility
        // TODO: also re: nullable, getting in the way more than it is helping
        /// <summary>
        /// 
        /// </summary>
        public static ILocalizer Localizer { get; set; }

        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => Localizer.Gettext(MsgId, value);

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}