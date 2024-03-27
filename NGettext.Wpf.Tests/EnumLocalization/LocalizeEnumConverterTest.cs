using System.Windows.Data;

namespace NGettext.Wpf.EnumLocalization
{
    using NSubstitute;
    using Xunit;

    /// <summary>
    /// 
    /// </summary>
    public class LocalizeEnumConverterTest
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IEnumLocalizer _enumLocalizer = Substitute.For<IEnumLocalizer>();

        /// <summary>
        /// 
        /// </summary>
#pragma warning disable CA1859 // Use concrete types when possible for improved performance
        private readonly IValueConverter _target;

        /// <summary>
        /// 
        /// </summary>
        public LocalizeEnumConverterTest()
        {
            _target = new LocalizeEnumConverter(_enumLocalizer);
        }

        /// <summary>
        /// 
        /// </summary>
        private enum TestEnum
        {
            EnumValue,
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void Converts_Enum_Value_To_Localized_Value()
        {
            _enumLocalizer.LocalizeEnum(Arg.Is(TestEnum.EnumValue))
                .Returns("localized value");

            var actual = _target.Convert(TestEnum.EnumValue, null, null, null);
            Assert.Equal("localized value", actual);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void Converts_Null_To_Null()
        {
            var actual = Assert.IsAssignableFrom<IValueConverter>(_target)
                .Convert(null, null, null, null);
            Assert.Null(actual);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void Nothing_Bad_Happens_When_There_Is_No_Enum_Localizer()
        {
            var target = new LocalizeEnumConverter();

            var actual = target.Convert(TestEnum.EnumValue, null, null, null);
            Assert.Equal(TestEnum.EnumValue, actual);
        }
    }
}