using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace NGettext.Wpf.Example
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        /// <summary>
        /// 
        /// </summary>
        private int _memoryLeakTestProgress;

        /// <summary>
        /// 
        /// </summary>
        private DateTime _currentTime;

        /// <summary>
        /// 
        /// </summary>
        private readonly string _someDeferredLocalization = Localization.Noop("Deferred localization");

        /// <summary>
        /// 
        /// </summary>
        private int _counter;

        // TODO: need to sub in 'behaviors' for expression blend deps
        /// <summary>
        /// 
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.1) };
            timer.Tick += (sender, args) => { CurrentTime = DateTime.Now; };
            timer.Tick += (sender, args) => { Counter = (Counter + 1) % 1000; };
            timer.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal SomeNumber => 1234567.89m;

        /// <summary>
        /// 
        /// </summary>
        public DateTime CurrentTime
        {
            get => _currentTime;
            set
            {
                _currentTime = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentTime)));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OpenMemoryLeakTestWindow(object sender, RoutedEventArgs e)
        {
            var leakTestWindowReference = GetWeakReferenceToLeakTestWindow();
            for (var i = 0; i < 20; ++i)
            {
                if (!leakTestWindowReference.TryGetTarget(out _)) return;
                await Task.Delay(TimeSpan.FromSeconds(1));
                GC.Collect();
            }
            Debug.Assert(!leakTestWindowReference.TryGetTarget(out _), "memory leak detected");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private WeakReference<MemoryLeakTestWindow> GetWeakReferenceToLeakTestWindow()
        {
            var window = new MemoryLeakTestWindow();
            window.Closed += async (o, args) =>
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                ++MemoryLeakTestProgress;
                foreach (var locale in new[]
                    {"da-DK", "de-DE", "en-US", TrackCurrentCultureBehavior.CultureTracker?.CurrentCulture?.Name})
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    if (TrackCurrentCultureBehavior.CultureTracker != null)
                    {
                        TrackCurrentCultureBehavior.CultureTracker.CurrentCulture = CultureInfo.GetCultureInfo(locale);
                    }

                    ++MemoryLeakTestProgress;
                }
            };
            window.Show();
            MemoryLeakTestProgress = 0;

            window.Close();

            return new WeakReference<MemoryLeakTestWindow>(window);
        }

        /// <summary>
        /// 
        /// </summary>
        public int MemoryLeakTestProgress
        {
            get => _memoryLeakTestProgress;
            set
            {
                _memoryLeakTestProgress = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MemoryLeakTestProgress)));
            }
        }

        // TODO: surely also there are 'better' ways of doing this
        // TODO: migrate into our baked in enum view models
        public ICollection<ExampleEnum> EnumValues { get; } = Enum.GetValues(typeof(ExampleEnum)).Cast<ExampleEnum>().ToList();

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        // TODO: rather like the '_' method naming convention
        // TODO: somewhat in keeping with the underpinning NGettext approach as well
        /// <summary>
        /// 
        /// </summary>
        public string SomeDeferredLocalization => Localization._(_someDeferredLocalization);

        // TODO: which and if we are listening/tracking for culture changes...
        // TODO: then do we not also have to notify "Header" and the other props as well (?)
        /// <summary>
        /// 
        /// </summary>
        public string Header => Localization._("NGettext.WPF Example");

        // TODO: what is the argument list migration path
        /// <summary>
        /// 
        /// </summary>
        public string PluralGettext => Localization.PluralGettext(1, "Singular", "Plural") +
                                       "---" + Localization.PluralGettext(2, "Singular", "Plural");

        /// <summary>
        /// 
        /// </summary>
        public string PluralGettextParams => Localization.PluralGettext(1, "Singular {0:n3}", "Plural {0:n3}", 1m / 3m) +
                                             "---" + Localization.PluralGettext(2, "Singular {0:n3}", "Plural {0:n3}", 1m / 3m);

        // TODO: notifying properties on the window itself, not great MVVM form, IMO
        /// <summary>
        /// 
        /// </summary>
        public int Counter
        {
            get => _counter;
            set
            {
                _counter = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Counter)));
            }
        }
    }
}