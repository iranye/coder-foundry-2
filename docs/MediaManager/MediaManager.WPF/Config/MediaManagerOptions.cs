using System.IO;
using System;

namespace MediaManager.WPF.Config
{
    // --------------------------------------------------------------------------------------------------------------------
    // <copyright file="MediaManagerOptions.cs" company="IRANYE">
    //   Copyright (c) IRANYE. All rights reserved.
    // </copyright>
    // --------------------------------------------------------------------------------------------------------------------
    public class MediaManagerOptions
    {
        public const string SectionName = "Directories";
        public string Media { get; set; } = string.Empty;
        public string RootDirectory { get; set; } = string.Empty;
        public string StartingPath { get; set; } = string.Empty;
        public string CopyToPath { get; set; } = string.Empty;

        public string StartPath
        {
            get
            {
                return Path.Combine(RootPath, StartingPath);
            }
        }

        public string CopyPath
        {
            get
            {
                return Path.Combine(RootPath, CopyToPath);
            }
        }

        private string rootPath = string.Empty;

        public string RootPath
        {
            get
            {
                if (String.IsNullOrWhiteSpace(rootPath))
                {
                    var configSettingRoot = this.Media;

                    var foundPath = string.Empty;
                    try
                    {
                        if (Directory.Exists(configSettingRoot))
                        {
                            rootPath = configSettingRoot;
                        }
                        else
                        {
                            if (configSettingRoot == "%MEDIA%")
                            {
                                foundPath = Environment.GetEnvironmentVariable("MEDIA");
                                if (Directory.Exists(foundPath))
                                {
                                    rootPath = foundPath;
                                }
                            }
                            else
                            {
                                rootPath = RootDirectory;
                            }
                        }
                    }
                    catch { }
                    if (String.IsNullOrWhiteSpace(rootPath))
                    {
                        rootPath = @"D:\\Media-Track";
                    }

                }
                return rootPath;
            }
        }
    }
}
