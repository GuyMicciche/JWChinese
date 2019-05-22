using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWChinese.Objects
{
    public enum Rotation
    {
        Rotation0,
        Rotation90,
        Rotation180,
        Rotation270
    }
    public static class Orientation
    {
        public static int Width;
        public static int Height;
        private static Rotation rotation;
        public static EventHandler RotationChanged;
        public static Rotation Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                if (Rotation != value)
                {
                    rotation = value;
                    OnRotationChanged();
                }
            }
        }

        public static bool IsLandscape
        {
            get
            {
                return Rotation == Rotation.Rotation90
                    || Rotation == Rotation.Rotation270;
            }
        }

        public static bool Landscape { get; set; }

        private static void OnRotationChanged()
        {
            RotationChanged?.Invoke(null, EventArgs.Empty);
        }
    }
}
