namespace MediaManager.Domain.Model
{
    // --------------------------------------------------------------------------------------------------------------------
    // <copyright file="M3uFile.cs" company="IRANYE">
    //   Copyright (c) IRANYE. All rights reserved.
    // </copyright>
    // --------------------------------------------------------------------------------------------------------------------
    using Newtonsoft.Json;
    using System.IO;

    public class M3uFile
    {
        private FileEntry fileEntry;
        private string partialPath = string.Empty;

        public M3uFile(string fullPath, string parentDirPath)
        {
            if (!string.IsNullOrEmpty(parentDirPath))
            {
                ParentDirPath = parentDirPath;
            }
            if (!string.IsNullOrEmpty(fullPath))
            {
                if (!File.Exists(fullPath))
                {
                    throw new ArgumentException("Invalid File Path");
                }
                if (String.IsNullOrWhiteSpace(ParentDirPath) || !Directory.Exists(parentDirPath))
                {
                    throw new ArgumentException("Invalid ParentDirPath");
                }
                fileEntry = new FileEntry(new FileInfo(fullPath));
                partialPath = fullPath.Replace(ParentDirPath, String.Empty);
                partialPath = partialPath.TrimStart('\\');
            }
        }

        public int TotalMegaBytes { get; set; } = 22;

        [JsonIgnore]
        public FileEntry FileEntry
        {
            get { return fileEntry; }
            set
            {
                if (fileEntry != value)
                {
                    fileEntry = value;
                }
            }
        }

        [JsonIgnore]
        public string Name
        {
            get { return fileEntry == null ? string.Empty : fileEntry.Name; }
        }

        private string parentDirPath = String.Empty;

        [JsonIgnore]
        public string ParentDirPath
        {
            get { return parentDirPath;}
            set
            {
                if (parentDirPath != value)
                {
                    parentDirPath = value;
                    fileEntry = new FileEntry(new FileInfo(Path.Combine(parentDirPath, partialPath)));
                }
            }
        }

        public string PartialPath
        {
            get
            {
                return partialPath;
            }
            set
            {
                if (partialPath != value)
                {
                    if (!String.IsNullOrEmpty(value))
                    {
                        partialPath = value;
                        if (!String.IsNullOrWhiteSpace(ParentDirPath))
                        {
                            fileEntry = new FileEntry(new FileInfo(Path.Combine(ParentDirPath, partialPath)));
                        }
                    }
                    else
                    {
                        partialPath = String.Empty;
                    }
                }
            }
        }

        [JsonIgnore]
        public string FullPath
        {
            get { return fileEntry == null ? string.Empty : fileEntry.FullPath; }
            set
            {
                if (!string.IsNullOrEmpty(value) && File.Exists(value))
                {
                    fileEntry.FullPath = value;
                }
            }
        }

        [JsonIgnore]
        public string LastModified
        {
            get { return fileEntry.LastModified; }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
