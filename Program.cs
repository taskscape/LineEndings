using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace LineEndings
{
    class Program
    {
        private const byte Cr = 0x0D;
        private const byte Lf = 0x0A;

        private static readonly HashSet<string> KnownBinaryExtensions = new HashSet<string>
        {
            ".exe", ".jar", ".dll", ".class", ".zip", ".rar", ".suo", ".doc", ".docx", ".xls", ".xlsx", ".png", ".gif",
            ".jpg", ".jpeg", ".tif", ".tiff", ".swf", ".fla", ".bin"
        };

        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: LineEndings.exe ([D2U/U2D]) (FullPathToFiles)");
                return;
            }

            bool convertToUnix = args[0] == "D2U";
            bool convertToDos = args[0] == "U2D";

            if (!convertToUnix && !convertToDos)
            {
                Console.WriteLine("Invalid conversion type. Use \"D2U\" or \"U2D\".");
                return;
            }

            if (!Directory.Exists(args[1]))
            {
                Console.WriteLine("Invalid path. Ensure the directory exists.");
                return;
            }

            Console.WriteLine($"Scanning: '{args[1]}' and subdirectories");
            var allFiles = Directory.EnumerateFiles(args[1], "*.*", SearchOption.AllDirectories);

            foreach (string fileName in allFiles)
            {
                string extension = Path.GetExtension(fileName).ToLowerInvariant();
                if (KnownBinaryExtensions.Contains(extension))
                {
                    Console.WriteLine($"Skipping binary file: '{fileName}'");
                    continue;
                }

                Console.WriteLine($"Converting: '{fileName}'");
                try
                {
                    if (convertToUnix)
                    {
                        Dos2Unix(fileName);
                    }
                    else
                    {
                        Unix2Dos(fileName);
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Failed to convert '{fileName}': {exception.Message}");
                }
            }

            Console.WriteLine("Conversion complete.");
        }

        /// <summary>
        /// Converts Windows-style line endings to Unix-style line endings in a file.
        /// </summary>
        /// <param name="fileName">The path to the file to convert.</param>
        private static void Dos2Unix(string fileName)
        {

            // Check if the file exists
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException("The specified file does not exist.", fileName);
            }

            // Read the content of the file
            string content = File.ReadAllText(fileName);

            // Replace Windows line endings (\r\n) with Unix line endings (\n)
            string convertedContent = content.Replace("\r\n", "\n");

            // Write the converted content back to the file
            File.WriteAllText(fileName, convertedContent);
        }

        /// <summary>
        /// Converts Unix-style line endings to Windows-style line endings in a file.
        /// </summary>
        /// <param name="fileName">The path to the file to convert.</param>
        private static void Unix2Dos(string fileName)
        {
            // Check if the file exists
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException("The specified file does not exist.", fileName);
            }

            // Read the content of the file
            string content = File.ReadAllText(fileName);

            // Replace Unix line endings (\n) with Windows line endings (\r\n)
            string convertedContent = content.Replace("\r\n", "\n").Replace("\n", "\r\n");

            // Write the converted content back to the file
            File.WriteAllText(fileName, convertedContent);
        }
    }
}
}
