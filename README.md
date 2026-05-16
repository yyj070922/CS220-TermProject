# CS220-TermProject
A command-line Dobutsu Shogi built with **F# / .NET 10**.

You play as **RED** against a AI enemy **BLUE**. Enter a command(Move/Drop) to decide own behavior.

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)  
  Verify with: `dotnet --version` (should show `10.x.x`)

### Run

```bash
# Windows
run.bat

# Unix / macOS
chmod +x run.sh
./run.sh

# Or directly
dotnet run
```

### Build

```bash
dotnet build
```
### Publish Self-Contained Binary

```bash
# Windows x64
dotnet publish -c Release -r win-x64 --self-contained

# Linux x64
dotnet publish -c Release -r linux-x64 --self-contained
```

---


## LLM Part
- 기물 이동 가능 여부 코드 판단
- Move, Drop 버그 고치기
- 텍스트 색깔 입히기
- AI depth 2 구현
