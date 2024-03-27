using NGettext.Wpf.EnumLocalization;

// TODO: this one needs to be rethunk, especially since EnumMsgId is effectively oversimplified
namespace NGettext.Wpf.Example
{
    /// <summary>
    /// 
    /// </summary>
    public enum ExampleEnum
    {
        /// <summary>
        /// 
        /// </summary>
        [EnumMsgId("Some value")]
        SomeValue,

        /// <summary>
        /// 
        /// </summary>
        [EnumMsgId("Some other value")]
        SomeOtherValue,

        /// <summary>
        /// 
        /// </summary>
        [EnumMsgId("Some third value")]
        SomeThirdValue,

        /// <summary>
        /// 
        /// </summary>
        [EnumMsgId("EnumMsgId example|Some fourth value")]
        SomeFourthValue,

        /// <summary>
        /// 
        /// </summary>
        SomeValueWithoutEnumMsgId,
    }
}