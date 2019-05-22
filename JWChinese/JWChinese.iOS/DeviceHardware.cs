using System;
using System.Runtime.InteropServices;

namespace JWChinese.iOS
{
    class DeviceHardware
    {
        public enum IOSHardware
        {
            iPhone,
            iPhone3G,
            iPhone3GS,
            iPhone4,
            iPhone4RevA,
            iPhone4CDMA,
            iPhone4S,
            iPhone5GSM,
            iPhone5CDMAGSM,
            İPhone6,
            İPhone6Plus,
            İPhone6S,
            İPhone6SPlus,
            İPhoneSE,
            unknown
        }

        public const string HardwareProperty = "hw.machine";
        [DllImport(ObjCRuntime.Constants.SystemLibrary)]
        static internal extern int sysctlbyname([MarshalAs(UnmanagedType.LPStr)] string property, IntPtr output, IntPtr oldLen, IntPtr newp, uint newlen);

        public static IOSHardware Version
        {
            get
            {
                var pLen = Marshal.AllocHGlobal(sizeof(int));
                sysctlbyname(DeviceHardware.HardwareProperty, IntPtr.Zero, pLen, IntPtr.Zero, 0);

                var length = Marshal.ReadInt32(pLen);

                var pStr = Marshal.AllocHGlobal(length);
                sysctlbyname(DeviceHardware.HardwareProperty, pStr, pLen, IntPtr.Zero, 0);

                var hardwareStr = Marshal.PtrToStringAnsi(pStr);

                Marshal.FreeHGlobal(pLen);
                Marshal.FreeHGlobal(pStr);

                if (hardwareStr == "iPhone1,1") return IOSHardware.iPhone;
                if (hardwareStr == "iPhone1,2") return IOSHardware.iPhone3G;
                if (hardwareStr == "iPhone2,1") return IOSHardware.iPhone3GS;
                if (hardwareStr == "iPhone3,1") return IOSHardware.iPhone4;
                if (hardwareStr == "iPhone3,2") return IOSHardware.iPhone4RevA;
                if (hardwareStr == "iPhone3,3") return IOSHardware.iPhone4CDMA;
                if (hardwareStr == "iPhone4,1") return IOSHardware.iPhone4S;
                if (hardwareStr == "iPhone5,1") return IOSHardware.iPhone5GSM;
                if (hardwareStr == "iPhone5,2") return IOSHardware.iPhone5CDMAGSM;
                if (hardwareStr == "iPhone7,2") return IOSHardware.İPhone6;
                if (hardwareStr == "iPhone8,1") return IOSHardware.İPhone6S;
                if (hardwareStr == "iPhone8,2") return IOSHardware.İPhone6SPlus;
                if (hardwareStr == "iPhone7,1") return IOSHardware.İPhone6Plus;
                if (hardwareStr == "iPhone8,4") return IOSHardware.İPhoneSE;

                return IOSHardware.unknown;
            }
        }
    }
}