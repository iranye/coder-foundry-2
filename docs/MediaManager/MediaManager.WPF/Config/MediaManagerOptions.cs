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
        public string RootDirectory { get; set; }
        public string StartingPath { get; set; }
        public string CopyToPath { get; set; }
    }
}
