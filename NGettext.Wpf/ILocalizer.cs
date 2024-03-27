using System.Globalization;

namespace NGettext.Wpf
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILocalizer
    {
        /// <summary>
        /// 
        /// </summary>
        ICultureTracker CultureTracker { get; }

        /// <summary>
        /// 
        /// </summary>
        ICatalog Catalog { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        ICatalog CreateCatalog(CultureInfo cultureInfo);
    }
}