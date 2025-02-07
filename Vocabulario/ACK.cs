using System;
using System.Collections.Generic;
using System.Text;

namespace Vocabulario
{
    public class ACK : ICodec //Mensaje que manda el receptor al emisor conteniendo unicamente el numero de secuencia
    {
        //Atributos de la clase Datos
        private int _seq;

        //Propiedades para acceder a los atributos
        public int seq
        {
            get { return _seq; }
            set { _seq = value; }
        }

        //Constructor de la clase
        public ACK(int s = 0)
        {
            _seq = s;
        }

        //Metodo que obtiene el numero de secuencia de un array de bytes
        public void Decode(byte[] A)
        {
            _seq = A[0];
        }

        //Metodo que codifica en un array de bytes el numero de secuencia
        public byte[] Code()
        {
            byte[] A = new byte[1];
            byte seqByte = (byte)_seq;
            A[0] = seqByte;
            return A;
        }
    }
}

