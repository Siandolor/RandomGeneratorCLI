using System.Security.Cryptography;

namespace RandomGenerator
{
    internal class RandomGenerator
    {
        // ==========================================================
        //  CORE TRANSFORMATION LOOP
        //  Performs a pseudo-iterative transformation on an integer
        //  value using cryptographically strong randomness.
        //
        //  The loop executes <value> times. On each iteration:
        //   • If the input is even, it is replaced by a random bit (0–1)
        //   • If the input is odd, it is replaced by a signed random value (-1 or 0)
        //
        //  The final transformed integer is returned as the output.
        // ==========================================================
        public int SetValueWithLoop(int input, int value)
        {
            int count = 0;

            do
            {
                // Even input → replace with random bit
                if (input % 2 == 0)
                {
                    input = RandomNumberGenerator.GetInt32(0, 2);
                }
                // Odd input → replace with random signed value
                else
                {
                    input = -RandomNumberGenerator.GetInt32(-1, 1);
                }

                count++;
            }
            while (count < value);

            return input;
        }
    }
}
