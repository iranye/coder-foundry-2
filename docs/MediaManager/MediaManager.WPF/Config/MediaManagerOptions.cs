namespace MediaManager.WPF.Config
{
    public class MediaManagerOptions
    {
        public const string SectionName = "Directories";
        public string RootDirectory { get; set; }
        public string StartingPath { get; set; }
        public string CopyToPath { get; set; }
    }
}
