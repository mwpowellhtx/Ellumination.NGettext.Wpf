using System;
using System.Linq;
using System.Reflection;

namespace NGettext.Wpf.EnumLocalization
{
    /// <inheritdoc/>
    public class EnumLocalizer : IEnumLocalizer
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizer _localizer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localizer"></param>
        /// <exception cref="ArgumentNullException"></exception>
#pragma warning disable IDE0290 // Use primary constructor
        public EnumLocalizer(ILocalizer localizer)
        {
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string LocalizeEnum(Enum value)
        {
            var type = value.GetType();
            var enumMemberName = value.ToString();
            var msgIdAttribute = type.GetMember(enumMemberName).SingleOrDefault()?.GetCustomAttribute<EnumMsgIdAttribute>(true);

            // TODO: what are we localizing here, exactly? the name only?
            // TODO: how about for name, description (i.e. tooltip) etc
            // TODO: think about whether 'DisplayAttribute' is more appropriate at some level
            if (msgIdAttribute is null)
            {
                Console.Error.WriteLine($"{type}.{enumMemberName} lacks the [MsgId(\"...\")] attribute.");
                return enumMemberName;
            }

            return _localizer.Gettext(msgIdAttribute.MsgId);
        }
    }
}