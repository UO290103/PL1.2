using System;

namespace Vocabulario
{
    public interface ICodec
    {
        // Interfaz para codificar y decodificar.
        byte[] Encode();
        void Decode(byte[] encodedData);
    }
}
