using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;

namespace NGettext.Wpf
{
    /// <inheritdoc/>
    [MarkupExtensionReturnType(typeof(string))]
    public class GettextExtension : MarkupExtension, IWeakCultureObserver
    {
        /// <summary>
        /// 
        /// </summary>
        [ConstructorArgument("msgId")]
        public string MsgId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ConstructorArgument("args")]
        public object[] Arguments { get; set; } = [];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgId"></param>
        public GettextExtension(string msgId)
            : this(msgId, [])
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgId"></param>
        /// <param name="args"></param>
#pragma warning disable IDE0290 // Use primary constructor
        public GettextExtension(string msgId, params object[] args)
        {
            MsgId = msgId;
            Arguments = args;
        }

        /// <summary>
        /// 
        /// </summary>
        public static ILocalizer Localizer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private DependencyObject _dependencyObject;

        /// <summary>
        /// 
        /// </summary>
        private DependencyProperty _dependencyProperty;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget pvt)
            {
                if (pvt.TargetObject is DependencyObject dependencyObject)
                {
                    _dependencyObject = dependencyObject;

                    if (DesignerProperties.GetIsInDesignMode(_dependencyObject))
                    {
                        return Gettext();
                    }

                    AttachToCultureChangedEvent();

                    _dependencyProperty = (DependencyProperty)pvt.TargetProperty;

                    KeepGettextExtensionAliveForAsLongAsDependencyObject();
                }
                else
                {
                    Console.WriteLine("NGettext.Wpf: Target object of type {0} is not yet implemented", pvt.TargetObject?.GetType());
                }
            }

            return Gettext();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string Gettext()
            => Arguments.Length > 0 ? Localizer?.Gettext(MsgId, Arguments) : Localizer?.Gettext(MsgId);

        /// <summary>
        /// 
        /// </summary>
        private void KeepGettextExtensionAliveForAsLongAsDependencyObject()
            => SetGettextExtension(_dependencyObject, this);

        /// <summary>
        /// 
        /// </summary>
        private void AttachToCultureChangedEvent()
        {
            var localizer = Localizer;

            // TODO: again write to console (error?) or throw exception?
            if (localizer is null)
            {
                Console.Error.WriteLine("NGettext.WPF.GettextExtension.Localizer not set. Localization is disabled.");
                return;
            }

            // TODO: can we name 'culture tracker' better? because naming is hard...
            var localizer_CultureTracker = Localizer.CultureTracker;

            localizer_CultureTracker.AddWeakCultureObserver(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ChangeCulture(ICultureTracker sender, CultureEventArgs e)
            => _dependencyObject?.SetValue(_dependencyProperty, Gettext());

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty GettextExtensionProperty = DependencyProperty.RegisterAttached(
            nameof(GettextExtension)
            , typeof(GettextExtension)
            , typeof(GettextExtension)
            , new PropertyMetadata(default(GettextExtension))
        );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetGettextExtension(DependencyObject element, GettextExtension value)
            => element?.SetValue(GettextExtensionProperty, value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static GettextExtension GetGettextExtension(DependencyObject element)
            => (GettextExtension)element.GetValue(GettextExtensionProperty);
    }
}