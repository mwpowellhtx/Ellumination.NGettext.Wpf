using System;
using System.ComponentModel;
using System.Linq;

// TODO: what is the point of this module? it overrides the Xunit Assert? is that a good thing to be doing?
namespace NGettext.Wpf
{
    using NSubstitute;
    using Xunit;

    // TODO: huh, interesting we can do this
    // TODO: I did not think partial classes were possible, especially across assembly boundaries
    // TODO: which is causing a CS0436 'warning' ('/error'), probably justifiably so
    /// <summary>
    /// 
    /// </summary>
    public partial class Assert
    {
        /// <summary>
        /// Validates that the <paramref name="testCode"/> evaluates depending on
        /// <see cref="ArgumentNullException.ParamName"/> <paramref name="paramName"/>.
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="testCode"></param>
        public static void NotNullParameter(string paramName, Action testCode)
        {
            var e = Xunit.Assert.Throws<ArgumentNullException>(testCode);
            Xunit.Assert.Equal(paramName, e.ParamName);
        }

        //// TODO: all the rest of these are sufficient in the default Xunit.Assert implementations
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="action"></param>
        //public static void DoesNotThrow(Action action)
        //{
        //    var e = Record.Exception(action);
        //    Null(e);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <typeparam name="TInnerException"></typeparam>
        ///// <param name="paramName"></param>
        ///// <param name="action"></param>
        ///// <returns></returns>
        //public static TInnerException ThrowsArgumentException<TInnerException>(string paramName, Action action)
        //{
        //    var e = Throws<ArgumentException>(action);
        //    Equal(paramName, e.ParamName);
        //    return IsAssignableFrom<TInnerException>(e.InnerException);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="propertyName"></param>
        ///// <param name="target"></param>
        ///// <param name="action"></param>
        ///// <param name="validationErrorPredicate"></param>
        //public static void NotifiesDataError(string propertyName, INotifyDataErrorInfo target, Action action,
        //    Predicate<object> validationErrorPredicate)
        //{
        //    var listener = Substitute.For<EventHandler<DataErrorsChangedEventArgs>>();
        //    listener.WhenForAnyArgs(l => l.Invoke(null, null)).Do(
        //        callInfo =>
        //        {
        //            if (callInfo.Arg<DataErrorsChangedEventArgs>().PropertyName == propertyName)
        //            {
        //                Contains(
        //                    callInfo.Arg<INotifyDataErrorInfo>().GetErrors(propertyName).Cast<object>(),
        //                    validationErrorPredicate);
        //            }
        //        });
        //    target.ErrorsChanged += listener;
        //    action.Invoke();
        //    True(target.HasErrors);
        //    target.ErrorsChanged -= listener;
        //    listener.Received()
        //        .Invoke(Arg.Is(target), Arg.Is<DataErrorsChangedEventArgs>(args => args.PropertyName == propertyName));
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="propertyName"></param>
        ///// <param name="target"></param>
        ///// <param name="action"></param>
        //public static void NoDataError(string propertyName, INotifyDataErrorInfo target, Action action)
        //{
        //    var listener = Substitute.For<EventHandler<DataErrorsChangedEventArgs>>();
        //    target.ErrorsChanged += listener;
        //    action.Invoke();
        //    target.ErrorsChanged -= listener;
        //    listener.DidNotReceive()
        //        .Invoke(Arg.Is(target), Arg.Is<DataErrorsChangedEventArgs>(args => args.PropertyName == propertyName));
        //    Empty(target.GetErrors(propertyName));
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="propertyName"></param>
        ///// <param name="target"></param>
        ///// <param name="action"></param>
        //public static void NotifiesNoDataError(string propertyName, INotifyDataErrorInfo target, Action action)
        //{
        //    var listener = Substitute.For<EventHandler<DataErrorsChangedEventArgs>>();
        //    target.ErrorsChanged += listener;
        //    action.Invoke();
        //    target.ErrorsChanged -= listener;
        //    listener.Received()
        //        .Invoke(Arg.Is(target), Arg.Is<DataErrorsChangedEventArgs>(args => args.PropertyName == propertyName));
        //    Empty(target.GetErrors(propertyName));
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="target"></param>
        ///// <param name="propertyName"></param>
        ///// <param name="action"></param>
        //public static void NotifiesPropertyChanged(INotifyPropertyChanged target, string propertyName, Action action)
        //{
        //    var listener = Substitute.For<PropertyChangedEventHandler>();
        //    target.PropertyChanged += listener;
        //    try
        //    {
        //        action.Invoke();
        //    }
        //    finally
        //    {
        //        target.PropertyChanged -= listener;
        //    }
        //    listener.Received()
        //        .Invoke(Arg.Is(target), Arg.Is<PropertyChangedEventArgs>(e => e.PropertyName == propertyName));
        //}
    }
}