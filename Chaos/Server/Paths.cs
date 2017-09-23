﻿using System;

namespace Chaos
{
    static class Paths
    {
        internal static string Dir =>
#if DEBUG
            @"C:\Users\mewbb\Desktop\chaosmap\";
#else
            AppDomain.CurrentDomain.BaseDirectory;
#endif
        internal static string LogFiles => $@"{Dir}logs\";
        internal static string MetaFiles => $@"{Dir}metafiles\";
        internal static string MapFiles => $@"{Dir}maps\";
    }
}
