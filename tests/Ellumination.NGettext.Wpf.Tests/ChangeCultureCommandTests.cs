using System.Windows.Input;

namespace NGettext.Wpf
{
    using NSubstitute;
    using Xunit;

    /// <summary>
    /// 
    /// </summary>
    public class ChangeCultureCommandTests
    {
        //
        private readonly ChangeCultureCommand _target;

        /// <summary>
        /// 
        /// </summary>
        private readonly ICultureTracker _cultureTracker = Substitute.For<ICultureTracker>();

        /// <summary>
        /// 
        /// </summary>
        public ChangeCultureCommandTests()
        {
            _target = new ChangeCultureCommand();
            ChangeCultureCommand.CultureTracker = _cultureTracker;
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void Implements_ICommand()
        {
            Xunit.Assert.IsAssignableFrom<ICommand>(_target);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="culture"></param>
        [Theory
            , InlineData("da-DK")
            , InlineData("en-US")]
        public void CanExecute_If_Parameter_Is_Supported_Culture(string culture)
        {
            Xunit.Assert.True(_target.CanExecute(culture));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="culture"></param>
        [Theory
            , InlineData("bad culture")]
        public void Cannot_Execute_If_Parameter_Is_Not_Supported_Culture(string culture)
        {
            Xunit.Assert.False(_target.CanExecute(culture));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="culture"></param>
        [Theory
            , InlineData("da-DK")
            , InlineData("en-US")]
        public void Execute_Sets_CurrentCulture_From_Parameter(string culture)
        {
            _target.Execute(culture);
            Xunit.Assert.Equal(culture, _cultureTracker.CurrentCulture.Name);
        }
    }
}