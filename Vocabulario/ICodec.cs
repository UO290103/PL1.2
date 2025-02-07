using System;

namespace Vocabulario
{
    public interface ICodec
    { //Interfaz para la codificación y decodificación de los mensajes
        byte[] Code();
        void Decode(byte[] A);
    }
}
