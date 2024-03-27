namespace NGettext.Wpf
{
    /// <summary>
    /// 
    /// </summary>
    public interface IWeakCultureObserver
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        void ChangeCulture(ICultureTracker sender, CultureEventArgs eventArgs);
    }
}