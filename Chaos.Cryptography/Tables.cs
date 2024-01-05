﻿using System.Collections.Immutable;

namespace Chaos.Cryptography;

/// <summary>
///     Holds CheckSum and KeySalt tables.
/// </summary>
public static class Tables
{
    //@formatter:off
    /// <summary>///     ///     ///     ///     ///     ///     16-bit CRC table (CRC-16/CCITT-FALSE) (0x1021)////////////////// </summary>
    public static readonly ImmutableArray<ushort> TABLE16 = new ushort[]
    {
        0, 4129, 8258, 12387, 16516, 20645, 24774, 28903, 33032, 37161, 41290, 45419, 49548, 53677, 57806, 61935, 4657, 528, 12915,
        8786, 21173, 17044, 29431, 25302, 37689, 33560, 45947, 41818, 54205, 50076, 62463, 58334, 9314, 13379, 1056, 5121, 25830,
        29895, 17572, 21637, 42346, 46411, 34088, 38153, 58862, 62927, 50604, 54669, 13907, 9842, 5649, 1584, 30423, 26358, 22165,
        18100, 46939, 42874, 38681, 34616, 63455, 59390, 55197, 51132, 18628, 22757, 26758, 30887, 2112, 6241, 10242, 14371, 51660,
        55789, 59790, 63919, 35144, 39273, 43274, 47403, 23285, 19156, 31415, 27286, 6769, 2640, 14899, 10770, 56317, 52188, 64447,
        60318, 39801, 35672, 47931, 43802, 27814, 31879, 19684, 23749, 11298, 15363, 3168, 7233, 60846, 64911, 52716, 56781, 44330,
        48395, 36200, 40265, 32407, 28342, 24277, 20212, 15891, 11826, 7761, 3696, 65439, 61374, 57309, 53244, 48923, 44858, 40793,
        36728, 37256, 33193, 45514, 41451, 53516, 49453, 61774, 57711, 4224, 161, 12482, 8419, 20484, 16421, 28742, 24679, 33721,
        37784, 41979, 46042, 49981, 54044, 58239, 62302, 689, 4752, 8947, 13010, 16949, 21012, 25207, 29270, 46570, 42443, 38312,
        34185, 62830, 58703, 54572, 50445, 13538, 9411, 5280, 1153, 29798, 25671, 21540, 17413, 42971, 47098, 34713, 38840, 59231,
        63358, 50973, 55100, 9939, 14066, 1681, 5808, 26199, 30326, 17941, 22068, 55628, 51565, 63758, 59695, 39368, 35305, 47498,
        43435, 22596, 18533, 30726, 26663, 6336, 2273, 14466, 10403, 52093, 56156, 60223, 64286, 35833, 39896, 43963, 48026, 19061,
        23124, 27191, 31254, 2801, 6864, 10931, 14994, 64814, 60687, 56684, 52557, 48554, 44427, 40424, 36297, 31782, 27655, 23652,
        19525, 15522, 11395, 7392, 3265, 61215, 65342, 53085, 57212, 44955, 49082, 36825, 40952, 28183, 32310, 20053, 24180, 11923,
        16050, 3793, 7920
    }.ToImmutableArray();

    /// <summary>///     ///     ///     ///     ///     ///     32-bit CRC table (CRC-32/adler) (0xEDB88320)////////////////// </summary>
    public static readonly ImmutableArray<uint> TABLE32 = new[]
    {
        0U, 1996959894U, 3993919788U, 2567524794U, 124634137U, 1886057615U, 3915621685U, 2657392035U, 249268274U, 2044508324U,
        3772115230U, 2547177864U, 162941995U, 2125561021U, 3887607047U, 2428444049U, 498536548U, 1789927666U, 4089016648U,
        2227061214U, 450548861U, 1843258603U, 4107580753U, 2211677639U, 325883990U, 1684777152U, 4251122042U, 2321926636U,
        335633487U, 1661365465U, 4195302755U, 2366115317U, 997073096U, 1281953886U, 3579855332U, 2724688242U, 1006888145U,
        1258607687U, 3524101629U, 2768942443U, 901097722U, 1119000684U, 3686517206U, 2898065728U, 853044451U, 1172266101U,
        3705015759U, 2882616665U, 651767980U, 1373503546U, 3369554304U, 3218104598U, 565507253U, 1454621731U, 3485111705U,
        3099436303U, 671266974U, 1594198024U, 3322730930U, 2970347812U, 795835527U, 1483230225U, 3244367275U, 3060149565U,
        1994146192U, 31158534U, 2563907772U, 4023717930U, 1907459465U, 112637215U, 2680153253U, 3904427059U, 2013776290U,
        251722036U, 2517215374U, 3775830040U, 2137656763U, 141376813U, 2439277719U, 3865271297U, 1802195444U, 476864866U,
        2238001368U, 4066508878U, 1812370925U, 453092731U, 2181625025U, 4111451223U, 1706088902U, 314042704U, 2344532202U,
        4240017532U, 1658658271U, 366619977U, 2362670323U, 4224994405U, 1303535960U, 984961486U, 2747007092U, 3569037538U,
        1256170817U, 1037604311U, 2765210733U, 3554079995U, 1131014506U, 879679996U, 2909243462U, 3663771856U, 1141124467U,
        855842277U, 2852801631U, 3708648649U, 1342533948U, 654459306U, 3188396048U, 3373015174U, 1466479909U, 544179635U,
        3110523913U, 3462522015U, 1591671054U, 702138776U, 2966460450U, 3352799412U, 1504918807U, 783551873U, 3082640443U,
        3233442989U, 3988292384U, 2596254646U, 62317068U, 1957810842U, 3939845945U, 2647816111U, 81470997U, 1943803523U,
        3814918930U, 2489596804U, 225274430U, 2053790376U, 3826175755U, 2466906013U, 167816743U, 2097651377U, 4027552580U,
        2265490386U, 503444072U, 1762050814U, 4150417245U, 2154129355U, 426522225U, 1852507879U, 4275313526U, 2312317920U,
        282753626U, 1742555852U, 4189708143U, 2394877945U, 397917763U, 1622183637U, 3604390888U, 2714866558U, 953729732U,
        1340076626U, 3518719985U, 2797360999U, 1068828381U, 1219638859U, 3624741850U, 2936675148U, 906185462U, 1090812512U,
        3747672003U, 2825379669U, 829329135U, 1181335161U, 3412177804U, 3160834842U, 628085408U, 1382605366U, 3423369109U,
        3138078467U, 570562233U, 1426400815U, 3317316542U, 2998733608U, 733239954U, 1555261956U, 3268935591U, 3050360625U,
        752459403U, 1541320221U, 2607071920U, 3965973030U, 1969922972U, 40735498U, 2617837225U, 3943577151U, 1913087877U, 83908371U,
        2512341634U, 3803740692U, 2075208622U, 213261112U, 2463272603U, 3855990285U, 2094854071U, 198958881U, 2262029012U,
        4057260610U, 1759359992U, 534414190U, 2176718541U, 4139329115U, 1873836001U, 414664567U, 2282248934U, 4279200368U,
        1711684554U, 285281116U, 2405801727U, 4167216745U, 1634467795U, 376229701U, 2685067896U, 3608007406U, 1308918612U,
        956543938U, 2808555105U, 3495958263U, 1231636301U, 1047427035U, 2932959818U, 3654703836U, 1088359270U, 936918000U,
        2847714899U, 3736837829U, 1202900863U, 817233897U, 3183342108U, 3401237130U, 1404277552U, 615818150U, 3134207493U,
        3453421203U, 1423857449U, 601450431U, 3009837614U, 3294710456U, 1567103746U, 711928724U, 3020668471U, 3272380065U,
        1510334235U, 755167117U
    }.ToImmutableArray();

    
    internal static readonly ImmutableArray<ImmutableArray<byte>> SALT_TABLE = new[]
    {
        new byte[]
        {
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59,
            60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88,
            89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113,
            114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136,
            137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159,
            160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182,
            183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205,
            206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228,
            229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251,
            252, 253, 254, 255
        }.ToImmutableArray(),
        new byte[]
        {
            128, 127, 129, 126, 130, 125, 131, 124, 132, 123, 133, 122, 134, 121, 135, 120, 136, 119, 137, 118, 138, 117, 139,
            116, 140, 115, 141, 114, 142, 113, 143, 112, 144, 111, 145, 110, 146, 109, 147, 108, 148, 107, 149, 106, 150, 105,
            151, 104, 152, 103, 153, 102, 154, 101, 155, 100, 156, 99, 157, 98, 158, 97, 159, 96, 160, 95, 161, 94, 162, 93,
            163, 92, 164, 91, 165, 90, 166, 89, 167, 88, 168, 87, 169, 86, 170, 85, 171, 84, 172, 83, 173, 82, 174, 81, 175, 80,
            176, 79, 177, 78, 178, 77, 179, 76, 180, 75, 181, 74, 182, 73, 183, 72, 184, 71, 185, 70, 186, 69, 187, 68, 188, 67,
            189, 66, 190, 65, 191, 64, 192, 63, 193, 62, 194, 61, 195, 60, 196, 59, 197, 58, 198, 57, 199, 56, 200, 55, 201, 54,
            202, 53, 203, 52, 204, 51, 205, 50, 206, 49, 207, 48, 208, 47, 209, 46, 210, 45, 211, 44, 212, 43, 213, 42, 214, 41,
            215, 40, 216, 39, 217, 38, 218, 37, 219, 36, 220, 35, 221, 34, 222, 33, 223, 32, 224, 31, 225, 30, 226, 29, 227, 28,
            228, 27, 229, 26, 230, 25, 231, 24, 232, 23, 233, 22, 234, 21, 235, 20, 236, 19, 237, 18, 238, 17, 239, 16, 240, 15,
            241, 14, 242, 13, 243, 12, 244, 11, 245, 10, 246, 9, 247, 8, 248, 7, 249, 6, 250, 5, 251, 4, 252, 3, 253, 2, 254, 1,
            255, 0
        }.ToImmutableArray(),
        new byte[]
        {
            255, 254, 253, 252, 251, 250, 249, 248, 247, 246, 245, 244, 243, 242, 241, 240, 239, 238, 237, 236, 235, 234, 233,
            232, 231, 230, 229, 228, 227, 226, 225, 224, 223, 222, 221, 220, 219, 218, 217, 216, 215, 214, 213, 212, 211, 210,
            209, 208, 207, 206, 205, 204, 203, 202, 201, 200, 199, 198, 197, 196, 195, 194, 193, 192, 191, 190, 189, 188, 187,
            186, 185, 184, 183, 182, 181, 180, 179, 178, 177, 176, 175, 174, 173, 172, 171, 170, 169, 168, 167, 166, 165, 164,
            163, 162, 161, 160, 159, 158, 157, 156, 155, 154, 153, 152, 151, 150, 149, 148, 147, 146, 145, 144, 143, 142, 141,
            140, 139, 138, 137, 136, 135, 134, 133, 132, 131, 130, 129, 128, 127, 126, 125, 124, 123, 122, 121, 120, 119, 118,
            117, 116, 115, 114, 113, 112, 111, 110, 109, 108, 107, 106, 105, 104, 103, 102, 101, 100, 99, 98, 97, 96, 95, 94,
            93, 92, 91, 90, 89, 88, 87, 86, 85, 84, 83, 82, 81, 80, 79, 78, 77, 76, 75, 74, 73, 72, 71, 70, 69, 68, 67, 66, 65,
            64, 63, 62, 61, 60, 59, 58, 57, 56, 55, 54, 53, 52, 51, 50, 49, 48, 47, 46, 45, 44, 43, 42, 41, 40, 39, 38, 37, 36,
            35, 34, 33, 32, 31, 30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6,
            5, 4, 3, 2, 1, 0
        }.ToImmutableArray(),
        new byte[]
        {
            255, 1, 254, 2, 253, 3, 252, 4, 251, 5, 250, 6, 249, 7, 248, 8, 247, 9, 246, 10, 245, 11, 244, 12, 243, 13, 242, 14,
            241, 15, 240, 16, 239, 17, 238, 18, 237, 19, 236, 20, 235, 21, 234, 22, 233, 23, 232, 24, 231, 25, 230, 26, 229, 27,
            228, 28, 227, 29, 226, 30, 225, 31, 224, 32, 223, 33, 222, 34, 221, 35, 220, 36, 219, 37, 218, 38, 217, 39, 216, 40,
            215, 41, 214, 42, 213, 43, 212, 44, 211, 45, 210, 46, 209, 47, 208, 48, 207, 49, 206, 50, 205, 51, 204, 52, 203, 53,
            202, 54, 201, 55, 200, 56, 199, 57, 198, 58, 197, 59, 196, 60, 195, 61, 194, 62, 193, 63, 192, 64, 191, 65, 190, 66,
            189, 67, 188, 68, 187, 69, 186, 70, 185, 71, 184, 72, 183, 73, 182, 74, 181, 75, 180, 76, 179, 77, 178, 78, 177, 79,
            176, 80, 175, 81, 174, 82, 173, 83, 172, 84, 171, 85, 170, 86, 169, 87, 168, 88, 167, 89, 166, 90, 165, 91, 164, 92,
            163, 93, 162, 94, 161, 95, 160, 96, 159, 97, 158, 98, 157, 99, 156, 100, 155, 101, 154, 102, 153, 103, 152, 104,
            151, 105, 150, 106, 149, 107, 148, 108, 147, 109, 146, 110, 145, 111, 144, 112, 143, 113, 142, 114, 141, 115, 140,
            116, 139, 117, 138, 118, 137, 119, 136, 120, 135, 121, 134, 122, 133, 123, 132, 124, 131, 125, 130, 126, 129, 127,
            128, 128
        }.ToImmutableArray(),
        new byte[]
        {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 4, 4, 4, 4, 4, 4,
            4, 4, 4, 4, 4, 4, 4, 4, 4, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16,
            16, 16, 16, 16, 16, 16, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 36, 36, 36, 36, 36, 36, 36,
            36, 36, 36, 36, 36, 36, 36, 36, 36, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 64, 64, 64, 64,
            64, 64, 64, 64, 64, 64, 64, 64, 64, 64, 64, 64, 81, 81, 81, 81, 81, 81, 81, 81, 81, 81, 81, 81, 81, 81, 81, 81, 100,
            100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 121, 121, 121, 121, 121, 121, 121, 121,
            121, 121, 121, 121, 121, 121, 121, 121, 144, 144, 144, 144, 144, 144, 144, 144, 144, 144, 144, 144, 144, 144, 144,
            144, 169, 169, 169, 169, 169, 169, 169, 169, 169, 169, 169, 169, 169, 169, 169, 169, 196, 196, 196, 196, 196, 196,
            196, 196, 196, 196, 196, 196, 196, 196, 196, 196, 225, 225, 225, 225, 225, 225, 225, 225, 225, 225, 225, 225, 225,
            225, 225, 225
        }.ToImmutableArray(),
        new byte[]
        {
            0, 2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32, 34, 36, 38, 40, 42, 44, 46, 48, 50, 52, 54, 56, 58,
            60, 62, 64, 66, 68, 70, 72, 74, 76, 78, 80, 82, 84, 86, 88, 90, 92, 94, 96, 98, 100, 102, 104, 106, 108, 110, 112,
            114, 116, 118, 120, 122, 124, 126, 128, 130, 132, 134, 136, 138, 140, 142, 144, 146, 148, 150, 152, 154, 156, 158,
            160, 162, 164, 166, 168, 170, 172, 174, 176, 178, 180, 182, 184, 186, 188, 190, 192, 194, 196, 198, 200, 202, 204,
            206, 208, 210, 212, 214, 216, 218, 220, 222, 224, 226, 228, 230, 232, 234, 236, 238, 240, 242, 244, 246, 248, 250,
            252, 254, 0, 2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32, 34, 36, 38, 40, 42, 44, 46, 48, 50, 52, 54,
            56, 58, 60, 62, 64, 66, 68, 70, 72, 74, 76, 78, 80, 82, 84, 86, 88, 90, 92, 94, 96, 98, 100, 102, 104, 106, 108,
            110, 112, 114, 116, 118, 120, 122, 124, 126, 128, 130, 132, 134, 136, 138, 140, 142, 144, 146, 148, 150, 152, 154,
            156, 158, 160, 162, 164, 166, 168, 170, 172, 174, 176, 178, 180, 182, 184, 186, 188, 190, 192, 194, 196, 198, 200,
            202, 204, 206, 208, 210, 212, 214, 216, 218, 220, 222, 224, 226, 228, 230, 232, 234, 236, 238, 240, 242, 244, 246,
            248, 250, 252, 254
        }.ToImmutableArray(),
        new byte[]
        {
            255, 253, 251, 249, 247, 245, 243, 241, 239, 237, 235, 233, 231, 229, 227, 225, 223, 221, 219, 217, 215, 213, 211,
            209, 207, 205, 203, 201, 199, 197, 195, 193, 191, 189, 187, 185, 183, 181, 179, 177, 175, 173, 171, 169, 167, 165,
            163, 161, 159, 157, 155, 153, 151, 149, 147, 145, 143, 141, 139, 137, 135, 133, 131, 129, 127, 125, 123, 121, 119,
            117, 115, 113, 111, 109, 107, 105, 103, 101, 99, 97, 95, 93, 91, 89, 87, 85, 83, 81, 79, 77, 75, 73, 71, 69, 67, 65,
            63, 61, 59, 57, 55, 53, 51, 49, 47, 45, 43, 41, 39, 37, 35, 33, 31, 29, 27, 25, 23, 21, 19, 17, 15, 13, 11, 9, 7, 5,
            3, 1, 255, 253, 251, 249, 247, 245, 243, 241, 239, 237, 235, 233, 231, 229, 227, 225, 223, 221, 219, 217, 215, 213,
            211, 209, 207, 205, 203, 201, 199, 197, 195, 193, 191, 189, 187, 185, 183, 181, 179, 177, 175, 173, 171, 169, 167,
            165, 163, 161, 159, 157, 155, 153, 151, 149, 147, 145, 143, 141, 139, 137, 135, 133, 131, 129, 127, 125, 123, 121,
            119, 117, 115, 113, 111, 109, 107, 105, 103, 101, 99, 97, 95, 93, 91, 89, 87, 85, 83, 81, 79, 77, 75, 73, 71, 69,
            67, 65, 63, 61, 59, 57, 55, 53, 51, 49, 47, 45, 43, 41, 39, 37, 35, 33, 31, 29, 27, 25, 23, 21, 19, 17, 15, 13, 11,
            9, 7, 5, 3, 1
        }.ToImmutableArray(),
        new byte[]
        {
            255, 253, 251, 249, 247, 245, 243, 241, 239, 237, 235, 233, 231, 229, 227, 225, 223, 221, 219, 217, 215, 213, 211,
            209, 207, 205, 203, 201, 199, 197, 195, 193, 191, 189, 187, 185, 183, 181, 179, 177, 175, 173, 171, 169, 167, 165,
            163, 161, 159, 157, 155, 153, 151, 149, 147, 145, 143, 141, 139, 137, 135, 133, 131, 129, 127, 125, 123, 121, 119,
            117, 115, 113, 111, 109, 107, 105, 103, 101, 99, 97, 95, 93, 91, 89, 87, 85, 83, 81, 79, 77, 75, 73, 71, 69, 67, 65,
            63, 61, 59, 57, 55, 53, 51, 49, 47, 45, 43, 41, 39, 37, 35, 33, 31, 29, 27, 25, 23, 21, 19, 17, 15, 13, 11, 9, 7, 5,
            3, 1, 0, 2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32, 34, 36, 38, 40, 42, 44, 46, 48, 50, 52, 54, 56,
            58, 60, 62, 64, 66, 68, 70, 72, 74, 76, 78, 80, 82, 84, 86, 88, 90, 92, 94, 96, 98, 100, 102, 104, 106, 108, 110,
            112, 114, 116, 118, 120, 122, 124, 126, 128, 130, 132, 134, 136, 138, 140, 142, 144, 146, 148, 150, 152, 154, 156,
            158, 160, 162, 164, 166, 168, 170, 172, 174, 176, 178, 180, 182, 184, 186, 188, 190, 192, 194, 196, 198, 200, 202,
            204, 206, 208, 210, 212, 214, 216, 218, 220, 222, 224, 226, 228, 230, 232, 234, 236, 238, 240, 242, 244, 246, 248,
            250, 252, 254
        }.ToImmutableArray(),
        new byte[]
        {
            0, 2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32, 34, 36, 38, 40, 42, 44, 46, 48, 50, 52, 54, 56, 58,
            60, 62, 64, 66, 68, 70, 72, 74, 76, 78, 80, 82, 84, 86, 88, 90, 92, 94, 96, 98, 100, 102, 104, 106, 108, 110, 112,
            114, 116, 118, 120, 122, 124, 126, 128, 130, 132, 134, 136, 138, 140, 142, 144, 146, 148, 150, 152, 154, 156, 158,
            160, 162, 164, 166, 168, 170, 172, 174, 176, 178, 180, 182, 184, 186, 188, 190, 192, 194, 196, 198, 200, 202, 204,
            206, 208, 210, 212, 214, 216, 218, 220, 222, 224, 226, 228, 230, 232, 234, 236, 238, 240, 242, 244, 246, 248, 250,
            252, 254, 255, 253, 251, 249, 247, 245, 243, 241, 239, 237, 235, 233, 231, 229, 227, 225, 223, 221, 219, 217, 215,
            213, 211, 209, 207, 205, 203, 201, 199, 197, 195, 193, 191, 189, 187, 185, 183, 181, 179, 177, 175, 173, 171, 169,
            167, 165, 163, 161, 159, 157, 155, 153, 151, 149, 147, 145, 143, 141, 139, 137, 135, 133, 131, 129, 127, 125, 123,
            121, 119, 117, 115, 113, 111, 109, 107, 105, 103, 101, 99, 97, 95, 93, 91, 89, 87, 85, 83, 81, 79, 77, 75, 73, 71,
            69, 67, 65, 63, 61, 59, 57, 55, 53, 51, 49, 47, 45, 43, 41, 39, 37, 35, 33, 31, 29, 27, 25, 23, 21, 19, 17, 15, 13,
            11, 9, 7, 5, 3, 1
        }.ToImmutableArray(),
        new byte[]
        {
            255, 30, 30, 30, 30, 30, 30, 30, 30, 59, 59, 59, 59, 59, 59, 59, 59, 86, 86, 86, 86, 86, 86, 86, 86, 111, 111, 111,
            111, 111, 111, 111, 111, 134, 134, 134, 134, 134, 134, 134, 134, 155, 155, 155, 155, 155, 155, 155, 155, 174, 174,
            174, 174, 174, 174, 174, 174, 191, 191, 191, 191, 191, 191, 191, 191, 206, 206, 206, 206, 206, 206, 206, 206, 219,
            219, 219, 219, 219, 219, 219, 219, 230, 230, 230, 230, 230, 230, 230, 230, 239, 239, 239, 239, 239, 239, 239, 239,
            246, 246, 246, 246, 246, 246, 246, 246, 251, 251, 251, 251, 251, 251, 251, 251, 254, 254, 254, 254, 254, 254, 254,
            254, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 254, 254, 254, 254, 254, 254, 254,
            254, 251, 251, 251, 251, 251, 251, 251, 251, 246, 246, 246, 246, 246, 246, 246, 246, 239, 239, 239, 239, 239, 239,
            239, 239, 230, 230, 230, 230, 230, 230, 230, 230, 219, 219, 219, 219, 219, 219, 219, 219, 206, 206, 206, 206, 206,
            206, 206, 206, 191, 191, 191, 191, 191, 191, 191, 191, 174, 174, 174, 174, 174, 174, 174, 174, 155, 155, 155, 155,
            155, 155, 155, 155, 134, 134, 134, 134, 134, 134, 134, 134, 111, 111, 111, 111, 111, 111, 111, 111, 86, 86, 86, 86,
            86, 86, 86, 86, 59, 59, 59, 59, 59, 59, 59, 59, 30, 30, 30, 30, 30, 30, 30, 30
        }.ToImmutableArray()
    }.ToImmutableArray();
    //@formatter:on
}