namespace NGettext.Wpf
{
    /// <summary>
    /// 
    /// </summary>
    public static class LocalizerExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        internal struct MsgIdWithContext
        {
            /// <summary>
            /// 
            /// </summary>
            internal string Context { get; set; }

            /// <summary>
            /// 
            /// </summary>
            internal string MsgId { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgId"></param>
        /// <returns></returns>
        internal static MsgIdWithContext ConvertToMsgIdWithContext(string msgId)
        {
            var result = new MsgIdWithContext { MsgId = msgId };

            const char pipe = '|';

            if (msgId.Contains(pipe))
            {
                var pipePosition = msgId.IndexOf(pipe);
                result.Context = msgId[..pipePosition];
                result.MsgId = msgId[(pipePosition + 1)..];
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="this"></param>
        /// <param name="msgId"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        internal static string Gettext(this ILocalizer localizer, string msgId, params object[] values)
        {
            if (msgId is null)
            {
                return null;
            }

            var msgIdWithContext = ConvertToMsgIdWithContext(msgId);

            if (localizer is null)
            {
                CompositionRoot.WriteMissingInitializationErrorMessage();
                return string.Format(msgIdWithContext.MsgId, values);
            }

            var localizer_Catalog = localizer.Catalog;

            return msgIdWithContext.Context != null
                ? localizer_Catalog.GetParticularString(msgIdWithContext.Context, msgIdWithContext.MsgId, values)
                : localizer_Catalog.GetString(msgIdWithContext.MsgId, values);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localizer"></param>
        /// <param name="msgId"></param>
        /// <returns></returns>
        internal static string Gettext(this ILocalizer localizer, string msgId)
        {
            if (msgId is null)
            {
                return null;
            }

            var msgIdWithContext = ConvertToMsgIdWithContext(msgId);

            if (localizer is null)
            {
                CompositionRoot.WriteMissingInitializationErrorMessage();
                return msgIdWithContext.MsgId;
            }

            var localizer_Catalog = localizer.Catalog;

            return msgIdWithContext.Context != null
                ? localizer_Catalog.GetParticularString(msgIdWithContext.Context, msgIdWithContext.MsgId)
                : localizer_Catalog.GetString(msgIdWithContext.MsgId);
        }
    }
}