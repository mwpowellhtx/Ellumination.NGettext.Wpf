using System;
using System.Globalization;

namespace NGettext.Wpf
{
    using NSubstitute;
    using Xunit;

    /// <summary>
    /// 
    /// </summary>
    public class CultureTrackerTests
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly CultureTracker _target;

        /// <summary>
        /// 
        /// </summary>
        public CultureTrackerTests()
        {
            _target = new CultureTracker();
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void Setting_CurrentCulture_Raise_CultureChanging_And_Then_CultureChanged()
        {
            CultureInfo culture = new("en-US");
            var cultureChanging = Substitute.For<EventHandler<CultureEventArgs>>();
            _target.CultureChanging += cultureChanging;

            var cultureChanged = Substitute.For<EventHandler<CultureEventArgs>>();
            _target.CultureChanged += cultureChanged;

            _target.CurrentCulture = culture;

            Received.InOrder(() =>
            {
                cultureChanging.Invoke(Arg.Is(_target), Arg.Is<CultureEventArgs>(e => e.Culture == culture));
                cultureChanged.Invoke(Arg.Is(_target), Arg.Is<CultureEventArgs>(e => e.Culture == culture));
            });
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void Setting_CurrentCulture_Raise_CultureChanged_After_CurrentCulture_Changed()
        {
            CultureInfo culture = new("en-US");
            _target.CultureChanged += (s, e) => Xunit.Assert.Same(culture, _target.CurrentCulture);

            _target.CurrentCulture = culture;
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void Setting_CurrentCulture_Raise_CultureChanging_Before_CurrentCulture_Changed()
        {
            CultureInfo culture = new("en-US");
            var oldCulture = _target.CurrentCulture;
            _target.CultureChanging += (s, e) => Xunit.Assert.Same(oldCulture, _target.CurrentCulture);

            _target.CurrentCulture = culture;
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void CurrentCulture_Is_Initially_CurrentUiCulture()
        {
            Xunit.Assert.Same(CultureInfo.CurrentUICulture, _target.CurrentCulture);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void Setting_CurrentCulture_Notifies_Weak_Culture_Observers()
        {
            var cultureObserver = Substitute.For<IWeakCultureObserver>();
            _target.AddWeakCultureObserver(cultureObserver);

            CultureInfo culture = new("en-US");

            _target.CurrentCulture = culture;

            cultureObserver.Received()
                .ChangeCulture(Arg.Is(_target), Arg.Is<CultureEventArgs>(e => e.Culture == culture));
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void Weak_Culture_Observers_May_Be_Garbage_Collected()
        {
            var weakCultureObserver = GetWeakReferenceToObservingCultureObserver();
            GC.Collect();
            Xunit.Assert.False(weakCultureObserver.TryGetTarget(out var _));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private WeakReference<IWeakCultureObserver> GetWeakReferenceToObservingCultureObserver()
        {
            var cultureObserver = Substitute.For<IWeakCultureObserver>();
            _target.AddWeakCultureObserver(cultureObserver);
            return new WeakReference<IWeakCultureObserver>(cultureObserver);
        }
    }
}