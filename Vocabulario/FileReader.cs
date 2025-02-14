using System;
using System.Collections.Generic;
using System.IO;

namespace Vocabulario
{
    public class FileReader : IArchive
    {
        public IEnumerable<string> Reader(string path)
        {

            // Especificamos las posibles excepciones.

            if (path == null)
            {
                throw new ArgumentNullException(nameof(path), "El argumento 'path' no puede ser nulo.");
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("El archivo no existe.", path);
            }

            var numbers = new List<string>();
            
            // Vamos a leer línea por línea el archivo.
            using (var file = new StreamReader(path))
            {
                string line;

                // Comprobamos que la línea no esté vacía.
                while ((line = file.ReadLine()) != null)
                {
                    foreach (var number in LineReader(line))
                    {
                        numbers.Add(number);
                    }
                }
            }

            // Devolvemos el array de números.
            return numbers.ToArray(); 
        }


        // Definimos el método de lectura de la línea

        private IEnumerable<string> LineReader(string line)
        {
            string temp = ""; // Cadena temporal para acumular dígitos.

            for (int i = 0; i < line.Length; i++)
            {
                // Comprobamos si el caracter actual es un '-' y el siguiente un número.
                if (line[i] == '-' && i + 1 < line.Length && Char.IsNumber(line[i + 1]))
                {
                    if (temp.Length > 0)
                    {
                        yield return temp;
                        temp = "";
                    }
                    temp += "-"; // Agregamos el símbolo negativo.
                }

                // Comprobamos si el caracter actual es un nº.
                else if (Char.IsNumber(line[i]))
                {
                    temp += line[i]; // Agregamos número.
                }

                // Si encontramos un caracter no numérico y temp no está vacío, devolvemos el número.
                else if (temp.Length > 0)
                {
                    yield return temp;
                    temp = ""; // Reseteamos variable temp.
                }
            }

            // Si queda algún número acumulado en temp, lo devolvemos.
            if (temp.Length > 0)
            {
                yield return temp;
            }
        }

        // Indicamos que no se emplea Writter en esta implementación.

        public void Writter(List<string> elements, string archivePath)
        {
            throw new NotImplementedException();
        }

    }
}
