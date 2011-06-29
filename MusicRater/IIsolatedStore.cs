using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MusicRater
{
    public interface IIsolatedStore : IDisposable
    {
        bool FileExists(string fileName);
        Stream CreateFile(string fileName);
        Stream OpenFile(string fileName);
    }
}
