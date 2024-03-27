using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web;

namespace NGettext.Wpf.Serialization
{
    using JetBrains.Annotations;

    /// <summary>
    /// 
    /// </summary>
    public class LocalizationSerializer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public delegate ICatalog CreateCatalogHandler(CultureInfo culture);

        /// <summary>
        /// Gets the <see cref="ICatalog"/> factory as a function of <see cref="CultureInfo"/>.
        /// </summary>
        private CreateCatalogHandler CreateCatalog { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createCatalog"></param>
#pragma warning disable IDE0290 // Use primary constructor
        public LocalizationSerializer(CreateCatalogHandler createCatalog)
        {
            CreateCatalog = createCatalog;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cultureInfos"></param>
        /// <param name="msgId"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        [StringFormatMethod("msgId"),
            Obsolete("This method is experimental, and may go away")]
        public string SerializedGettext(IEnumerable<CultureInfo> cultureInfos, string msgId, params object[] args)
        {
            // TODO: think about whether we can identify single|array bits
            // TODO: and do an appropriate join with comma delimiter
            const char beginJsonObj = '{';
            const char endJsonObj = '}';
            const char comma = ',';

            var msgIdWithContext = LocalizerExtensions.ConvertToMsgIdWithContext(msgId);
            var result = new StringBuilder();
            result.Append(beginJsonObj);
            var addComma = false;
            foreach (var cultureInfo in cultureInfos)
            {
                if (addComma)
                {
                    result.Append(comma);
                }
                else
                {
                    addComma = true;
                }

                string SerializeMessage(ICatalog cat)
                {
                    if (string.IsNullOrEmpty(msgIdWithContext.Context))
                    {
                        if (args.Length > 0)
                        {
                            return cat.GetString(msgIdWithContext.MsgId, args);
                        }
                        else
                        {
                            return cat.GetString(msgIdWithContext.MsgId);
                        }
                    }
                    else
                    {
                        if (args.Length > 0)
                        {
                            return cat.GetParticularString(msgIdWithContext.Context, msgIdWithContext.MsgId, args);
                        }
                        else
                        {
                            return cat.GetParticularString(msgIdWithContext.Context, msgIdWithContext.MsgId);
                        }
                    }
                }

                var catalog = CreateCatalog(cultureInfo);

                var message = SerializeMessage(catalog);

                // TODO: could also think about delimit name value pair with colon ':'
                result.AppendFormat("\"{0}\": \"{1}\"", cultureInfo.Name, HttpUtility.JavaScriptStringEncode(message));
            }

            result.Append(endJsonObj);
            return result.ToString();
        }
    }
}