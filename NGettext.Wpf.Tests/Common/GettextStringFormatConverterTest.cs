using System.Windows.Data;

namespace NGettext.Wpf.Common
{
    using NSubstitute;
    using Xunit;

    /// <summary>
    /// 
    /// </summary>
    public class GettextStringFormatConverterTest
    {
        /// <summary>
        /// 
        /// </summary>
#pragma warning disable CA1859 // Use concrete types when possible for improved performance
        private readonly IValueConverter _target;

        /// <summary>
        /// 
        /// </summary>
        public GettextStringFormatConverterTest()
        {
            _target = new GettextStringFormatConverter("MSGID {0}");
            GettextStringFormatConverter.Localizer = Substitute.For<ILocalizer>();
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void Convert_Formats_Value_With_Localized_MsgId()
        {
            var value = "SOME VALUE";
            GettextStringFormatConverter.Localizer.Catalog.GetString(Arg.Is("MSGID {0}"), Arg.Is(value))
                .Returns("FORMATTED TRANSLATION");

            var actual = _target.Convert(value, null, null, null);
            Assert.Equal("FORMATTED TRANSLATION", actual);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void Convert_Formats_Value_With_Localized_MsgId_Using_Localization_Context()
        {
            var target = new GettextStringFormatConverter("CTX|MSGID {0}");

            var value = "SOME VALUE";
            GettextStringFormatConverter.Localizer.Catalog.GetParticularString(Arg.Is("CTX"), Arg.Is("MSGID {0}"), Arg.Is(value))
                .Returns("FORMATTED TRANSLATION");

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var actual = target.Convert(value, null, null, null);
            Assert.Equal("FORMATTED TRANSLATION", actual);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void Convert_Falls_Back_To_Formats_Value_With_Unlocalized_MsgId()
        {
            var value = "SOME VALUE";
            GettextStringFormatConverter.Localizer = null;

            var actual = _target.Convert(value, null, null, null);
            Assert.Equal("MSGID SOME VALUE", actual);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void Convert_Falls_Back_To_Formats_Value_With_Unlocalized_MsgId_Stripping_Context()
        {
            GettextStringFormatConverter target = new("CTX|MSGID {0}");
            var value = "SOME VALUE";
            GettextStringFormatConverter.Localizer = null;

            var actual = target.Convert(value, null, null, null);
            Assert.Equal("MSGID SOME VALUE", actual);
        }
    }
}