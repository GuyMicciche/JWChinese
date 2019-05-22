using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace JWChinese.UWP
{
    public static class Helper
    {
        public static Color GetColorFromHexa(string hexaColor)
        {
            return
                Windows.UI.Color.FromArgb(
                    255,
                    Convert.ToByte(hexaColor.Substring(1, 2), 16),
                    Convert.ToByte(hexaColor.Substring(3, 2), 16),
                    Convert.ToByte(hexaColor.Substring(5, 2), 16)
                );
        }

        public static SolidColorBrush GetBrushFromHexa(string hexaColor)
        {
            return new SolidColorBrush(GetColorFromHexa(hexaColor));
        }

        public static Brush ToBrush(this Xamarin.Forms.Color color)
        {
            return new SolidColorBrush(color.ToMediaColor());
        }

        public static Thickness ToWinPhone(this Xamarin.Forms.Thickness t)
        {
            return new Thickness(t.Left, t.Top, t.Right, t.Bottom);
        }

        /// <summary>
        /// Finds a parent of a given item on the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="child">A direct or indirect child of the queried item.</param>
        /// <returns>The first parent item that matches the submitted type parameter. 
        /// If not matching item can be found, a null reference is being returned.</returns>
        public static T FindVisualParent<T>(DependencyObject child)
          where T : DependencyObject
        {
            // get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            // we’ve reached the end of the tree
            if (parentObject == null) return null;

            // check if the parent matches the type we’re looking for
            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                // use recursion to proceed with next level
                return FindVisualParent<T>(parentObject);
            }
        }

        public static T FindChildControl<T>(this DependencyObject control, string ctrlName) where T : UIElement
        {
            int childNumber = VisualTreeHelper.GetChildrenCount(control);
            for (int i = 0; i < childNumber; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(control, i);
                FrameworkElement fe = child as FrameworkElement;
                // Not a framework element or is null
                if (fe == null) return default(T);


                if (child is T && fe.Name == ctrlName)
                {
                    // Found the control so return
                    return child as T;
                }
                else
                {
                    // Not found it - search children
                    T nextLevel = FindChildControl<T>(child, ctrlName);
                    if (nextLevel != null)
                        return nextLevel;
                }
            }
            return default(T);
        }

        public static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                Debug.WriteLine("child => " + child);
                Debug.WriteLine("Name => " + (child as FrameworkElement).Name);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public static childItemType FindVisualChild<childItemType>(DependencyObject obj, string name) where childItemType : FrameworkElement
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child is childItemType && ((FrameworkElement)child).Name == name)
                    return (childItemType)child;
                else
                {
                    childItemType childOfChild = FindVisualChild<childItemType>(child, name);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        public static FrameworkElement GetRootPage(this FrameworkElement control)
        {
            var p = control.Parent;
            //while (p != null) {
            //    var pp = (p as FrameworkElement).Parent;
            //    if (pp != null && !typeof(Page).IsInstanceOfType(pp)) {
            //        p = pp;
            //    } else
            //        break;
            //}
            //return p as FrameworkElement;

            while (p != null)
            {
                if (typeof(Page).IsInstanceOfType(p))
                {
                    break;
                }
                else
                    p = (p as FrameworkElement).Parent;
            }

            return p as FrameworkElement;
        }

        //public static FontFamily ToFontFamily(this string ff)
        //{
        //    //font file must in assets folder.
        //    ff = Regex.Replace(ff, "(^/Assets/)|(^Assets/)", "");

        //    // ff like : FontAwesome.otf
        //    // Full Path must like : Assets/Fonts/FontAwesome.otf#FontAwesome
        //    // font name must same as font file name
        //    var fontName = Path.GetFileNameWithoutExtension(ff);
        //    // not have prefix "/", if have preifx "/", Path.Combin will return fail path.
        //    string path = string.Format("Assets/{0}", ff);
        //    if (File.Exists(Path.Combine(AppContext.BaseDirectory, path)))
        //    {
        //        return new FontFamily(string.Format("/{0}#{1}", path, fontName));
        //    }
        //    else
        //        return FontFamily.XamlAutoFontFamily;
        //}
    }
}
