namespace DreamCatcher.Core.Models
{
    public enum DreamIntensity
    {
        Normal,
        Vivid,
        Lucid,
    }

    public enum DreamType
    {
        Normal,
        Nightmare,
        Pleasent
    }


    public class Dream : EntityBase
    {
        public byte[]? Picture { get; set; }

        public DateTime DateTime { get; set; }

        public string Title { get; set; } = null!;

        public string? ShortDescription { get; set; }

        public string? Description { get; set; }

        public DreamIntensity Intensity { get; set; } = DreamIntensity.Normal;

        public DreamType Type { get; set; } = DreamType.Normal;
    }
}
