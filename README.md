# RandomGenerator CLI

A high-entropy random data generator written in **C# (.NET)**.  
It creates cryptographically secure digit sequences, transforms them through a custom symbolic algorithm,  
renders them as monochrome **PNG images**, and extracts **256-bit hexadecimal salts** for use in post-quantum cryptography experiments or randomness analysis.

---

## Features

- **Interactive CLI**
    - User-selectable entropy size (256 → 65,534)
    - Automatic repetition without restart
- **Cryptographically Secure RNG**
    - Based on `System.Security.Cryptography.RandomNumberGenerator`
- **Parallelized Computation**
    - Dual-phase generation and transformation loops using `Parallel.For`
- **Visual Entropy Export**
    - Generates monochrome PNG bitmaps (black = 0, white = 1)
- **Salt Extraction**
    - Reconstructs 256-bit hexadecimal salt values from image pixel data
- **Performance Metrics**
    - Millisecond-accurate runtime tracking via `Stopwatch`

---

## Project Structure

```
RandomGenerator/
├── Program.cs               # Main CLI loop
├── RandomApi.cs             # Parallel random generation & transformation
├── RandomGenerator.cs       # Core transformation logic
├── RandomImageExporter.cs   # PNG export (ImageSharp)
├── SaltExtractor.cs         # Salt reconstruction from PNGs
├── RandomResult.cs          # Data container (Original + Processed)
├── images/                  # Auto-created export directory
└── salts/                   # Auto-created output directory
```

---

## How It Works

1. **CLI Selection**  
   Choose the entropy size interactively:

```
Please enter a value as input:

+-----------------------------------------+
|  1) 256     |  2) 512     |  3) 1024    |
|  4) 2048    |  5) 4096    |  6) 8192    |
|  7) 16384   |  8) 32768   |  9) 65534   |
+-----------------------------------------+
|  0) Exit                                |
+-----------------------------------------+
Valid values (0–9, Default = 7):
```

2. **Random Generation**  
   Two-phase process:
    - Phase 1: Generate secure digit strings using CSPRNG
    - Phase 2: Transform digits via the custom loop function in `RandomGenerator.cs`

3. **Image Export**  
   `RandomImageExporter.SaveAsPng()` encodes each processed bitstring as a monochrome pixel row.

4. **Salt Extraction**  
   `SaltExtractor.ExtractAllSalts()` scans the PNG, reconstructs bits → bytes → 32-byte salts (256-bit) → hex.

5. **Output Example**

```
RandomApi done after 3184 ms
Image saved: /images/random_output_16384.png
Salt-Extraction done (Duration: 72 ms)

Salt 1: 7E9D3F02A01B6C...
Salt 2: 1F005A49DCCA8B...
...
Salts saved: /salts/salts_16384.txt
```

---

## Build & Run

### Prerequisites
- .NET 6 or later
- [SixLabors.ImageSharp](https://github.com/SixLabors/ImageSharp)

### Build
```bash
   dotnet build
```

### Run
```bash
   dotnet run
```

---

## Example Output

```
Runtime: 4217 ms

Process finished. Press any key to continue...
```

Generated files:
```
/images/random_output_16384.png
/salts/salts_16384.txt
```

---

## Notes

- The project is designed for **educational and experimental use** in high-entropy generation and PQC-related data transformation.
- The algorithm does **not** implement standard cryptographic key derivation — it’s meant for controlled randomness studies.
- All components are thread-safe and deterministic at the structural level, but yield non-deterministic data outputs.

---

## Future Work

- Add visualization overlays (entropy density heatmaps)
- Implement export to binary entropy files
- Integrate with PQC key-test harness
- Optional GPU acceleration (OpenCL / CUDA)

---

## Author
**Daniel Fitz, MBA, MSc, BSc**  
Vienna, Austria  
Developer & Security Technologist — *Post-Quantum Cryptography, Blockchain/Digital Ledger & Simulation*  
C/C++ · C# · Java · Python · Visual Basic · ABAP · JavaScript/TypeScript

International Accounting · Macroeconomics & International Relations · Physiotherapy · Computer Sciences  
Former Officer of the German Federal Armed Forces

---

## License
**MIT License** — free for educational and research use.  
Attribution required for redistribution or commercial adaptation.

---

> "Entropy is not chaos.  
> It’s the silence between two collisions of probability."  
> — Daniel Fitz, 2025
