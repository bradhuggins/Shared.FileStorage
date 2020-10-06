#region Using Statements
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
#endregion

namespace Shared.FileStorage
{
    public class Common
    {
        public static string GetUniqueFilename(string fileName)
        {
            string toReturn = string.Empty;
            toReturn = GenerateCleanGuid() + Path.GetExtension(fileName);
            return toReturn;
        }

        public static StorageFilePath ParseStoragePath(string path)
        {
            StorageFilePath toReturn = new StorageFilePath();
            List<string> splitValues = path.Split('/').ToList();
            toReturn.ShareName = splitValues[0];
            toReturn.FolderName = splitValues[1];
            toReturn.FileName = splitValues[2];
            return toReturn;
        }

        public static string GenerateCleanGuid()
        {
            return Guid.NewGuid().ToString()
                .Replace("{", string.Empty)
                .Replace("}", string.Empty)
                .Replace("-", string.Empty)
                .ToLower();
        }
    }
}
