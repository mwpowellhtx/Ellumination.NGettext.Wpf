using System.ComponentModel;
using System.Windows;

namespace NGettext
{
    /// <summary>
    /// 
    /// </summary>
    internal static class DesignerExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dependencyObject"></param>
        /// <returns></returns>
        public static bool IsInDesignMode(this DependencyObject dependencyObject)
            => DesignerProperties.GetIsInDesignMode(dependencyObject);
    }
}
