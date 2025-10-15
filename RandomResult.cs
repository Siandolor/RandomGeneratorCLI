namespace RandomGenerator
{
    internal class RandomResult
    {
        // ==========================================================
        //  DATA STRUCTURE
        //  Holds the results produced by the RandomApi.
        //
        //  • Original : the raw random digit strings generated
        //               by the cryptographic RNG.
        //
        //  • Processed: the transformed representations after
        //               passing through RandomGenerator logic.
        //
        //  Acts as a lightweight container for further processing
        //  such as image export and salt extraction.
        // ==========================================================
        public string[] Original { get; set; } = Array.Empty<string>();
        public string[] Processed { get; set; } = Array.Empty<string>();
    }
}
