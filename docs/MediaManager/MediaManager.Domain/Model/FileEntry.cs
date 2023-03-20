namespace MediaManager.Domain.Model
{
    public class FileEntry
    {
        private FileInfo fileInfo;

        public FileEntry() : this(new FileInfo("NA"))
        { }

        public FileEntry(FileInfo fileInfo)
        {
            this.fileInfo = fileInfo;
        }

        public static FileEntry Copy(FileEntry fm)
        {
            return new FileEntry(fm.fileInfo);
        }

        public string FullPath
        {
            get { return fileInfo.FullName; }
            set
            {
                if (!string.IsNullOrEmpty(value) && File.Exists(value))
                {
                    fileInfo = new FileInfo(value);
                }
            }
        }

        public string LastModified
        {
            get { return fileInfo.LastWriteTime.ToShortDateString(); }
        }

        public string Name
        {
            get { return fileInfo.Name; }
        }

        public DirectoryInfo ParentDirectory
        {
            get { return fileInfo.Directory; }
        }

        public bool FileExists
        {
            get { return fileInfo != null && File.Exists(fileInfo.FullName); }
        }

        private bool fileMatches = false;
        public bool FileMatches
        {
            get { return fileMatches; }
            set
            {
                if (fileMatches != value)
                {
                    fileMatches = value;
                }
            }
        }

        public long FileLength
        {
            get { return fileInfo.Length; }
        }

        public override string ToString()
        {
            return FullPath;
        }
    }
}
