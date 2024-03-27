using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace NGettext.Wpf
{
    /// <summary>
    /// 
    /// </summary>
    public class CultureEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="culture"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public CultureEventArgs([NotNull] CultureInfo culture)
        {
            ArgumentNullException.ThrowIfNull(culture);
            Culture = culture;
        }

        /// <summary>
        /// 
        /// </summary>
        public CultureInfo Culture { get; }
    }
}