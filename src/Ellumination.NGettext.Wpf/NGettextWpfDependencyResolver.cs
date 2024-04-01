// TODO: again re: 'composition roots', "DI" at this level seems a bit like overkill to me
namespace NGettext.Wpf
{
    /// <summary>
    /// 
    /// </summary>
    public class NGettextWpfDependencyResolver
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual ICultureTracker ResolveCultureTracker() => new CultureTracker();
    }
}