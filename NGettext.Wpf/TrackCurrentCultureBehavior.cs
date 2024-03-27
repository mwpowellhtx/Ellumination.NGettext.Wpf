using System.Windows;
using System.Windows.Markup;
using Microsoft.Xaml.Behaviors;

namespace NGettext.Wpf
{
    /// <summary>
    /// Makes sure that the CultureInfo used for all binding operations inside the associated
    /// FrameworkElement follows the CurrentCulture of the CultureTracker injected to the static
    /// CultureTracker property.
    ///
    /// For instance, dates and numbers bound with a culture specific StringFormat will be formatted
    /// according to the tracked culture and even reformatted on culture changed.
    /// </summary>
    public class TrackCurrentCultureBehavior : Behavior<FrameworkElement>, IWeakCultureObserver
    {
        /// <summary>
        /// 
        /// </summary>
        public static ICultureTracker CultureTracker { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnAttached()
        {
            if (!AssociatedObject.IsInDesignMode())
            {
                if (CultureTracker is null)
                {
                    CompositionRoot.WriteMissingInitializationErrorMessage();
                    return;
                }
                CultureTracker.AddWeakCultureObserver(this);
                UpdateAssociatedObjectCulture();
            }

            base.OnAttached();
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateAssociatedObjectCulture()
        {
            if (AssociatedObject is not null)
            {
                var language = XmlLanguage.GetLanguage(CultureTracker?.CurrentCulture.IetfLanguageTag);
                AssociatedObject.Language = language;
            }
        }

        // TODO: "handle" culture changed? "on culture changed" (?)
        // TODO: what about a proper event EH, or even community toolkit messaging receipient (?)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        public void ChangeCulture(ICultureTracker sender, CultureEventArgs eventArgs)
            => UpdateAssociatedObjectCulture();
    }
}