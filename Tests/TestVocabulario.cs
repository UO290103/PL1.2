using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using Vocabulario;

namespace Tests
{
    [TestClass]
    public class MessageTest
    {
        // Prueba para la clase Message de la biblioteca Vocabulario
        [TestMethod]
        public void TestMessageEmptyConstructor()
        {
            // Crear un mensaje sin argumentos, por lo que _seq y _num deberían ser 0
            Data msg = new Data();
            // Comprobar si _seq es 0
            Assert.AreEqual(0, msg.Seq);
        }

        [TestMethod]
        public void TestMessageConstructorWithData()
        {
            // Crear un mensaje con secuencia 3 y número 4
            Data msg = new Data(3, 4);
            // Comprobar si _seq es 3
            Assert.AreEqual(3, msg.Seq);
        }

        [TestMethod]
        public void TestMessageCodec()
        {
            // Crear msg1 para codificar y msg2 para decodificar
            Data msg1 = new Data(2, 5);
            Data msg2 = new Data();
            // Codificar msg1 en un array de bytes
            byte[] testBytes = msg1.Encode();
            // Decodificar el array de bytes en msg2
            msg2.Decode(testBytes);
            // Comprobar si msg1.Seq es igual a msg2.Seq
            Assert.AreEqual(msg1.Seq, msg2.Seq);
        }
    }

    [TestClass]
    public class AcknowledgmentTest
    {
        // Pruebas para la clase Acknowledgment de la biblioteca Vocabulario
        [TestMethod]
        public void TestAcknowledgmentEmptyConstructor()
        {
            // Crear un Acknowledgment sin argumentos, por lo que la secuencia debería ser 0
            ACK ack = new ACK();
            // Comprobar si la secuencia es 0
            Assert.AreEqual(0, ack.SequenceNumber);
        }

        [TestMethod]
        public void TestAcknowledgmentConstructorWithData()
        {
            // Crear un Acknowledgment con el número de secuencia 4
            ACK ack = new ACK(4);
            // Comprobar si el número de secuencia es 4
            Assert.AreEqual(4, ack.SequenceNumber);
        }

        [TestMethod]
        public void TestAcknowledgmentCodec()
        {
            // Crear ack1 con número de secuencia 5 para codificar, y ack2 para decodificar
            ACK ack1 = new ACK(5);
            ACK ack2 = new ACK();
            // Codificar el número de secuencia de ack1 en un array de bytes
            byte[] testBytes = ack1.Encode();
            // Decodificar el array de bytes en ack2
            ack2.Decode(testBytes);
            // Comprobar si ack1.SequenceNumber y ack2.SequenceNumber son iguales
            Assert.AreEqual(ack1.SequenceNumber, ack2.SequenceNumber);
        }
    }

    [TestClass]
    public class NumberReaderTest
    {

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))] // Espera una ArgumentNullException
        public void TestReadFilePathNull()
        {
            // Creamos una instancia de NumberReader.
            FileReader numberReader = new FileReader();

            // Intentamos pasar un path nulo y comprobamos que se lanza ArgumentNullException.
            numberReader.Reader(null);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))] // Espera un FileNotFoundException
        public void TestReadNoDataFilePath()
        {
            /*
             * Comprobamos el correcto uso de la excepción en caso
             * de no encontrar un archivo con el nombre.
             */
            FileReader numberReader = new FileReader();

            // Comprobamos la inexistencia del archivo con su excepción.
            numberReader.Reader("InexistentFile.txt");
        }

        [TestMethod]
        public void TestReadFileData()
        {
            FileReader numberReader = new FileReader();

            string filePath = "TestFileReader.txt";

            var numbers = numberReader.Reader(filePath);

            CollectionAssert.AreEqual(
                new List<sbyte> { 0, 1, 2, 3, 4, 5, 6, -1, -2, -3, 5 },
                numbers.ToList()
                );
        }

        [TestMethod]
        public void TestReadLineData()
        {
            var fileReader = new FileReader();
            string line = "Esto123es4u127na-128prueba89-1";


            var result = fileReader.LineReader(line);

            CollectionAssert.AreEqual(
                new List<sbyte> { 123, 4, 127, -128, 89, -1 },
                result.Cast<sbyte>().ToList());
        }

        [TestMethod]
        public void TestIsSByte()
        {
            var fileReader = new FileReader();
            string line = "6060";

            var result = fileReader.LineReader(line);

            string expectedOut = $"Advertencia: El número '{(string)line}' está fuera del rango permitido (-128 a 127) y será ignorado.";

        }
    }
}