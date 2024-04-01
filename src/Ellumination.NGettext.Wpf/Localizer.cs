using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;

namespace NGettext.Wpf
{
    /// <inheritdoc cref="ILocalizer" />
    public class Localizer : ILocalizer, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual string DomainName { get; }

        /// <summary>
        /// 
        /// </summary>
        public virtual ICultureTracker CultureTracker { get; }

        /// <summary>
        /// 
        /// </summary>
        public virtual ICatalog Catalog { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cultureTracker"></param>
        /// <param name="domainName"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public Localizer([NotNull] ICultureTracker cultureTracker, string domainName)
        {
            ArgumentNullException.ThrowIfNull(cultureTracker);
            DomainName = domainName;
            CultureTracker = cultureTracker;
            cultureTracker.CultureChanging += OnCultureChangingResetCatalog;
            ResetCatalog(cultureTracker.CurrentCulture);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCultureChangingResetCatalog(object sender, CultureEventArgs e) => ResetCatalog(e.Culture);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cultureInfo"></param>
        private void ResetCatalog(CultureInfo cultureInfo)
        {
            Catalog = CreateCatalog(cultureInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public ICatalog CreateCatalog(CultureInfo cultureInfo)
        {
            // TODO: what is 'LC_MESSAGES' (?)
            const string LC_MESSAGES = nameof(LC_MESSAGES);
            // TODO: also based on 'Locale' (?)
            const string Locale = nameof(Locale);
            // TODO: TBD: console write lines throughout (?)
            // TODO: TBD: would it make some sense to consider an 'ILogger' compliant approach (?)
            // TODO: TBD: perhaps even consider something like a Serilog (?) for the same
            Console.WriteLine($"NGettext.Wpf: Attempting to load \"{Path.GetFullPath(Path.Combine(Locale, cultureInfo.Name, LC_MESSAGES, DomainName + ".mo"))}\"");
            return new Catalog(DomainName, Locale, cultureInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !IsDisposed)
            {
                CultureTracker.CultureChanging -= OnCultureChangingResetCatalog;

                IsDisposed = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}