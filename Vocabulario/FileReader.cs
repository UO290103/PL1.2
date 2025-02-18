using System;
using System.Collections.Generic;
using System.IO;

namespace Vocabulario
{
    public class FileReader : IArchive<sbyte>
    {
        public sbyte[] Reader(string path)
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

            var numbers = new List<sbyte>();

            // Vamos a leer línea por línea el archivo.
            // El using se asegura de que el archivo se cierre automáticamente.
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

            // Devolvemos la lista de números convertida en un array.
            return numbers.ToArray();
        }

        // Definimos el método de lectura de la línea.
        public IEnumerable<sbyte> LineReader(string line)
        {
            string temp = ""; // Cadena temporal para acumular dígitos.

            for (int i = 0; i < line.Length; i++)
            {
                // Comprobamos si el caracter actual es un '-' y el siguiente un número.
                if (line[i] == '-' && i + 1 < line.Length && Char.IsNumber(line[i + 1]))
                {
                    if (temp.Length > 0 && sbyte.TryParse(temp, out sbyte result))
                    {
                        yield return result;
                    }
                    temp = "-"; // Agregamos el símbolo negativo.
                }
                else if (Char.IsNumber(line[i]))
                {
                    temp += line[i]; // Agregamos número.
                }
                else if (temp.Length > 0) // Si encontramos un caracter no numérico y temp no está vacío, devolvemos el número.
                {
                    if (sbyte.TryParse(temp, out sbyte result))
                    {
                        yield return result;
                    }
                    else
                    {
                        Console.WriteLine($"Advertencia: El número '{temp}' está fuera del rango permitido (-128 a 127) y será ignorado.");
                    }
                    temp = ""; // Reseteamos variable temp.
                }
            }

            // Si queda algún número acumulado en temp, lo procesamos.
            if (temp.Length > 0)
            {
                if (sbyte.TryParse(temp, out sbyte lastResult))
                {
                    yield return lastResult;
                }
                else
                {
                    Console.WriteLine($"Advertencia: El número '{temp}' está fuera del rango permitido (-128 a 127) y será ignorado.");
                }
            }
        }

        // Indicamos una excepción ya que no implementa Writer
        public void Writter(List<sbyte> elements, string archivePath)
        {
            throw new NotImplementedException();
        }
    }
}
