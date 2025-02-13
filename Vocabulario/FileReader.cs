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

            return numbers;
        }


        // Definimos el método de lectura de la línea

        private IEnumerable<string> LineReader(string line)
        {
            string temp = ""; // Cadena temporal para acumular dígitos.

            for (int i = 0; i < line.Length; i++)
            {
                // Si el carácter actual es un dígito, lo agregamos a temp.
                if (Char.IsNumber(line[i]))
                {
                    temp += line[i];
                }
                // Si el carácter actual es un guion y es el inicio de un número negativo.
                else if (line[i] == '-' && i + 1 < line.Length && Char.IsNumber(line[i + 1]) && (i == 0 || !Char.IsNumber(line[i - 1])))
                {
                    temp += line[i]; // Agregamos el guion a temp.
                }
                else
                {
                    // Si encontramos un carácter no numérico y temp no está vacío, devolvemos el número acumulado.
                    if (temp.Length > 0)
                    {
                        yield return temp;
                        temp = ""; // Reiniciamos la cadena temporal.
                    }
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
