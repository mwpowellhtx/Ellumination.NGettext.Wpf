using System;

namespace NGettext.Wpf.EnumLocalization
{
    // TODO: I see what was being done here...
    // TODO: but it ignores the fact there is a DisplayAttribute that would be more useful for Enum values
    // TODO: which being the case, or at least 'now' there is, do not know how contemporary that is/was at the time
    // TODO: may want to think about whether a 1C 'view model' is appropriate, with 'msgid' comprehension at the various levels exposing out the Display parts
    // TODO: which would make referencing into things like combobox views more useful we think
    /// <summary>
    /// 
    /// </summary>
    public class EnumMsgIdAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string MsgId { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgId"></param>
        public EnumMsgIdAttribute(string msgId)
        {
            MsgId = msgId;
        }
    }
}