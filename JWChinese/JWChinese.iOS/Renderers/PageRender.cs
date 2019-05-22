using CoreGraphics;
using Foundation;
using JWChinese.iOS;
using JWChinese;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.Diagnostics;
using System;

[assembly: ExportRenderer(typeof(CustomPage), typeof(PageRender))]

namespace JWChinese.iOS
{
    public class PageRender : PageRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (ParentViewController != null)
            {
                var page = Element as CustomPage;

                if (!string.IsNullOrEmpty(page.Subtitle))
                {
                    var titleString = new NSMutableAttributedString(page.Title + "\n", new UIStringAttributes() { ForegroundColor = UIColor.White, Font = UIFont.BoldSystemFontOfSize(17) });
                    var subtitleString = new NSAttributedString(page.Subtitle, new UIStringAttributes() { ForegroundColor = UIColor.White, Font = UIFont.SystemFontOfSize(12) });
                    titleString.Append(subtitleString);

                    var label = new UILabel(new CGRect(0, 0, titleString.Size.Width, 44));
                    label.Lines = 0;
                    label.TextAlignment = UITextAlignment.Center;
                    label.AttributedText = titleString;

                    ParentViewController.NavigationItem.TitleView = label;
                }
            }

            try
            {
                UIBarButtonItem backButton = new UIBarButtonItem();
                backButton.Title = "Back";
                NavigationController.NavigationBar.TopItem.BackBarButtonItem = backButton;
            }
            catch (Exception ex)
            {

            }
        }











        //let subtitleAttribute = [NSForegroundColorAttributeName: UIColor.gray, NSFontAttributeName: UIFont.systemFont(ofSize: 12.0)]
        //let titleString = NSMutableAttributedString(string: "title" + "\n", attributes: [NSFontAttributeName: UIFont.boldSystemFont(ofSize: 17.0)])
        //let subtitleString = NSAttributedString(string: "subtitle", attributes: subtitleAttribute)
        //titleString.append(subtitleString)

        //let label = UILabel(frame: CGRect(x: 0, y: 0, width: titleString.size().width, height: 44))
        //label.numberOfLines = 0
        //label.textAlignment = NSTextAlignment.center
        //label.attributedText = titleString
        //self.navigationItem.titleView = label


        //UILabel titleLabel;
        //UILabel subtitleLabel;
        //UIView containerView;
        //UIView titleView;
        //UIView marginView;
        //nfloat lastNavBarHeight = 0.0f;
        //nfloat lastNavBarWidth = 0.0f;

        //protected override void OnElementChanged(VisualElementChangedEventArgs e)
        //{
        //    base.OnElementChanged(e);

        //    Debug.WriteLine("Element");
        //}

        //public override void ViewWillAppear(bool animated)
        //{
        //    base.ViewWillAppear(animated);

        //    if(NavigationController != null)
        //    {
        //        SetupNavBar(NavigationController.NavigationBar.Bounds.Size);
        //        //SetTitlePosition(0, 0, new CGRect(0, -5, 0, 0));
        //        SetTitlePosition(0, 0, new CGRect(0, 0, Math.Max(subtitleLabel.IntrinsicContentSize.Width, titleLabel.IntrinsicContentSize.Width), (titleLabel.IntrinsicContentSize.Height + subtitleLabel.IntrinsicContentSize.Height + (subtitleLabel.IntrinsicContentSize.Height > 0.0f ? 3.0f : 0.0f))));
        //    }

        //    Debug.WriteLine("Preparing");
        //}

        //public override void ViewWillLayoutSubviews()
        //{
        //    base.ViewWillLayoutSubviews();
        //    Debug.WriteLine("SubViews");
        //}

        //public override void ViewDidLayoutSubviews()
        //{
        //    base.ViewDidLayoutSubviews();

        //    if (lastNavBarWidth != NavigationController?.NavigationBar?.Bounds.Size.Width || lastNavBarHeight != NavigationController?.NavigationBar?.Bounds.Size.Height)
        //    {
        //        lastNavBarHeight = NavigationController?.NavigationBar?.Bounds.Size.Height ?? 0.0f;
        //        lastNavBarWidth = NavigationController?.NavigationBar?.Bounds.Size.Width ?? 0.0f;
        //        SetupNavBar(new CGSize(lastNavBarWidth, lastNavBarHeight));
        //    }

        //    //SetTitlePosition(0, 0, new CGRect(0, -5, 0, 0));
        //    SetTitlePosition(0, 0, new CGRect(0, 0, Math.Max(subtitleLabel.IntrinsicContentSize.Width, titleLabel.IntrinsicContentSize.Width), (titleLabel.IntrinsicContentSize.Height + subtitleLabel.IntrinsicContentSize.Height + (subtitleLabel.IntrinsicContentSize.Height > 0.0f ? 3.0f : 0.0f))));

        //    Debug.WriteLine("didSubViews");
        //}

        //void SetupNavBar(CGSize size)
        //{
        //    if (NavigationController != null && titleView != null)
        //    {
        //        var page = Element as Page;
        //        containerView.Frame = new CGRect(0, 0, size.Width, size.Height);


        //        titleView.Layer.BorderWidth = 0;
        //        titleView.Layer.CornerRadius = 0;
        //        titleView.Layer.BorderColor = UIColor.Clear.CGColor;

        //        SetupTextFont(titleLabel);

        //        titleView.BackgroundColor = UIColor.Clear;

        //        ParentViewController.NavigationItem.TitleView = containerView;
        //        ParentViewController.NavigationItem.TitleView.SetNeedsDisplay();
        //    }
        //}

        //public override void ViewDidLoad()
        //{
        //    base.ViewDidLoad();

        //    containerView = new UIView()
        //    {
        //        AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth
        //    };

        //    titleView = new UIView()
        //    {

        //    };

        //    marginView = new UIView()
        //    {

        //    };

        //    titleLabel = new UILabel()
        //    {
        //        Text = Title,
        //        Font = UIFont.BoldSystemFontOfSize(20)
        //    };

        //    subtitleLabel = new UILabel()
        //    {
        //        Hidden = true,
        //        Font = UIFont.SystemFontOfSize(12)
        //    };

        //    titleView.Add(titleLabel);
        //    titleView.Add(subtitleLabel);
        //    marginView.Add(titleView);
        //    containerView.Add(marginView);

        //    Element.PropertyChanged += Element_PropertyChanged;
        //}
        //public override void ViewDidAppear(bool animated)
        //{
        //    base.ViewDidAppear(animated);
        //}

        //void SetTitlePosition(Thickness padding, Thickness margin, CGRect vFrame)
        //{
        //    var marginX = margin.Top;
        //    var marginY = margin.Left;
        //    var marginWidth = margin.Left + margin.Right;
        //    var marginHeight = margin.Top + margin.Bottom;
        //    var paddingWidth = padding.Left + padding.Right;
        //    var paddingHeight = padding.Top + padding.Bottom;
        //    var paddingX = padding.Left;
        //    var paddingY = padding.Top;

        //    marginView.Frame = new CGRect(vFrame.X, vFrame.Y, vFrame.Width, vFrame.Height);

        //    double offset = 0;

        //    titleLabel.AutoresizingMask = UIViewAutoresizing.All;

        //    // CENTER ALIGNMENT
        //    offset = marginX;
        //    marginView.Frame = new CGRect(marginView.Frame.X, marginView.Frame.Y, marginView.Bounds.Width + marginWidth + paddingWidth, marginView.Bounds.Height + marginHeight + paddingHeight);
        //    marginView.Center = marginView.Superview.Center;
        //    titleLabel.TextAlignment = UITextAlignment.Center;
        //    subtitleLabel.TextAlignment = UITextAlignment.Center;

        //    titleView.Frame = new CGRect(offset, vFrame.Y + marginY, vFrame.Width + paddingWidth, vFrame.Height + paddingHeight);

        //    var cPage = Element as CustomPage;
        //    if (cPage != null && (!string.IsNullOrEmpty(cPage.Subtitle) || (cPage.FormattedSubtitle != null && cPage.FormattedSubtitle.Spans.Count > 0)))
        //    {
        //        titleLabel.Frame = new CGRect(paddingX, paddingY, titleView.Frame.Width, titleLabel.IntrinsicContentSize.Height);
        //        subtitleLabel.Frame = new CGRect(titleLabel.Frame.X, titleLabel.Frame.Y + titleLabel.Frame.Height + 3, titleView.Frame.Width, subtitleLabel.Frame.Height);
        //    }
        //    else
        //    {
        //        titleLabel.Frame = new CGRect(paddingX, paddingY, titleLabel.IntrinsicContentSize.Width, titleLabel.IntrinsicContentSize.Height);
        //    }
        //}

        //public override void ViewWillTransitionToSize(CGSize toSize, IUIViewControllerTransitionCoordinator coordinator)
        //{
        //    base.ViewWillTransitionToSize(toSize, coordinator);

        //    SetupNavBar(new CGSize(NavigationController?.NavigationBar?.Bounds.Size.Width ?? 0.0f, NavigationController?.NavigationBar?.Bounds.Height ?? 0.0f));
        //}

        //public override void ViewDidDisappear(bool animated)
        //{
        //    base.ViewDidDisappear(animated);
        //}

        //void SetupShadow(bool hasShadow)
        //{
        //    if (hasShadow)
        //    {
        //        NavigationController.NavigationBar.Layer.ShadowColor = UIColor.Gray.CGColor;
        //        NavigationController.NavigationBar.Layer.ShadowOffset = new CGSize(0, 0);
        //        NavigationController.NavigationBar.Layer.ShadowOpacity = 1;
        //    }
        //    else
        //    {
        //        NavigationController.NavigationBar.Layer.ShadowColor = UIColor.Clear.CGColor;
        //        NavigationController.NavigationBar.Layer.ShadowOffset = new CGSize(0, 0);
        //        NavigationController.NavigationBar.Layer.ShadowOpacity = 0;
        //    }
        //}

        //void SetupBackground(UIImage image, float alpha)
        //{
        //    NavigationController.NavigationBar.SetBackgroundImage(image, UIBarMetrics.Default);
        //    NavigationController.NavigationBar.Alpha = alpha;
        //}

        //void SetupTextFont(UILabel label)
        //{

        //    var cPage = Element as CustomPage;
        //    if (cPage != null && cPage.FormattedTitle != null && cPage.FormattedTitle.Spans.Count > 0)
        //    {
        //        SetupFormattedText(titleLabel, cPage.FormattedTitle, cPage.Title);
        //    }
        //    else
        //    {
        //        SetupText(label, (Element as Page).Title);
        //    }

        //    if (cPage != null && cPage.FormattedSubtitle != null && cPage.FormattedSubtitle.Spans.Count > 0)
        //    {
        //        subtitleLabel.Hidden = false;
        //        SetupFormattedText(subtitleLabel, cPage.FormattedSubtitle, cPage.Subtitle);

        //    }
        //    else if (cPage != null && !string.IsNullOrEmpty(cPage.Subtitle))
        //    {
        //        subtitleLabel.Hidden = false;
        //        SetupText(subtitleLabel, cPage.Subtitle);

        //        subtitleLabel.SetNeedsDisplay();
        //    }
        //    else
        //    {
        //        subtitleLabel.Text = string.Empty;
        //        subtitleLabel.Frame = CGRect.Empty;
        //        subtitleLabel.Hidden = true;
        //    }


        //    label.SizeToFit();
        //    subtitleLabel.SizeToFit();
        //    titleView.SizeToFit();

        //}

        //void SetupTextColor(UILabel label, UIColor color)
        //{
        //    label.TextColor = color;
        //}

        //void SetupFormattedText(UILabel label, FormattedString formattedString, string defaulTitle)
        //{
        //    label.AttributedText = formattedString.ToAttributed(Font.Default, Xamarin.Forms.Color.Default);
        //    label.SetNeedsDisplay();
        //}

        //void SetupText(UILabel label, string text)
        //{

        //    if (!string.IsNullOrEmpty(text))
        //    {
        //        label.Text = text;
        //    }
        //    else
        //    {
        //        label.Text = string.Empty;
        //        label.AttributedText = new NSAttributedString();
        //    }
        //}

        //private void Element_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    var page = sender as Page;
        //    Debug.WriteLine(e.PropertyName);
        //    if (e.PropertyName == Page.TitleProperty.PropertyName || e.PropertyName == CustomPage.SubtitleProperty.PropertyName)
        //    {
        //        SetupTextFont(titleLabel);

        //        SetTitlePosition(0, 0, new CGRect(0, 0, Math.Max(subtitleLabel.IntrinsicContentSize.Width, titleLabel.IntrinsicContentSize.Width), (titleLabel.IntrinsicContentSize.Height + subtitleLabel.IntrinsicContentSize.Height + (subtitleLabel.IntrinsicContentSize.Height > 0.0f ? 3.0f : 0.0f))));
        //    }
        //    else if (e.PropertyName == CustomPage.FormattedTitleProperty.PropertyName && (page is CustomPage))
        //    {
        //        var cPage = page as CustomPage;
        //        SetupFormattedText(titleLabel, cPage.FormattedTitle, cPage.Title);
        //    }
        //}

        //public override void ViewDidUnload()
        //{
        //    base.ViewDidUnload();
        //    titleLabel = null;
        //    subtitleLabel = null;
        //    Element.PropertyChanged -= Element_PropertyChanged;
        //}
    }
}