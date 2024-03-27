using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace NGettext.Wpf
{
    // TODO: not sure what the JetBrains R# stuff is doing here, or what/why/where we do otherwise to 'fake it'
    using JetBrains.Annotations;
    using Serialization;

    /// <summary>
    /// 
    /// </summary>
    public static class Localization
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgId"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        [StringFormatMethod("msgId")]
        public static string _(string msgId, params object[] args)
            => args.Length > 0 ? Localizer.Gettext(msgId, args) : Localizer.Gettext(msgId);

        // TODO: this is one of those squirrely use cases in which nullable is 'not' helpful, getting in the way more than it is helping
        /// <summary>
        /// 
        /// </summary>
        public static ILocalizer Localizer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgId"></param>
        /// <returns></returns>
        public static string Noop(string msgId) => msgId;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <param name="singularMsgId"></param>
        /// <param name="pluralMsgId"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        [StringFormatMethod("singularMsgId")]
        [StringFormatMethod("pluralMsgId")] //< not yet supported, #1833369.
        [Obsolete("This method will be removed in 2.x. Use GetPluralString() instead.")]
        public static string PluralGettext(int n, string singularMsgId, string pluralMsgId, params object[] args)
            => GetPluralString(singularMsgId, pluralMsgId, n, args);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="singularMsgId"></param>
        /// <param name="pluralMsgId"></param>
        /// <param name="n"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        [StringFormatMethod("singularMsgId")
            , StringFormatMethod("pluralMsgId")] //< not yet supported, #1833369.
        public static string GetPluralString(string singularMsgId, string pluralMsgId, int n, params object[] args)
        {
            var localizer = Localizer;

            if (localizer is null)
            {
                CompositionRoot.WriteMissingInitializationErrorMessage();
                return string.Format(CultureInfo.InvariantCulture, n == 1 ? singularMsgId : pluralMsgId, args);
            }

            var localizer_Catalog = Localizer.Catalog;

            return args.Length > 0
                ? localizer_Catalog.GetPluralString(singularMsgId, pluralMsgId, n, args)
                : localizer_Catalog.GetPluralString(singularMsgId, pluralMsgId, n);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="text"></param>
        /// <param name="pluralText"></param>
        /// <param name="n"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        [StringFormatMethod("text")
            , StringFormatMethod("pluralText")] //< not yet supported, #1833369.
        public static string GetParticularPluralString(string context, string text, string pluralText, int n, params object[] args)
        {
            var localizer = Localizer;

            if (localizer is null)
            {
                CompositionRoot.WriteMissingInitializationErrorMessage();
                return string.Format(CultureInfo.InvariantCulture, n == 1 ? text : pluralText, args);
            }

            var localizer_Catalog = Localizer.Catalog;

            return args.Length > 0
                ? localizer_Catalog.GetParticularPluralString(context, text, pluralText, n, args)
                : localizer_Catalog.GetParticularPluralString(context, text, pluralText, n);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="text"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        [StringFormatMethod("text")]
        public static string GetParticularString(string context, string text, params object[] args)
        {
            var localizer = Localizer;

            if (localizer is null)
            {
                CompositionRoot.WriteMissingInitializationErrorMessage();
                return args.Length > 0 ? string.Format(CultureInfo.InvariantCulture, text, args) : text;
            }

            var localizer_Catalog = Localizer.Catalog;

            return args.Length > 0
                ? localizer_Catalog.GetParticularString(context, text, args)
                : localizer_Catalog.GetParticularString(context, text);
        }

#if ALPHA

        /// <summary>
        /// 
        /// </summary>
        private static LocalizationSerializer _localizationSerializer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cultureInfos"></param>
        /// <param name="msgId"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        [StringFormatMethod("msgId")
            , Obsolete("This method is experimental, and may go away")]
        public static string SerializedGettext(IEnumerable<CultureInfo> cultureInfos, string msgId, params object[] args)
        {
            if (Localizer is null)
            {
                var message = args.Length > 0
                    ? Localizer.Gettext(msgId, args)
                    : Localizer.Gettext(msgId);
                return "{" + string.Join(", ", cultureInfos.Select(c => $"\"{c.Name}\": \"{HttpUtility.JavaScriptStringEncode(message)}\"")) + "}";
            }

            _localizationSerializer ??= new(Localizer.CreateCatalog);

            return _localizationSerializer.SerializedGettext(cultureInfos, msgId, args);
        }

#endif

    }
}