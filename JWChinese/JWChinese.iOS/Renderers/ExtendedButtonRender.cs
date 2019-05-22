// ***********************************************************************
// Assembly         : XLabs.Forms.iOS
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

using JWChinese;
using JWChinese.iOS;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedButton), typeof(ExtendedButtonRender))]

namespace JWChinese.iOS
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

            var element = this.Element;

            if (element == null || this.Control == null)
            {
                return;
            }

            this.Control.VerticalAlignment = this.Element.VerticalContentAlignment.ToContentVerticalAlignment();
            this.Control.HorizontalAlignment = this.Element.HorizontalContentAlignment.ToContentHorizontalAlignment();

            // Set the padding
            if(Control.HorizontalAlignment != UIControlContentHorizontalAlignment.Center)
            {
                Control.ContentEdgeInsets = new UIEdgeInsets(0,10,0,0);
            }
        }

        /// <summary>
        /// Handles the <see cref="E:ElementPropertyChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "VerticalContentAlignment":
                    this.Control.VerticalAlignment = this.Element.VerticalContentAlignment.ToContentVerticalAlignment();
                    break;
                case "HorizontalContentAlignment":
                    this.Control.HorizontalAlignment = this.Element.HorizontalContentAlignment.ToContentHorizontalAlignment();
                    break;
                default:
                    base.OnElementPropertyChanged(sender, e);
                    break;
            }
        }

        /// <summary>
        /// Gets the element.
        /// </summary>
        /// <value>The element.</value>
        public new ExtendedButton Element
        {
            get
            {
                return base.Element as ExtendedButton;
            }
        }
    }
}