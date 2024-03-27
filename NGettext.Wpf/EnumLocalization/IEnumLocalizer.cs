using System;

namespace NGettext.Wpf.EnumLocalization
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEnumLocalizer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        string LocalizeEnum(Enum value);
    }
}