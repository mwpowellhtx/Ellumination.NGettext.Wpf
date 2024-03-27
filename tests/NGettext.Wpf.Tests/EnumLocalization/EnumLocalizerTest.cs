namespace NGettext.Wpf.EnumLocalization
{
    using NSubstitute;
    using Xunit;

    /// <summary>
    /// 
    /// </summary>
    public class EnumLocalizerTest
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizer _localizer = Substitute.For<ILocalizer>();

        /// <summary>
        /// 
        /// </summary>
        private readonly EnumLocalizer _target;

        /// <summary>
        /// 
        /// </summary>
        public EnumLocalizerTest()
        {
            _target = new EnumLocalizer(_localizer);
        }

        // TODO: migrate away from 'enummsgid' toward displayattribute based solution
        /// <summary>
        /// 
        /// </summary>
        public enum TestEnum
        {
            [EnumMsgId("enum value")]
            EnumValue,

            [EnumMsgId("another enum value")]
            AnotherEnumValue,

            [EnumMsgId("some context|a third enum value")]
            ThirdEnumValue,

            EnumValueWithoutMsgId,
        }

        /// <summary>
        /// 
        /// </summary>
        public enum EmptyEnum
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void Handles_Invalid_Enum_Member()
        {
            var e = Record.Exception(() => _target.LocalizeEnum(default(EmptyEnum)));
            Assert.Null(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumValue"></param>
        /// <param name="msgId"></param>
        [Theory,
            InlineData(TestEnum.EnumValue, "enum value"),
            InlineData(TestEnum.AnotherEnumValue, "another enum value")]
        public void Translates_MsgId_Of_Enum_Value(TestEnum enumValue, string msgId)
        {
            _localizer.Catalog.GetString(Arg.Is(msgId))
                .Returns("expected translation");

            var actual = _target.LocalizeEnum(enumValue);
            Assert.Equal("expected translation", actual);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumValue"></param>
        /// <param name="context"></param>
        /// <param name="msgId"></param>
        [Theory,
            InlineData(TestEnum.ThirdEnumValue, "some context", "a third enum value")]
        public void Translates_MsgId_Of_Enum_Value_With_Context(TestEnum enumValue, string context, string msgId)
        {
            _localizer.Catalog.GetParticularString(Arg.Is(context), Arg.Is(msgId))
                .Returns("expected translation");

            var actual = _target.LocalizeEnum(enumValue);
            Assert.Equal("expected translation", actual);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void Translates_Enum_Value_Without_MsgId_To_Value_Name()
        {
            var actual = _target.LocalizeEnum(TestEnum.EnumValueWithoutMsgId);
            Assert.Equal("EnumValueWithoutMsgId", actual);
        }
    }
}