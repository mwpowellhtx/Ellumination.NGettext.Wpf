using System;
using System.Globalization;

namespace NGettext.Wpf
{
    using NSubstitute;
    using Xunit;

    /// <summary>
    /// 
    /// </summary>
    public class LocalizerTests
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ICultureTracker _cultureTracker = Substitute.For<ICultureTracker>();

        /// <summary>
        /// 
        /// </summary>
        private readonly Localizer _target;

        /// <summary>
        /// 
        /// </summary>
        private readonly CultureInfo _initialCulture = new("da-DK");

        /// <summary>
        /// 
        /// </summary>
        private readonly CultureInfo _changedCulture = new("en-US");

        /// <summary>
        /// 
        /// </summary>
        public LocalizerTests()
        {
            _cultureTracker.CurrentCulture
                .Returns(_initialCulture);
            _target = new Localizer(_cultureTracker, "some domain");
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void NotNullParameter_CultureTracker()
        {
#pragma warning disable CA1806 // Do not ignore method results
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Assert.NotNullParameter("cultureTracker", () => new Localizer(null, "domain requires tracker"));
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void Catalog_Is_Initialized_From_CultureTracker_CurrentCulture()
        {
            var culture= Xunit.Assert.IsAssignableFrom<Catalog>(_target.Catalog).CultureInfo;
            Xunit.Assert.Same(_cultureTracker.CurrentCulture, culture);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void Catalog_Is_Reset_When_CultureTracker_CultureChanging()
        {
            _cultureTracker.CultureChanging +=
                Raise.Event<EventHandler<CultureEventArgs>>(new CultureEventArgs(_changedCulture));
            var culture = Xunit.Assert.IsAssignableFrom<Catalog>(_target.Catalog).CultureInfo;
            Xunit.Assert.Same(_changedCulture, culture);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void Is_Disposable()
        {
            Xunit.Assert.IsAssignableFrom<IDisposable>(_target);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void Catalog_Is_Not_Reset_When_CultureTracker_CultureChanging_After_Disposal()
        {
            _target.Dispose();
            _cultureTracker.CultureChanging +=
                Raise.Event<EventHandler<CultureEventArgs>>(new CultureEventArgs(_changedCulture));
            var culture = Xunit.Assert.IsAssignableFrom<Catalog>(_target.Catalog).CultureInfo;
            Xunit.Assert.Same(_initialCulture, culture);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void CultureTracker_Is_Injected()
        {
            Xunit.Assert.Same(_cultureTracker, _target.CultureTracker);
        }
    }
}