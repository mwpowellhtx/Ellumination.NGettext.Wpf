using System;
using System.Globalization;

namespace NGettext.Wpf
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICultureTracker
    {
        /// <summary>
        /// 
        /// </summary>
        [Obsolete("Use AddWeakCultureObserver() instead.  Otherwise the culture tracker (which is probably a singleton) will keep your object alive for longer than it needs to.   This method will be removed in 2.x")]
        event EventHandler<CultureEventArgs> CultureChanged;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<CultureEventArgs> CultureChanging;

        /// <summary>
        /// 
        /// </summary>
        CultureInfo CurrentCulture { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="weakCultureObserver"></param>
        void AddWeakCultureObserver(IWeakCultureObserver weakCultureObserver);
    }
}