using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MusicRater
{
    public interface IIsolatedStore
    {
        bool FileExists(string fileName);
        Stream CreateFile(string fileName);
        Stream OpenFile(string fileName);
        IEnumerable<string> GetFileNames(string pattern);
    }
}
