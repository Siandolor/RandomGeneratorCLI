using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace RandomGenerator
{
    internal class RandomImageExporter
    {
        // ==========================================================
        //  IMAGE EXPORT
        //  Converts processed random data into a monochrome PNG image.
        //  Each character ('0' or '1') in the processed data determines
        //  the color of a pixel (black or white).
        //
        //  • Width  = number of digits per row (string length)
        //  • Height = number of rows (total processed entries)
        //
        //  Uses ImageSharp for image creation and saving.
        // ==========================================================
        public static void SaveAsPng(RandomResult result, string filePath)
        {
            int width = result.Processed[0].Length;
            int height = result.Processed.Length;

            // ----------------------------------------------------------
            // STEP 1: Create new RGBA image with defined dimensions
            // ----------------------------------------------------------
            using (var image = new Image<Rgba32>(width, height))
            {
                // ------------------------------------------------------
                // STEP 2: Iterate over each row and assign pixel colors
                // ------------------------------------------------------
                for (int y = 0; y < height; y++)
                {
                    string row = result.Processed[y];
                    for (int x = 0; x < width; x++)
                    {
                        var color = row[x] == '0'
                            ? new Rgba32(0, 0, 0)         // black pixel
                            : new Rgba32(255, 255, 255);  // white pixel

                        image[x, y] = color;
                    }
                }

                // ------------------------------------------------------
                // STEP 3: Save the constructed bitmap as a PNG file
                // ------------------------------------------------------
                image.SaveAsPng(filePath);
            }
        }
    }
}
