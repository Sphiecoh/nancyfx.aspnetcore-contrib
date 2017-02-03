using System.IO;

namespace Nancy.AspNetCore.Contrib
{
    public class KestrelRootPathProvider : IRootPathProvider
    {
        public string GetRootPath() => Directory.GetCurrentDirectory();
        
    }
}
