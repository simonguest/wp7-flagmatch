using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Media;

namespace FlagMatch.Providers
{
    public class ResourceStreamImageProvider : IImageProvider
    {
        public Stream GetImage(string flagName)
        {
            String fileName = String.Format("{0}.png", flagName);
            return Application.GetResourceStream(new Uri("/FlagMatch;component/Images/" + fileName[0] + "/" + fileName, UriKind.Relative)).Stream;
        }
    }
}
