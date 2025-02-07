using System;
using System.Collections.Generic;
using System.Text;

namespace Vocabulario
{
    public class Datos : ICodec //Mensaje que manda el emisor al receptor contiene el numero de secuencia y el numero a transferir
    {
        //Atributos de la clase Datos
        private int _seq;
        private int _num;

        //Propiedades para acceder a los atributos
        public int seq
        {
            get { return _seq; }
            set { _seq = value; }
        }
        public int num
        {
            get { return _num; }
            set { _num = value; }
        }
        //Constructor de la clase
        public Datos(int s = 0, int n = 0)
        {
            _seq = s;
            _num = n;
        }
        //Metodo que obtiene el numero de secuencia y el numero transferido de un array de bytes
        public void Decode(byte[] A)
        {
            _seq = A[0];
            _num = A[1];
        }
        //Metodo que codifica en un array de bytes el numero de secuencia y el numero tranferido
        public byte[] Code()
        {
            byte[] A = new byte[2];
            byte seqByte = (byte)_seq;
            byte numByte = (byte)_num;
            A[0] = seqByte;
            A[1] = numByte;
            return A;
        }
    }
}
