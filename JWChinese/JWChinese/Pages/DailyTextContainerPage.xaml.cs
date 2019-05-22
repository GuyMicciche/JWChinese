using FreshMvvm;

namespace JWChinese
{
    public partial class DailyTextContainerPage : FreshTabbedFONavigationContainer
    {
        public DailyTextContainerPage(string titleOfFirstTab) : base(titleOfFirstTab)
        {
            InitializeComponent();
        }

        public DailyTextContainerPage(string titleOfFirstTab, string navigationServiceName) : base(titleOfFirstTab, navigationServiceName)
        {
            InitializeComponent();
        }
    }
}