using FlagMatch;
using System.IO;
using System.Reflection;
using FlagMatch.Providers;

namespace FlagMatch.Test
{
    public class MockImageProvider : IImageProvider
    {
        public Stream GetImage(string resourceName)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream("United Kingdom.png");
        }
    }
}
