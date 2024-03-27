using System;
using System.Windows.Markup;

namespace NGettext.Wpf
{
    using Common;
    using NSubstitute;
    using Xunit;

    /// <summary>
    /// 
    /// </summary>
    public class GettextFormatConverterExtensionTests
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly MarkupExtension _target;

        /// <summary>
        /// 
        /// </summary>
        public GettextFormatConverterExtensionTests()
        {
            _target = new GettextFormatConverterExtension("MSGID {0}");
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void Provides_GettextStringFormatConverter()
        {
            var serviceProvider = Substitute.For<IServiceProvider>();

            var value = Xunit.Assert.IsAssignableFrom<GettextStringFormatConverter>(
                _target.ProvideValue(serviceProvider)
            );

            Xunit.Assert.Equal("MSGID {0}", value.MsgId);
        }
    }
}