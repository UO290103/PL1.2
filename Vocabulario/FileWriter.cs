using System;
using System.Collections.Generic;
using System.IO;

namespace Vocabulario
{
    public class FileWriter : IArchive<int>
    {
        public void Writer(List<int> data, string path) 
        {
            // Especificamos las posibles excepciones

            if (data == null)
            {
                throw new ArgumentNullException(nameof(data), "El argumento 'elements' no puede ser nulo.");
            }

            // Verificamos si el directorio existe.
            string directoryPath = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException($"El directorio '{directoryPath}' no existe.");
            }

            // Vamos a mirar la existencia de un archivo:
            string lastPath = path;
            int count = 1;

            while(File.Exists(lastPath))
            {
                /*
                 * Este bucle tiene como finalidad observar si existe el archivo en el path indicado
                 * en caso de su existencia, creará uno nuevo con el siguiente formato:
                 * NombreArchivo_x.extensión
                 * Donde x -> Siguiente número empleado al último.
                 *       extensión -> La extensión que haya enviado el usuario.
                 */

                lastPath = Path.Combine(directoryPath, 
                    $"{Path.GetFileNameWithoutExtension(path)}_{count}{Path.GetExtension(path)}");
                count++;
            }

            // Finalmente, una vez comprobado el directorio, lo crearemos y escribiremos la información.
            using (var writer = new StreamWriter(lastPath))
            {
                foreach (var item in data)
                {
                    writer.WriteLine(item.ToString());
                }
            }

            // Mensaje informativo.
            Console.WriteLine($"Los datos se han guardado en {lastPath}");
        }

        public IEnumerable<int> LineReader(string line)
        {
            throw new NotImplementedException();
        }

        public int[] Reader(string archivePath)
        {
            throw new NotImplementedException();
        }
    }
}
