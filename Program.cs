using System;
using System.IO;
using System.Linq;

namespace LineEndings
{
    class Program
    {
        private const byte CR = 0x0D;
        private const byte LF = 0x0A;
        // ReSharper disable once InconsistentNaming
        private static readonly byte[] DOSLineEnding = { CR, LF };

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
            string[] allfiles = Directory.GetFiles(args[1], "*.*", SearchOption.AllDirectories);

            foreach (string fileName in allfiles)
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
        /// Convert DOS style line endings to UNIX line endings
        /// </summary>
        /// <param name="fileName">file to convert</param>
        private static void Dos2Unix(string fileName)
        {

            byte[] data = File.ReadAllBytes(fileName);
            using (FileStream fileStream = File.OpenWrite(fileName))
            {
                BinaryWriter binaryWriter = new BinaryWriter(fileStream);
                int position = 0;
                int index;
                do
                {
                    index = Array.IndexOf(data, CR, position);
                    if (index < 0 || data[index + 1] != LF)
                        break;
                    // Write before the CR
                    binaryWriter.Write(data, position, index - position);
                    // from LF
                    position = index + 1;
                } while (index > 0);

                binaryWriter.Write(data, position, data.Length - position);
                fileStream.SetLength(fileStream.Position);
            }
        }

        /// <summary>
        /// Convert UNIX style line endings to DOS line endings
        /// </summary>
        /// <param name="fileName">file to convert</param>
        private static void Unix2Dos(string fileName)
        {
            byte[] data = File.ReadAllBytes(fileName);
            using (FileStream fileStream = File.OpenWrite(fileName))
            {
                BinaryWriter binaryWriter = new BinaryWriter(fileStream);
                int position = 0;
                int index;
                do
                {
                    index = Array.IndexOf(data, LF, position);
                    if (index < 0)
                        continue;

                    if (index > 0 && data[index - 1] == CR)
                    {
                        // already dos ending
                        binaryWriter.Write(data, position, index - position + 1);
                    }
                    else
                    {
                        binaryWriter.Write(data, position, index - position);
                        binaryWriter.Write(DOSLineEnding);
                    }
                    position = index + 1;
                } while (index > 0);

                binaryWriter.Write(data, position, data.Length - position);
                fileStream.SetLength(fileStream.Position);
            }
        }
    }
}