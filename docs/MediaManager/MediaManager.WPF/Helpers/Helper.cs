namespace MediaManager.WPF.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public static class Helper
    {
        public static IEnumerable<FileInfo> GetFileInfosFromFile(string fullPath)
        {
            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException($"File not found: '{fullPath}'");
            }

            FileInfo fileListFileInfo = new FileInfo(fullPath);
            DirectoryInfo? parentDirInfo = fileListFileInfo?.Directory;
            if (parentDirInfo is null)
            {
                yield return new FileInfo("");
            }
            List<string> filesInDirectory = new List<string>();
            foreach (var file in parentDirInfo!.GetFiles())
            {
                filesInDirectory.Add(file.Name);
            }
            // foreach (var subDirInfo in parentDirInfo.GetDirectories())
            // {
            //     filesInDirectory.AddRange(subDirInfo.GetFiles().Select(file => Path.Combine(subDirInfo.Name, file.Name)));
            // }

            var lines = File.ReadAllLines(fullPath);
            if (lines.Length == 0)
            {
                throw new Exception($"Empty file listing '{fullPath}'");
            }
            foreach (var line in lines)
            {
                if (line.StartsWith("#EXTM3U") || line.StartsWith("#EXTINF"))
                {
                    continue;
                }

                bool fileFound = false;
                foreach (var pathToFile in filesInDirectory)
                {
                    if (pathToFile.ToLower() == line.Trim().ToLower())
                    {
                        fileFound = true;
                        yield return new FileInfo(Path.Combine(parentDirInfo.FullName, pathToFile));
                        break;
                    }
                }

                if (!fileFound)
                {
                    yield return new FileInfo(Path.Combine(parentDirInfo.FullName, line));
                }
            }
        }

        public static void UpdateM3uFile(string fullPath, IEnumerable<FileInfo> fileInfos)
        {
            using (StreamWriter file = new StreamWriter(fullPath))
            {
                foreach (var fi in fileInfos)
                {
                    file.WriteLine(fi.Name);
                }
            }
        }

        public static DirectoryInfo? GetDirectoryInfo(string directoryPath, out string message)
        {
            DirectoryInfo? targetDirInfo = null;
            message = String.Empty;

            if (String.IsNullOrEmpty(directoryPath))
            {
                message = "directory is null or empty";
            }
            else
            {
                if (!Directory.Exists(directoryPath))
                {
                    message = $"directory not found: '{directoryPath}'{Environment.NewLine}";
                }
                else
                {
                    targetDirInfo = new DirectoryInfo(directoryPath);
                }
            }
            return targetDirInfo;
        }
    }
}
