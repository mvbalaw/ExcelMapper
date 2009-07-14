namespace ExcelMapper.Configuration
{
    public interface IFileConfiguration
    {
        string FileName { get; set; }
    }

    public class FileConfiguration : IFileConfiguration
    {
        public string FileName { get; set; }
    }
}