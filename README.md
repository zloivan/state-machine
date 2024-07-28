# Unity State Machine Package

A flexible and efficient state machine implementation for Unity projects.

## Features

- Generic state machine supporting any enum-based state
- Async support with UniTask for potentially long-running operations
- State history tracking and undo functionality
- Transition guards for conditional state changes
- Thread-safe implementation
- Customizable logging system

## Installation

1. This package requires UniTask. Please install UniTask in your project first. You can find installation instructions for UniTask [here](https://github.com/Cysharp/UniTask#install-via-git-url).

2. Open the Package Manager in Unity (Window > Package Manager).

3. Click the "+" button in the top-left corner and select "Add package from git URL".

4. Enter the following URL: `https://github.com/zloivan/state-machine.git`

5. Click "Add". Unity will download and install the package.

Note: This package is distributed as a git repository, so Unity's Package Manager cannot automatically install its dependencies. Make sure you have UniTask installed before adding this package.

## Package Structure

The package is organized as follows:

- `Runtime/`: Contains all scripts required for the state machine, including interfaces, classes, and helpers.
- `Examples/`: Contains example implementations and use cases for the state machine.

Note: The `Examples/` folder is optional. After installing the package, you can import the examples by clicking the "Import" button next to "Examples" in the Package Manager.

## Usage

[Basic usage example to be added]

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.
