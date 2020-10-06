using System;

namespace Shared.FileStorage.MockData
{
    public class Utilities
    {
        public static byte[] TransparentGif = {0x47, 0x49, 0x46, 0x38, 0x39, 0x61, 0x01, 0x00,
                                               0x01, 0x00, 0x80, 0xff, 0x00, 0xc0, 0xc0, 0xc0,
                                               0x00, 0x00, 0x00, 0x21, 0xf9, 0x04, 0x01, 0x00,
                                               0x00, 0x00, 0x00, 0x2c, 0x00, 0x00, 0x00, 0x00,
                                               0x01, 0x00, 0x01, 0x00, 0x00, 0x02, 0x02, 0x44,
                                               0x01, 0x00, 0x3b};

        public static string GenerateCleanGuid()
        {
            return Guid.NewGuid().ToString()
                .Replace("{", string.Empty)
                .Replace("}", string.Empty)
                .Replace("-", string.Empty)
                .ToLower();
        }

        public static string GenerateNewFilename()
        {
            return Guid.NewGuid().ToString()
                .Replace("{", string.Empty)
                .Replace("}", string.Empty)
                .Replace("-", string.Empty)
                .ToLower() + ".gif";
        }
    }
}
