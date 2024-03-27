using System;
using System.Globalization;

namespace NGettext.Wpf
{
    using Xunit;

    /// <summary>
    /// 
    /// </summary>
    public class CultureEventArgsTests
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly CultureEventArgs _target;

        /// <summary>
        /// 
        /// </summary>
        public CultureEventArgsTests()
        {
            _target = new(CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void NotNullParameter_Culture()
        {
            // 'Unused' instance creation is intentional here, since we are testing the parameter
#pragma warning disable CA1806 // Do not ignore method results
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Assert.NotNullParameter("culture", () => new CultureEventArgs(null));
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void CultureInfo_Is_Injected()
        {
            CultureInfo culture = new("en-GB");
            CultureEventArgs target = new(culture);
            Xunit.Assert.Same(culture, target.Culture);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void Is_An_EventArgs()
        {
            Xunit.Assert.IsAssignableFrom<EventArgs>(_target);
        }
    }
}