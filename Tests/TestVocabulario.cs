using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vocabulario;

namespace Tests
{
    [TestClass]
    public class DatosTest
    { //Test de la clase Datos de la librería Vocabulario
        [TestMethod]
        public void TestDatosConstructorVacio(){ //Test que comprueba el constructor sin argumentos de la clase Datos
            //Creamos Datos sin argumentos por lo que _seq y _num tienen que ser 0
            Datos D = new Datos(); 
            //Comprobamos si _seq es 0
            Assert.AreEqual(0, D.seq); 
        }
        [TestMethod]
        public void TestDatosConstructorConDatos(){ //Test que comprueba el constructor con argumentos de la clase Datos
            //Creamos Datos con secuencia 3 y numero 4
            Datos D = new Datos(3,4); 
            //Comprueba si _seq es 3
            Assert.AreEqual(3, D.seq); 
        }
        [TestMethod]
        public void TestDatosCodec(){ //Test que comprueba la codificación y decodificación de la clase Datos 
            //Creamos D1 como variable a codificar y D2 como variable donde se descodifica
            Datos D1 = new Datos(2, 5);
            Datos D2 = new Datos();
            //Codificamos D1 en un array de bytes
            byte[] BytesPrueba = D1.Code();
            //Decodficamos el array en D2
            D2.Decode(BytesPrueba);
            //Comprobamos que D1.seq sea igual a D2.seq
            Assert.AreEqual (D1.seq, D2.seq);
        }
    }
    [TestClass]
    public class ACKTest
    {//Tests de la clase ACK de la biblioteca Vocabulario
        [TestMethod]
        public void TestACKConstructorVacio(){//Test que comprueba el constructor vacio de la clase ACK
            //Creamos ACK sin argumentos por lo que secuencia tiene que ser 0
            ACK A = new ACK();
            //Comprobamos que _seq sea 0
            Assert.AreEqual(0, A.seq);
        }
        [TestMethod]
        public void TestACKConstructorConDatos(){//Test que comprueba el constructor con argumentos de la clase ACK
            //Creamos A con numero de secuencia 4
            ACK A = new ACK(4);
            //Comprobamos que _seq sea 4
            Assert.AreEqual(4, A.seq);
        }
        [TestMethod]
        public void TestACKCodec(){ //Test que comprueba la codificación y decodificación de la clase ACK
            //Creamos A1 con numero de secuencia 5 para codificar y A2 para decodificar
            ACK A1 = new ACK(5);
            ACK A2 = new ACK();
            //Codificamos la secuencia de A1 en un array de bytes
            byte[] BytesPrueba = A1.Code();
            //Decodificamos el array de bytes en A2
            A2.Decode(BytesPrueba);
            //Comprobamos que A1.seq y A2.seq sean iguales
            Assert.AreEqual(A1.seq, A2.seq);
        }
    }
}
