using System;
using System.Globalization;
using System.Linq;
using System.Windows.Input;

namespace NGettext.Wpf
{
    /// <inheritdoc/>
    public class ChangeCultureCommand : ICommand
    {
        /// <inheritdoc/>
        public virtual bool CanExecute(object parameter)
            => parameter is string s
                && CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                    .Any(cultureInfo => cultureInfo.Name == s);

        /// <inheritdoc/>
        public virtual void Execute(object parameter)
        {
            // TODO: base this around the 'behavior' for best single responsibility
            // TODO: goal also being, I think, to obviate 'CompositionRoot' being necessary
            if (CultureTracker is null)
            {
                CompositionRoot.WriteMissingInitializationErrorMessage();
                return;
            }

            if (parameter is string s)
            {
                CultureTracker.CurrentCulture = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                    .Single(cultureInfo => cultureInfo.Name == s);
            }
        }

        /// <summary>
        /// 
        /// </summary>
#pragma warning disable CS0067 // The event 'ChangeCultureCommand.CanExecuteChanged' is never used
        public event EventHandler CanExecuteChanged;

        // TODO: can this be reduced, single responsibility (?)
        /// <summary>
        /// 
        /// </summary>
        public static ICultureTracker CultureTracker { get; set; }
    }
}