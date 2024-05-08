# Line Endings Converter

This utility is designed to convert line endings in text files between Windows-style (CRLF) and Unix-style (LF). It is useful for ensuring consistent line endings in a cross-platform development environment.

## Features

- Convert all text files in a directory and its subdirectories from DOS to Unix line endings and vice versa.
- Automatically skips binary files based on file extension.
- Command-line interface for easy integration with scripts.

## Requirements

- .NET 8.0 SDK or higher

## Installation

Clone the repository or download the source code:
```bash
git clone https://github.com/yourusername/line-endings-converter.git
cd line-endings-converter
```

```bash
dotnet build
```

Convert from DOS to Unix line endings
```bash
dotnet run -- D2U /path/to/your/files
```

Convert from Unix to DOS line endings
```bash
dotnet run -- U2D /path/to/your/files
```

## Parameters

- `D2U`: Convert from DOS (CRLF) to Unix (LF) line endings.
- `U2D`: Convert from Unix (LF) to DOS (CRLF) line endings.
- `FullPathToFiles`: The full path to the directory containing the files to convert.

## Known Limitations

- The program identifies binary files based on their extensions. If a binary file does not have a recognized extension, it may be incorrectly processed as a text file.
- The program does not handle files without read or write permissions.

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request with your changes.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
