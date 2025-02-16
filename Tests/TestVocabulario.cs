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
                new List<int> { 0, 1, 2, 3, 4, 5, 6, -1, -2, -3, 5 },
                numbers.ToList()
                );
        }

        [TestMethod]
        public void TestReadLineData()
        {
            var fileReader = new FileReader();
            string line = "Esto123es4una567prueba89-1";


            var result = fileReader.LineReader(line);

            CollectionAssert.AreEqual(
                new List<int> { 123, 4, 567, 89, -1 },
                result.Cast<int>().ToList());
        }
    }

    [TestClass]
    public class NumberWriterTest
    {

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))] // Espera una ArgumentNullException
        public void TestWriterFilePathNull()
        {
            // Creamos una instancia de NumberReader.
            FileWriter numberWriter = new FileWriter();
            List<int> numbers = null;

            // Intentamos pasar un path nulo y comprobamos que se lanza ArgumentNullException.
            numberWriter.Writer(numbers, "Inexistent_file.txt");
        }

        [TestMethod]
        [ExpectedException(typeof(DirectoryNotFoundException))] // Espera un DirectoryNotFoundException
        public void TestWriterPathInexistent()
        {
            /*
             * Comprobamos el correcto uso de la excepción en caso
             * de no encontrar un directorio.
             */
            FileWriter numberWriter = new FileWriter();
            List<int> numbers = new List<int> { 1, 2, 3 };
            string filePath = "C:\\Inexistent\\File.txt";

            // Comprobamos la inexistencia del archivo con su excepción.
            numberWriter.Writer(numbers, filePath);
        }

        [TestMethod]
        public void TestWriterNewFile()
        {
            FileWriter numberWriter = new FileWriter();

            string tempDir = Path.GetTempPath();
            string fileName = "TestFileWriter.txt";
            string filePath = Path.Combine(tempDir, fileName);

            List<int> numbers = new List<int> { 1, 2, 3 };

            try
            {
                numberWriter.Writer(numbers, filePath);

                Assert.IsTrue(File.Exists(filePath), "El archivo no fue creado.");

            }
            finally // Revertimos la acción anterior para eliminar el archivo creado.
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }

        [TestMethod]
        public void TestWriterNewNameForFile()
        {
            FileWriter numberWriter = new FileWriter();

            string tempDir = Path.GetTempPath();
            string fileName = "TestFileWriter.txt";
            string filePath1 = Path.Combine(tempDir, fileName);
            string filePath2 = Path.Combine(tempDir, "TestFileWriter_1.txt");

            List<int> numbers = new List<int> { 1, 2, 3 };

            try
            {
                numberWriter.Writer(numbers, filePath1);
                numberWriter.Writer(numbers, filePath1); //Creamos nuevamente

                Assert.IsTrue(File.Exists(filePath1), "El archivo no ha sido creado.");
                // Comprobamos que se ha creado uno nuevo.
                Assert.IsTrue(File.Exists(filePath2), "No se ha creado el segundo archivo.");
            }
            finally
            {
                if (File.Exists(filePath1) && File.Exists(filePath2))
                {
                    File.Delete(filePath1);
                    File.Delete(filePath2);
                }
            }
        }

        [TestMethod]
        public void TestWriterData()
        {
            FileWriter numberWriter = new FileWriter();
            FileReader numberReader = new FileReader();

            string tempDir = Path.GetTempPath();
            string fileName = "TestFileWriter.txt";
            string filePath = Path.Combine(tempDir, fileName);

            List<int> numbers = new List<int> { 1, 2, 3 };

            try
            {
                numberWriter.Writer(numbers, filePath);
                var ints = numberReader.Reader(filePath);

                Assert.AreEqual(3, ints.Length);

                CollectionAssert.AreEqual( //Comprobamos que coincide la información del interior.
                numbers,
                ints.ToList()
                );
            }
            finally
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }
    }
}
