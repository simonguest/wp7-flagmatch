using System;
using System.IO;

namespace FlagMatch.Providers
{
    public interface IImageProvider
    {
        Stream GetImage(String flagName);
    }

}
