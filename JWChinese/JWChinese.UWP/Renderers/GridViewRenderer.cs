using JWChinese.UWP.Renderers;
using System;
using Xamarin.Forms.Platform.UWP;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(JWChinese.GridView), typeof(GridViewRenderer))]
namespace JWChinese.UWP.Renderers
{
    public class GridViewRenderer : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);
            var list = e.NewElement;

            if (Control != null)
            {
                var baseList = Control as ListViewBase;

                //Retrieve the min item width property.
                var itemWidth = (double)list.GetValue(JWChinese.GridView.MinItemWidthProperty);

                //If the property is set.
                if (itemWidth > 0)
                {
                    //Build the new items panel template.
                    string template =
                    "<ItemsPanelTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\">" +
                        "<ItemsWrapGrid VerticalAlignment = \"Top\" ItemWidth = \"" + itemWidth + "\" Orientation = \"Horizontal\"/>" +
                    "</ItemsPanelTemplate> ";

                    baseList.ItemsPanel = (ItemsPanelTemplate)XamlReader.Load(template);

                    var padding = (double)list.GetValue(JWChinese.GridView.PaddingProperty);
                    baseList.Padding = new Windows.UI.Xaml.Thickness(padding, 0, padding, padding);
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
          
            //If the property is width.
            if (e.PropertyName == "Width" || e.PropertyName == "Padding")
            {
                //Unbox the xamarin host control.
                var list = sender as Xamarin.Forms.ListView;

                //Get the Minimum item size.
                var itemMinSize = (double)list.GetValue(JWChinese.GridView.MinItemWidthProperty);

                var padding = (double)list.GetValue(JWChinese.GridView.PaddingProperty);

                //If the property is set.
                if (itemMinSize > 0)
                {
                    //Unbox the native control.
                    var itemsControl = Control as ListViewBase;

                    //Retrieve items panel.
                    ItemsWrapGrid itemsPanel = itemsControl.ItemsPanelRoot as ItemsWrapGrid;

                    //If the items panel is a wrap grid.
                    if (itemsPanel != null)
                    {
                        //Get total size (leave room for scrolling.) SUBTRACT 10 IF INCLUDING SCROLLER
                        var total = list.Width - (padding * 2);

                        //How many items can be fit whole.
                        var canBeFit = Math.Floor(total / itemMinSize);

                        //Set the items Panel item width appropriately.
                        //Note you will need your container to stretch
                        //along with the items panel or it will look 
                        //strange. 
                        itemsPanel.ItemWidth = total / canBeFit;

                        itemsControl.Padding = new Windows.UI.Xaml.Thickness(padding, 0, padding, padding);
                    }
                }
            }
        }
    }
}