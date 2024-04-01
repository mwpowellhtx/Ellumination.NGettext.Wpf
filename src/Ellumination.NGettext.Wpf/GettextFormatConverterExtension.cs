using System;
using System.Windows.Markup;

namespace NGettext.Wpf
{
    using Common;

    /// <summary>
    /// 
    /// </summary>
    public class GettextFormatConverterExtension : MarkupExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgId"></param>
#pragma warning disable IDE0290 // Use primary constructor
        public GettextFormatConverterExtension(string msgId)
        {
            MsgId = msgId;
        }

        /// <summary>
        /// 
        /// </summary>
        [ConstructorArgument("msgId")]
        public string MsgId { get; set; }

        /// <inheritdoc/>
        public override object ProvideValue(IServiceProvider serviceProvider) => new GettextStringFormatConverter(MsgId);
    }
}