using System;
using FreshMvvm;
using PropertyChanged;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace JWChinese
{
    [AddINotifyPropertyChangedInterface]
    public class TestPageModel: FreshBasePageModel
    {
        public ObservableCollection<string> Articles { get; set; }

        public TestPageModel()
        {
            
        }

        public override void Init(object initData)
        {
            Articles = new ObservableCollection<string>{
              "mono",
              "monodroid",
              "monotouch",
              "monorail",
              "monodevelop",
              "monotone",
              "monopoly",
              "monomodal",
              "mononucleosis"
            };
        }
    }
}