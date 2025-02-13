using System;
using System.IO;

namespace Vocabulario
{
    public class ACK : ICodec // Mensaje enviado por el receptor al emisor que contiene solo el número de secuencia
    {
        // Campo privado
        private int _sequenceNumber;

        // Propiedad pública
        public int SequenceNumber
        {
            get { return _sequenceNumber; }
            set { _sequenceNumber = value; }
        }

        public int Length { get; set; }

        // Constructor de la clase
        public ACK(int sequenceNumber = 0)
        {
            _sequenceNumber = sequenceNumber;
        }

        // Método que obtiene el número de secuencia de un array de bytes
        public void Decode(byte[] encodedData)
        {
            // Crear un flujo para lectura
            using (MemoryStream memoryStream = new MemoryStream(encodedData))
            using (BinaryReader reader = new BinaryReader(memoryStream))
            {
                // Obtener el número de secuencia
                _sequenceNumber = reader.ReadInt32();
            }
        }

        // Método que codifica el número de secuencia en un array de bytes
        public byte[] Encode()
        {
            // Crear un buffer y flujos de escritura
            byte[] encodedData;
            using (MemoryStream memoryStream = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(memoryStream))
            {
                // Escribir el número de secuencia
                writer.Write(_sequenceNumber);
                writer.Flush();

                // Obtener el array de bytes
                encodedData = memoryStream.ToArray();
            }

            return encodedData;
        }
    }
}
