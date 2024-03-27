using System.Collections.Generic;
using System.Globalization;

namespace NGettext.Wpf
{
    using Newtonsoft.Json;
    using NSubstitute;
    using Xunit;

    /// <summary>
    /// 
    /// </summary>
    public class LocalizationTests
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizer _localizer = Substitute.For<ILocalizer>();

        /// <summary>
        /// 
        /// </summary>
        public LocalizationTests()
        {
            Localization.Localizer = _localizer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgId"></param>
        /// <param name="expected"></param>
        [Theory
            , InlineData("some msgid", "some translation")]
        public void Underscore_Returns_Expected_Localization(string msgId, string expected)
        {
            _localizer.Catalog.GetString(Arg.Is(msgId))
                .Returns(expected);
            var actual = Localization._(msgId);
            Xunit.Assert.Equal(expected, actual);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void Underscore_Does_Not_Crash_When_MsgId_Is_Null()
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var e = Record.Exception(() => Localization._(null));
            Xunit.Assert.Null(e);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void Underscore_With_Args_Does_Not_Crash_When_MsgId_Is_Null()
        {
            var e = Record.Exception(() => Localization._(null, 1, 2, 3));

            Xunit.Assert.Null(e);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void Underscore_Allows_String_Interpolation()
        {
            _localizer.Catalog.GetString(Arg.Is("foo {0} bar {1} baz"), Arg.Is(0xdead), Arg.Is(0xbeef))
                .Returns("expected translation");

            var actual = Localization._("foo {0} bar {1} baz", 0xdead, 0xbeef);
            Xunit.Assert.Equal("expected translation", actual);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void Underscore_Function_Handles_Localizer_Being_Null()
        {
            Localization.Localizer = null;
            var actual = Localization._("untranslated");
            Xunit.Assert.Equal("untranslated", actual);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgId"></param>
        /// <param name="expected"></param>
        /// <param name="context"></param>
        [Theory
            , InlineData("context", "some msgid", "some translation")]
        public void GetParticularString_Returns_Expected_Localization(string msgId, string expected, string context)
        {
            _localizer.Catalog.GetParticularString(Arg.Is(context), Arg.Is(msgId))
                .Returns(expected);
            var actual = Localization.GetParticularString(context, msgId);
            Xunit.Assert.Equal(expected, actual);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void GetParticularString_Allows_String_Interpolation()
        {
            _localizer.Catalog.GetParticularString(Arg.Is("context"), Arg.Is("foo {0} bar {1} baz"), Arg.Is(0xdead), Arg.Is(0xbeef))
                .Returns("expected translation");

            var actual = Localization.GetParticularString("context", "foo {0} bar {1} baz", 0xdead, 0xbeef);
            Xunit.Assert.Equal("expected translation", actual);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void GetParticularString_Function_Handles_Localizer_Being_Null()
        {
            Localization.Localizer = null;
            var actual = Localization.GetParticularString("context", "untranslated");
            Xunit.Assert.Equal("untranslated", actual);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void Noop_Returns_MsgId()
        {
            var actual = Localization.Noop("some msgid");
            Xunit.Assert.Equal("some msgid", actual);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <param name="singularMsgId"></param>
        /// <param name="pluralMsgId"></param>
        /// <param name="expected"></param>
        [Theory
            , InlineData(42, "singular string", "plural string", "expected translation")]
        public void GetPluralString_Returns_Expected_Localization(int n, string singularMsgId, string pluralMsgId, string expected)
        {
            _localizer.Catalog.GetPluralString(singularMsgId, pluralMsgId, n)
                .Returns(expected);

            var actual = Localization.GetPluralString(singularMsgId, pluralMsgId, n);
            Xunit.Assert.Equal(expected, actual);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <param name="singularMsgId"></param>
        /// <param name="pluralMsgId"></param>
        /// <param name="expected"></param>
        [Theory
            , InlineData(42, "singular string", "plural string", "plural string")
            , InlineData(1, "singular string", "plural string", "singular string")
            , InlineData(0, "singular string", "plural string", "plural string")
            , InlineData(-12, "singular string", "plural string", "plural string")]
        public void GetPluralString_Function_Handles_Localizer_Being_Null(int n, string singularMsgId, string pluralMsgId, string expected)
        {
            Localization.Localizer = null;

            var actual = Localization.GetPluralString(singularMsgId, pluralMsgId, n);
            Xunit.Assert.Equal(expected, actual);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <param name="singularMsgId"></param>
        /// <param name="pluralMsgId"></param>
        /// <param name="expected"></param>
        /// <param name="args"></param>
        [Theory
            , InlineData(42, "singular string", "plural string", "expected translation", 1, 2.0, "foo")]
        public void GetPluralString_Supports_String_Interpolation(int n, string singularMsgId, string pluralMsgId, string expected, params object[] args)
        {
            _localizer.Catalog.GetPluralString(singularMsgId, pluralMsgId, n, args)
                .Returns(expected);

            var actual = Localization.GetPluralString(singularMsgId, pluralMsgId, n, args);
            Xunit.Assert.Equal(expected, actual);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <param name="singularMsgId"></param>
        /// <param name="pluralMsgId"></param>
        /// <param name="expected"></param>
        /// <param name="args"></param>
        [Theory
            , InlineData(42, "0x{0:x} looks like beef", "all 0x{0:x} look like beef", "all 0xbeef look like beef", 0xbeef)
            , InlineData(1, "0x{0:x} looks like beef", "all 0x{0:x} look like beef", "0xbeef looks like beef", 0xbeef)
            , InlineData(0, "0x{0:x} looks like beef", "all 0x{0:x} look like beef", "all 0xbeef look like beef", 0xbeef)
            , InlineData(-7, "0x{0:x} looks like beef", "all 0x{0:x} look like beef", "all 0xbeef look like beef", 0xbeef)]
        public void GetPluralString_Handles_Localizer_Being_Null_With_Parameters(int n, string singularMsgId, string pluralMsgId, string expected, params object[] args)
        {
            Localization.Localizer = null;

            var actual = Localization.GetPluralString(singularMsgId, pluralMsgId, n, args);
            Xunit.Assert.Equal(expected, actual);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <param name="singularMsgId"></param>
        /// <param name="pluralMsgId"></param>
        /// <param name="expected"></param>
        /// <param name="context"></param>
        [Theory
            , InlineData(42, "singular string", "plural string", "expected translation", "context")]
        public void GetParticularPluralString_Returns_Expected_Localization(int n, string singularMsgId, string pluralMsgId, string expected, string context)
        {
            _localizer.Catalog.GetParticularPluralString(context, singularMsgId, pluralMsgId, n)
                .Returns(expected);

            var actual = Localization.GetParticularPluralString(context, singularMsgId, pluralMsgId, n);
            Xunit.Assert.Equal(expected, actual);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <param name="singularMsgId"></param>
        /// <param name="pluralMsgId"></param>
        /// <param name="expected"></param>
        /// <param name="context"></param>
        [Theory
            , InlineData(42, "singular string", "plural string", "plural string", "context")
            , InlineData(1, "singular string", "plural string", "singular string", "context")
            , InlineData(0, "singular string", "plural string", "plural string", "context"), InlineData(-12, "singular string", "plural string", "plural string", "context")]
        public void GetParticularPluralString_Function_Handles_Localizer_Being_Null(int n, string singularMsgId, string pluralMsgId, string expected, string context)
        {
            Localization.Localizer = null;

            var actual = Localization.GetParticularPluralString(context, singularMsgId, pluralMsgId, n);
            Xunit.Assert.Equal(expected, actual);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="n"></param>
        /// <param name="singularMsgId"></param>
        /// <param name="pluralMsgId"></param>
        /// <param name="expected"></param>
        /// <param name="args"></param>
        [Theory
            , InlineData("context", 42, "singular string", "plural string", "expected translation", 1, 2.0, "foo")]
        public void GetParticularPluralString_Supports_String_Interpolation(string context, int n, string singularMsgId, string pluralMsgId, string expected, params object[] args)
        {
            _localizer.Catalog.GetParticularPluralString(context, singularMsgId, pluralMsgId, n, args)
                .Returns(expected);

            var actual = Localization.GetParticularPluralString(context, singularMsgId, pluralMsgId, n, args);
            Xunit.Assert.Equal(expected, actual);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="n"></param>
        /// <param name="singularMsgId"></param>
        /// <param name="pluralMsgId"></param>
        /// <param name="expected"></param>
        /// <param name="args"></param>
        [Theory
            , InlineData("context", 42, "0x{0:x} looks like beef", "all 0x{0:x} look like beef", "all 0xbeef look like beef", 0xbeef)
            , InlineData("context", 1, "0x{0:x} looks like beef", "all 0x{0:x} look like beef", "0xbeef looks like beef", 0xbeef)
            , InlineData("context", 0, "0x{0:x} looks like beef", "all 0x{0:x} look like beef", "all 0xbeef look like beef", 0xbeef)
            , InlineData("context", -7, "0x{0:x} looks like beef", "all 0x{0:x} look like beef", "all 0xbeef look like beef", 0xbeef)]
        public void GetParticularPluralString_Handles_Localizer_Being_Null_With_Parameters(string context, int n, string singularMsgId, string pluralMsgId, string expected, params object[] args)
        {
            Localization.Localizer = null;

            var actual = Localization.GetParticularPluralString(context, singularMsgId, pluralMsgId, n, args);
            Xunit.Assert.Equal(expected, actual);
        }

#if ALPHA

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void Static_LocalizationSerializer_Wrapper_Has_Fallback()
        {
            Localization.Localizer = null;

#pragma warning disable CS0618 // Type or member is obsolete
            var serializedGettext = Localization.SerializedGettext([new CultureInfo("en-US"), new CultureInfo("da-DK")], "Context|Message");
            Xunit.Assert.NotNull(serializedGettext);

            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(serializedGettext);
            Xunit.Assert.NotNull(dictionary);

            Xunit.Assert.True(dictionary.TryGetValue("da-DK", out var actual));

            Xunit.Assert.Equal("Message", actual);
        }

#endif

    }
}