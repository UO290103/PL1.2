using System;
using System.Collections.Generic;
using System.Text;

namespace Vocabulario
{
    internal interface IArchive<T>
    {
        T[] Reader(string archivePath); 
        void Writer(List<T> element, string archivePath);
        IEnumerable<T> LineReader(string line);
    }
}
