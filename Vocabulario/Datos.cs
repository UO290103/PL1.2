using System;
using System.IO;

namespace Vocabulario
{
    public class Data : ICodec // Mensaje enviado por el emisor al receptor, contiene el número de secuencia y el número a transferir
    {
        // Atributos de la clase
        private int _seq;
        private sbyte _num;

        // Propiedades para acceder a los atributos
        public int Seq
        {
            get { return _seq; }
            set { _seq = value; }
        }
        public sbyte Number
        {
            get { return _num; }
            set { _num = value; }
        }

        // Constructor de la clase
        public Data(int seq = 0, sbyte num = 0)
        {
            _seq = seq;
            _num = num;
        }

        // Método que recupera el número de secuencia y el número transferido desde un array de bytes
        public void Decode(byte[] encodedData)
        {
            // Crear un flujo para lectura
            MemoryStream ms = new MemoryStream(encodedData);
            BinaryReader reader = new BinaryReader(ms);
            // Leer el número de secuencia y el número transmitido
            _seq = reader.ReadInt32();
            _num = reader.ReadSByte();
        }

        // Método que codifica el número de secuencia y el número transferido en un array de bytes
        public byte[] Encode()
        {
            // Crear el array de bytes y el flujo de escritura
            byte[] encodedData;
            MemoryStream ms = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(ms);
            // Escribir el número de secuencia y el número a transmitir
            writer.Write(_seq);
            writer.Write(_num);
            writer.Flush();
            // Obtener el array de bytes
            encodedData = ms.ToArray();
            return encodedData;
        }
    }
}
