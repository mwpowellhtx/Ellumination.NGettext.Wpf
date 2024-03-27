using System;

namespace NGettext.Wpf
{
    using Common;
    using EnumLocalization;

    // TODO: 'composition root' is going to be a problem
    // TODO: need to investigate the gains for 'domain' if it does what I think it might be doing
    // TODO: but then there is the additional dependencies installing for different aspects
    // TODO: i.e. ME, 'enum', 'localization' in general, 'string formatters', etc

    // TODO: couple that with what is 'culture tracker' doing
    // TODO: underpinning that, I think, is the current thread current culture (with or without UI)
    // TODO: this is the key, how do we reliably track hold of 'that' issue

    // TODO: after that, it does seem at somewhat desirable prima facie on its face that we raise a 'weak' event (or send weak message)
    /// <summary>
    /// 
    /// </summary>
    public static class CompositionRoot
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainName"></param>
        /// <param name="dependencyResolver"></param>
        public static void Compose(string domainName, NGettextWpfDependencyResolver dependencyResolver = null)
        {
            dependencyResolver ??= new NGettextWpfDependencyResolver();

            var cultureTracker = dependencyResolver.ResolveCultureTracker();
            var localizer = new Localizer(cultureTracker, domainName);

            // TODO: lot of tracker+localizer instances going on here...
            // TODO: could that be single responsibility reduced at all (?)
            ChangeCultureCommand.CultureTracker = cultureTracker;
            TrackCurrentCultureBehavior.CultureTracker = cultureTracker;

            GettextExtension.Localizer = localizer;
            LocalizeEnumConverter.EnumLocalizer = new EnumLocalizer(localizer);
            Localization.Localizer = localizer;
            GettextStringFormatConverter.Localizer = localizer;
        }

        /// <summary>
        /// 
        /// </summary>
        internal static void WriteMissingInitializationErrorMessage()
        {
            Console.Error.WriteLine("NGettext.Wpf: NGettext.Wpf.CompositionRoot.Compose() must be called at the entry point of the application for localization to work");
        }
    }
}