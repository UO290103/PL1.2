using System;
using System.Collections.Generic;
using System.Text;

namespace Vocabulario
{
    internal interface IArchive<T>
    {
        T[] Reader(string archivePath); 
        void Writter(List<T> elements, string archivePath);
        IEnumerable<T> LineReader(string line);
    }
}
