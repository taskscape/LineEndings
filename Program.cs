using System;
using System.IO;
using System.Linq;

namespace LineEndings
{
    class Program
    {
        private const byte Cr = 0x0D;
        private const byte Lf = 0x0A;
        // ReSharper disable once InconsistentNaming
        private static readonly byte[] DOSLineEnding = { Cr, Lf };

        private static readonly string[] KnownBinaryExtensions =
        {
            ".exe", ".jar", ".dll", ".class", ".zip", ".rar", ".suo", ".doc", ".docx", ".xls", ".xlsx", ".png", ".gif",
            ".jpg", ".jpeg", ".tif", ".tiff", ".swf", ".fla", "bin"
        };

        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: LineEndings.exe ([D2U/U2D]) (FullPathToFiles)");
                return;
            }

            bool d2U = args[0] == "D2U";
            bool u2D = args[0] == "U2D";

            if (!d2U && !u2D)
            {
                Console.WriteLine("Specified conversion is invalid, provide either \"D2U\" or \"U2D\" as the first parameter");
                return;
            }

            if (Directory.Exists(args[1]) == false)
            {
                Console.WriteLine("Specified path does not exists, provide valid path name as the second parameter");
                return;
            }

            Console.WriteLine("Scanning: '" + args[1] + "' and subdirectories");
            string[] allFiles = Directory.GetFiles(args[1], "*.*", SearchOption.AllDirectories);

            foreach (string fileName in allFiles)
            {
                try
                {
                    if (KnownBinaryExtensions.Any(Path.GetExtension(fileName).ToLowerInvariant().Contains))
                    {
                        Console.WriteLine("Skipping: '" + fileName + "'");
                        continue;
                    }

                    Console.WriteLine("Converting: '" + fileName + "'");
                    if (d2U)
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
                    Console.WriteLine("Conversion failed: '" + fileName + "' with error: '" + exception.Message + "'");
                }
            }

            Console.WriteLine("Converting finished");
            Console.ReadLine();

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