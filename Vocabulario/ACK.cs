using System;
using System.Collections.Generic;
using System.IO;
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
        public void Decode(byte[] Codif)
        {
            //Creación de stream de lectura
            MemoryStream ms = new MemoryStream(Codif);
            BinaryReader reader = new BinaryReader(ms);
            //Obtener el numero de secuencia
            _seq = reader.ReadInt32();
        }

        //Metodo que codifica en un array de bytes el numero de secuencia
        public byte[] Code()
        {
            //Creación de buffer y streams de escritura
            byte[] Codif;
            MemoryStream ms = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(ms);
            //Leer el numero de secuencia
            writer.Write(_seq);
            writer.Flush();
            //Obtener el array de bytes
            Codif = ms.ToArray();
            return Codif;
        }
    }
}

