using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

namespace RandomGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // ==========================================================
            //  MAIN LOOP
            //  Displays the CLI header, handles user selection,
            //  executes the random generation pipeline,
            //  and repeats until the user chooses to exit.
            // ==========================================================
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\\\\\\###=== RandomGenerator CLI ===###///\n");

                int value = GetValueFromUser();
                if (value == -1)
                {
                    Console.WriteLine("\nProgram finished. Goodbye!");
                    break;
                }

                var stopwatch = Stopwatch.StartNew();

                int amountNumbers = value;
                int lengthNumbers = value;

                string projectDir = GetProjectDirectory();
                string imageDir = Path.Combine(projectDir, "images");
                string saltDir = Path.Combine(projectDir, "salts");

                Directory.CreateDirectory(imageDir);
                Directory.CreateDirectory(saltDir);

                var result = RunRandomApi(amountNumbers, lengthNumbers, stopwatch);

                string filePath = Path.Combine(imageDir, $"random_output_{value}.png");
                ExportImage(result, filePath, stopwatch);

                var salts = ExtractSalts(filePath, stopwatch);

                string saltFile = Path.Combine(saltDir, $"salts_{value}.txt");
                SaveSalts(salts, saltFile, stopwatch);

                stopwatch.Stop();
                Console.WriteLine($"Runtime: {stopwatch.ElapsedMilliseconds} ms\n");

                Console.WriteLine("Process finished. Press any key to continue...");
                Console.ReadKey(intercept: true);
            }
        }

        // ==========================================================
        //  USER INPUT
        //  Displays a table of selectable values (0–9) and returns
        //  the chosen numeric size parameter. Returns -1 if the user
        //  selects “Exit”.
        // ==========================================================
        private static int GetValueFromUser()
        {
            Console.WriteLine("Please enter a value as input:\n");

            Console.WriteLine("+-----------------------------------------+");
            Console.WriteLine("|  1) 256     |  2) 512     |  3) 1024    |");
            Console.WriteLine("|  4) 2048    |  5) 4096    |  6) 8192    |");
            Console.WriteLine("|  7) 16384   |  8) 32768   |  9) 65534   |");
            Console.WriteLine("+-----------------------------------------+");
            Console.WriteLine("|  0) Exit                                |");
            Console.WriteLine("+-----------------------------------------+\n");

            Console.Write("Valid values (0–9, Default = 7): ");

            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) return 16384;

            if (int.TryParse(input, out int num))
            {
                if (num == 0) return -1; // Exit
                int[] values = { 256, 512, 1024, 2048, 4096, 8192, 16384, 32768, 65534 };
                num = Math.Clamp(num, 1, values.Length);
                int value = values[num - 1];
                Console.WriteLine($"Selected value: {value}\n");
                return value;
            }

            Console.WriteLine("Invalid input – default value (16384) will be used.\n");
            return 16384;
        }

        // ==========================================================
        //  PROJECT DIRECTORY
        //  Returns the root directory of the current project
        //  (typically the folder above /bin/Debug or /bin/Release).
        // ==========================================================
        private static string GetProjectDirectory()
        {
            return Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;
        }

        // ==========================================================
        //  RANDOM API EXECUTION
        //  Calls the RandomApi class to generate random data
        //  using the given amount and length parameters.
        // ==========================================================
        private static RandomResult RunRandomApi(int amountNumbers, int lengthNumbers, Stopwatch stopwatch)
        {
            Console.WriteLine("Starting RandomApi...");
            var api = new RandomApi();
            var result = api.Run(amountNumbers, lengthNumbers);
            Console.WriteLine($"RandomApi done after {stopwatch.ElapsedMilliseconds} ms\n");
            return result;
        }

        // ==========================================================
        //  IMAGE EXPORT
        //  Saves the random data as a .png image to visualize
        //  the generated entropy. Tracks duration for performance stats.
        // ==========================================================
        private static void ExportImage(RandomResult result, string filePath, Stopwatch stopwatch)
        {
            Console.WriteLine("Exporting Image as .png...");
            var before = stopwatch.ElapsedMilliseconds;
            RandomImageExporter.SaveAsPng(result, filePath);
            Console.WriteLine($"Image saved: {filePath} (Duration: {stopwatch.ElapsedMilliseconds - before} ms)\n");
        }

        // ==========================================================
        //  SALT EXTRACTION
        //  Reads the previously generated image and extracts
        //  hexadecimal salt values embedded in the pixel data.
        // ==========================================================
        private static IEnumerable<string> ExtractSalts(string filePath, Stopwatch stopwatch)
        {
            Console.WriteLine("Extracting Hexadecimal-Salts...");
            var before = stopwatch.ElapsedMilliseconds;
            var salts = SaltExtractor.ExtractAllSalts(filePath);
            Console.WriteLine($"Salt-Extraction done (Duration: {stopwatch.ElapsedMilliseconds - before} ms)\n");

            int i = 1;
            foreach (var salt in salts)
            {
                Console.WriteLine($"Salt {i++}: {salt}");
            }

            Console.WriteLine("");
            return salts;
        }

        // ==========================================================
        //  SAVE SALTS
        //  Writes all extracted salts to a text file (.txt)
        //  for later use in key derivation or analysis.
        // ==========================================================
        private static void SaveSalts(IEnumerable<string> salts, string saltFile, Stopwatch stopwatch)
        {
            Console.WriteLine("Saving Salt-File as .txt...");
            var before = stopwatch.ElapsedMilliseconds;
            File.WriteAllLines(saltFile, salts);
            Console.WriteLine($"Salts saved: {saltFile} (Duration: {stopwatch.ElapsedMilliseconds - before} ms)\n");
        }
    }
}
