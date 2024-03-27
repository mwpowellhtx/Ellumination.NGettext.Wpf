#nullable disable

using System;
using System.Collections.Generic;
using System.Globalization;

namespace NGettext.Wpf
{
    /// <inheritdoc/>
    public class CultureTracker : ICultureTracker
    {
        /// <inheritdoc/>
        private CultureInfo _currentCulture = CultureInfo.CurrentUICulture;

        /// <summary>
        /// 
        /// </summary>
        private List<WeakReference<IWeakCultureObserver>> _weakObservers = [];

        /// <inheritdoc/>
        public event EventHandler<CultureEventArgs> CultureChanged;

        /// <summary>
        /// 
        /// </summary>
        public CultureInfo CurrentCulture
        {
            get => _currentCulture;
            set
            {
                CultureChanging?.Invoke(this, new CultureEventArgs(value));
                _currentCulture = value;
                OnCultureChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnCultureChanged()
        {
            var args = new CultureEventArgs(CurrentCulture);
            CultureChanged?.Invoke(this, args);

            var weakObserversStillAlive = new List<WeakReference<IWeakCultureObserver>>();

            foreach (var weakReference in _weakObservers)
            {
                if (!weakReference.TryGetTarget(out var observer))
                {
                    continue;
                }

                observer.ChangeCulture(this, args);
                weakObserversStillAlive.Add(weakReference);
            }

            _weakObservers = weakObserversStillAlive;
        }

        /// <inheritdoc/>>
        public event EventHandler<CultureEventArgs> CultureChanging;

        /// <inheritdoc/>
        public void AddWeakCultureObserver(IWeakCultureObserver weakCultureObserver)
            => _weakObservers.Add(new WeakReference<IWeakCultureObserver>(weakCultureObserver));
    }
}