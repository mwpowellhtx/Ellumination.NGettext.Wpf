using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows.Data;

namespace NGettext.Wpf.EnumLocalization
{
    // TODO: ditto 'converter' and which would obviate the need we think for 'converter'
    // TODO: i.e. converting to/back again anything, when we have things adequately stitched together by a 1C view model
    // TODO: be sure we are wrapping the Enum Value, and for comboboxes, best results usually happen based on the Indexing to author provided items source
    /// <inheritdoc/>
    public class LocalizeEnumConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IEnumLocalizer _enumLocalizer;

        /// <summary>
        /// 
        /// </summary>
        public LocalizeEnumConverter()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumLocalizer"></param>
        public LocalizeEnumConverter([NotNull] IEnumLocalizer enumLocalizer)
        {
            _enumLocalizer = enumLocalizer;
        }

        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumLocalizer = GetEnumLocalizer();
            if (enumLocalizer is null)
            {
                return value;
            }

            if (value is Enum enumValue)
            {
                return enumLocalizer.LocalizeEnum(enumValue);
            }

            return null;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();

        /// <summary>
        /// 
        /// </summary>
        public static IEnumLocalizer EnumLocalizer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IEnumLocalizer GetEnumLocalizer()
        {
            var result = _enumLocalizer ?? EnumLocalizer;

            // TODO: let's see if we could not do this part smarter
            // TODO: missing or just return with a default default?
            if (result is null)
            {
                // TODO: write a console error message? or actually throw an exception?
                CompositionRoot.WriteMissingInitializationErrorMessage();
            }

            return result;
        }
    }
}