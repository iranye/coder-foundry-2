namespace MediaManager.Domain.Model
{
    using System;

    public class Volume
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public List<M3uFile> M3uFiles { get; set; }

        public DateTime Created { get; set; } = DateTime.MinValue;

        public DateTime LastModified { get; set; } = DateTime.MinValue;
        
        public bool CopyDirectories { get; set; }

        public override string ToString()
        {
            return $"{Title}";
        }
    }
}
