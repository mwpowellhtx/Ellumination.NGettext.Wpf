using System;
using System.Collections.Generic;
using System.Globalization;

namespace NGettext.Wpf.Serialization
{
    using Newtonsoft.Json;
    using NSubstitute;
    using Xunit;

    /// <summary>
    /// 
    /// </summary>
    public class TranslationSerializerTest
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly LocalizationSerializer _target;

        /// <summary>
        /// 
        /// </summary>
        private readonly ICatalog _enCatalog = Substitute.For<ICatalog>();

        /// <summary>
        /// 
        /// </summary>
        private readonly ICatalog _daCatalog = Substitute.For<ICatalog>();

        /// <summary>
        /// 
        /// </summary>
        public TranslationSerializerTest()
        {
            _target = new(CreateCatalog);

            // TODO: establish a better helper method, first for 'english' catalogs
            // TODO: then also for 'da catalogs'
            // TODO: may also think about separating out into entirely different test classes
            _enCatalog.GetParticularString("Some context", "English message")
                .Returns("English message");

            _enCatalog.GetString("Other English message")
                .Returns("Other English message");

            _enCatalog.GetString("Quotes \"")
                .Returns("Quotes \"");

            _enCatalog.GetParticularString("Some context", "English message {0}", 42)
                .Returns("English message 42");

            _enCatalog.GetString("Other English message {0}", 42)
                .Returns("Other English message 42");

            _daCatalog.GetParticularString("Some context", "English message")
                .Returns("Dansk besked");

            _daCatalog.GetString("Other English message")
                .Returns("Anden dansk besked");

            _daCatalog.GetString("Quotes \"")
                .Returns("Gåseøjne \"");

            _daCatalog.GetParticularString("Some context", "English message {0}", 42)
                .Returns("Dansk besked 42");

            _daCatalog.GetString("Other English message {0}", 42)
                .Returns("Anden dansk besked 42");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private ICatalog CreateCatalog(CultureInfo culture)
            => culture.Name switch
            {
                "en-US" => _enCatalog,
                "da-DK" => _daCatalog,
                _ => throw new NotImplementedException(),
            };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locale"></param>
        /// <param name="msgId"></param>
        /// <param name="message"></param>
        [Theory
            , InlineData("en-US", "Some context|English message", "English message")
            , InlineData("da-DK", "Some context|English message", "Dansk besked")
            , InlineData("en-US", "Other English message", "Other English message")
            , InlineData("da-DK", "Other English message", "Anden dansk besked")
            , InlineData("da-DK", "Quotes \"", "Gåseøjne \"")]
        public void Serializes_Localized_Messages(string locale, string msgId, string message)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            var serializedGettext = _target.SerializedGettext([new CultureInfo("en-US"), new CultureInfo("da-DK")], msgId);
            Assert.NotNull(serializedGettext);

            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(serializedGettext);
            Assert.NotNull(dictionary);

            Assert.True(dictionary.TryGetValue(locale, out var actual));

            Assert.Equal(message, actual);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locale"></param>
        /// <param name="msgId"></param>
        /// <param name="message"></param>
        [Theory
            , InlineData("en-US", "Some context|English message {0}", "English message 42")
            , InlineData("da-DK", "Some context|English message {0}", "Dansk besked 42")
            , InlineData("en-US", "Other English message {0}", "Other English message 42")
            , InlineData("da-DK", "Other English message {0}", "Anden dansk besked 42")]
        public void Serializes_Localized_Messages_Args(string locale, string msgId, string message)
        {
            var serializedGettext = _target.SerializedGettext([new CultureInfo("en-US"), new CultureInfo("da-DK")], msgId, 42);
            Assert.NotNull(serializedGettext);

            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(serializedGettext);
            Assert.NotNull(dictionary);

            Assert.True(dictionary.TryGetValue(locale, out var actual));

            Assert.Equal(message, actual);
        }
    }
}