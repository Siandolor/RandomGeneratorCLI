using System.Security.Cryptography;
using System.Threading.Tasks;

namespace RandomGenerator
{
    internal class RandomApi
    {
        // ==========================================================
        //  FIELDS
        //  Instance of RandomGenerator used for internal calculations.
        // ==========================================================
        private readonly RandomGenerator generator = new RandomGenerator();

        // ==========================================================
        //  MAIN ENTRY POINT
        //  Generates a set of random digit strings using cryptographic RNG,
        //  processes each through a deterministic transformation loop,
        //  and returns both original and processed results.
        // ==========================================================
        public RandomResult Run(int amountNumbers, int lengthNumbers)
        {
            string[] numbers = new string[amountNumbers];
            string[] processed = new string[amountNumbers];

            // ----------------------------------------------------------
            // STEP 1: Generate random numeric strings in parallel
            // Each element receives a sequence of digits created by
            // GenerateSecureDigitString(), ensuring cryptographic entropy.
            // ----------------------------------------------------------
            Parallel.For(0, amountNumbers, i =>
            {
                numbers[i] = GenerateSecureDigitString(lengthNumbers);
            });

            // ----------------------------------------------------------
            // STEP 2: Transform the random digits through the internal
            // generator algorithm (SetValueWithLoop). This applies a
            // repeatable, value-dependent transformation to each digit.
            // ----------------------------------------------------------
            Parallel.For(0, amountNumbers, i =>
            {
                string digits = numbers[i];
                char[] binDigits = new char[lengthNumbers];

                for (int j = 0; j < digits.Length; j++)
                {
                    int digit = digits[j] - '0';
                    int input = digit;
                    int value = digit;
                    int result = generator.SetValueWithLoop(input, value);
                    binDigits[j] = (char)('0' + result);
                }

                processed[i] = new string(binDigits);
            });

            // ----------------------------------------------------------
            // STEP 3: Return both sets as part of the RandomResult object.
            // ----------------------------------------------------------
            return new RandomResult
            {
                Original = numbers,
                Processed = processed
            };
        }

        // ==========================================================
        //  SECURE DIGIT GENERATION
        //  Creates a cryptographically strong random string consisting
        //  of digits from 1–9. Used as entropy input for later processing.
        // ==========================================================
        private static string GenerateSecureDigitString(int lengthNumbers)
        {
            char[] digits = new char[lengthNumbers];

            for (int i = 0; i < lengthNumbers; i++)
            {
                // Generate a secure random integer in the range [1, 9]
                int r = RandomNumberGenerator.GetInt32(1, 10);
                digits[i] = (char)('0' + r);
            }

            return new string(digits);
        }
    }
}
