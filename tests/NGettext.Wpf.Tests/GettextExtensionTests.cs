using System;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;

namespace NGettext.Wpf
{
    using NSubstitute;
    using Xunit;

    /// <summary>
    /// 
    /// </summary>
    public class GettextExtensionTests
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 
        /// </summary>
        private readonly TestClass _valueTarget = new();

        /// <summary>
        /// 
        /// </summary>
        public class TestClass : FrameworkElement
        {
            /// <summary>
            /// 
            /// </summary>
            public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
                nameof(Text), typeof(string), typeof(TestClass), new PropertyMetadata(default(string)));

            /// <summary>
            /// 
            /// </summary>
            public string Text
            {
                get => (string)GetValue(TextProperty);
                set => SetValue(TextProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public GettextExtensionTests()
        {
            _serviceProvider = Substitute.For<IServiceProvider>();
            var provideValueTarget = Substitute.For<IProvideValueTarget>();
            provideValueTarget.TargetObject
                .Returns(_valueTarget);
            provideValueTarget.TargetProperty
                .Returns(TestClass.TextProperty);
            _serviceProvider.GetService(Arg.Is(typeof(IProvideValueTarget)))
                .Returns(provideValueTarget);
        }

        /// <summary>
        /// 
        /// </summary>
        [UIFact]
        public void Is_A_MarkupExtension()
        {
            var target = new GettextExtension("some msgid");
            Xunit.Assert.IsAssignableFrom<MarkupExtension>(target);
        }

        /// <summary>
        /// 
        /// </summary>
        [UIFact]
        public void ProvideValue_Returns_Text_For_MsgId()
        {
            var msgId = "msgid";
            var text = "translation";
            var target = new GettextExtension(msgId);
            GettextExtension.Localizer = Substitute.For<ILocalizer>();
            GettextExtension.Localizer.Catalog.GetString(Arg.Is(msgId))
                .Returns(text);

            Xunit.Assert.Equal(text, target.ProvideValue(_serviceProvider));
        }

        /// <summary>
        /// 
        /// </summary>
        [UIFact]
        public void ProvideValue_Returns_Text_For_MsgId_With_Glib_Style_Context()
        {
            var msgId = "test|msgid";
            var text = "translation";
            var target = new GettextExtension("some|test|msgid");
            GettextExtension.Localizer = Substitute.For<ILocalizer>();
            var context = "some";
            GettextExtension.Localizer.Catalog.GetParticularString(Arg.Is(context), Arg.Is(msgId))
                .Returns(text);

            Xunit.Assert.Equal(text, target.ProvideValue(_serviceProvider));
        }

        /// <summary>
        /// 
        /// </summary>
        [UIFact]
        public void ProvideValue_Returns_Text_For_MsgId_With_Params()
        {
            var msgId = "msgid";
            var text = "translation";
            object[] args = ["foo", 42];
            var target = new GettextExtension(msgId, args);
            GettextExtension.Localizer = Substitute.For<ILocalizer>();
            GettextExtension.Localizer.Catalog.GetString(Arg.Is(msgId), Arg.Is(args))
                .Returns(text);

            Xunit.Assert.Equal(text, target.ProvideValue(_serviceProvider));
        }

        /// <summary>
        /// 
        /// </summary>
        [UIFact]
        public void ValueTarget_Is_Updated_On_Localizer_CultureTracker_CultureChanged()
        {
            var msgId = "msgid";
            var text = "translation";
            object[] args = ["foo", 42];
            GettextExtension target = new(msgId, args);
            GettextExtension.Localizer = Substitute.For<ILocalizer>();
            GettextExtension.Localizer.CultureTracker
                .Returns(new CultureTracker());

            target.ProvideValue(_serviceProvider);

            GettextExtension.Localizer.Catalog.GetString(Arg.Is(msgId), Arg.Is(args))
                .Returns(text);
            GettextExtension.Localizer.CultureTracker.CurrentCulture = new("da-DK");

            Xunit.Assert.Equal(text, _valueTarget.Text);
        }

        /// <summary>
        /// 
        /// </summary>
        [UIFact]
        public void ValueTarget_Is_Not_Updated_On_Localizer_CultureTracker_CultureChanged_When_ValueTarget_Has_Been_Unloaded()
        {
            var msgId = "msgID";
            var text = "text";
            var oldText = "old text";
            _valueTarget.Text = oldText;
            object[] args = ["foo", 42];
            GettextExtension target = new(msgId, args);
            GettextExtension.Localizer = Substitute.For<ILocalizer>();

            target.ProvideValue(_serviceProvider);
            _valueTarget.RaiseEvent(new RoutedEventArgs(FrameworkElement.UnloadedEvent));

            GettextExtension.Localizer.Catalog.GetString(Arg.Is(msgId), Arg.Is(args))
                .Returns(text);
#pragma warning disable CS0618 // Type or member is obsolete
            GettextExtension.Localizer.CultureTracker.CultureChanged +=
                Raise.Event<EventHandler<CultureEventArgs>>(new CultureEventArgs(new CultureInfo("en-US")));

            Xunit.Assert.Equal(oldText, _valueTarget.Text);
        }
    }
}