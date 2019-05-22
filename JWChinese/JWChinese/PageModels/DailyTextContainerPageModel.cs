using FreshMvvm;
using JWChinese.Helpers;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WolDownloader;
using Xamarin.Forms;

namespace JWChinese
{
    [AddINotifyPropertyChangedInterface]
    public class DailyTextContainerPageModel : FreshBasePageModel
    {
        public override async void Init(object initData)
        {
            base.Init(initData);
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
        }
    }
}
