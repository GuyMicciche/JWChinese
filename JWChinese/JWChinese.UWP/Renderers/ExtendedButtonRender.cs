// ***********************************************************************
// Assembly         : XLabs.Forms.Droid
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ExtendedButtonRenderer.cs" company="XLabs Team">
//     Copyright (c) XLabs Team. All rights reserved.
// </copyright>
// <summary>
//       This project is licensed under the Apache 2.0 license
//       https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
//       
//       XLabs is a open source project that aims to provide a powerfull and cross 
//       platform set of controls tailored to work with Xamarin Forms.
// </summary>
// ***********************************************************************
// 

using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using JWChinese.UWP.Renderers;
using JWChinese;

[assembly: ExportRenderer(typeof(ExtendedButton), typeof(ExtendedButtonRender))]

namespace JWChinese.UWP.Renderers
{
    /// <summary>
    /// Class ExtendedButtonRenderer.
    /// </summary>
    public class ExtendedButtonRender : ButtonRenderer
    {
        /// <summary>
        /// Called when [element changed].
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            UpdateAlignment();
            UpdateFont();
        }

        /// <summary>
        /// Handles the <see cref="E:ElementPropertyChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == ExtendedButton.VerticalContentAlignmentProperty.PropertyName ||
                e.PropertyName == ExtendedButton.HorizontalContentAlignmentProperty.PropertyName)
            {
                UpdateAlignment();
            }
            else if (e.PropertyName == Button.FontProperty.PropertyName)
            {
                UpdateFont();
            }

            this.Control.Style = (global::Windows.UI.Xaml.Style)global::Windows.UI.Xaml.Application.Current.Resources["CustomButtonStyle"];

            if (((ExtendedButton)sender).IsNumber)
            {
                Control.Padding = new Windows.UI.Xaml.Thickness(0);
            }

            base.OnElementPropertyChanged(sender, e);
        }

        /// <summary>
        /// Updates the font
        /// </summary>
        private void UpdateFont()
        {
            //Control.Typeface = Element.Font.ToExtendedTypeface(Context);
        }

        /// <summary>
        /// Sets the alignment.
        /// </summary>
        private void UpdateAlignment()
        {
            var element = this.Element as ExtendedButton;

            if (element == null || this.Control == null)
            {
                return;
            }
            //this.Control.Padding = new global::Windows.UI.Xaml.Thickness(10,0,0,0);
            this.Control.HorizontalContentAlignment = element.HorizontalContentAlignment.ToContentHorizontalAlignment();
            this.Control.VerticalContentAlignment = element.VerticalContentAlignment.ToContentVerticalAlignment();
        }
    }
}