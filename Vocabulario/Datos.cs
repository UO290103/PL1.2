using System;
using System.Collections.Generic;
using System.IO;
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
        public void Decode(byte[] Codif)
        {
            //Crear stream de lectura
            MemoryStream ms = new MemoryStream(Codif);
            BinaryReader reader = new BinaryReader(ms); 
            //Leer el numero de secuencia y el numero transmitido
            _seq = reader.ReadInt32();
            _num = reader.ReadInt32();
        }
        //Metodo que codifica en un array de bytes el numero de secuencia y el numero tranferido
        public byte[] Code()
        {
            //Crear el array de bytes y el stream de escritura
            byte[] codif;
            MemoryStream ms = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(ms);
            //Escritura del numero de secuencia y del numero a transmitir
            writer.Write(_seq);
            writer.Write(_num);
            writer.Flush();
            //Obtención del array de bytes
            codif = ms.ToArray();
            return codif;
        }
    }
}
