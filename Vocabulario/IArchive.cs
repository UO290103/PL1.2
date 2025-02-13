using System;
using System.Collections.Generic;
using System.Text;

namespace Vocabulario
{
    internal interface IArchive
    {
        IEnumerable<string> Reader(string archivePath);
        void Writter(List<string> elements, string archivePath);
    }
}
