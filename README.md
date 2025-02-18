# rhinoXR

`rhinoXR` is a Rhino plugin that enables real-time transmission of 3D data from Rhino to external applications using WebSockets. This facilitates seamless integration with web-based 3D viewers

https://github.com/user-attachments/assets/29b4bb17-6255-4176-9f36-2621a49627a4

## Features

- Real-time streaming of 3D data from Rhino.
- Utilizes WebSockets for efficient data transmission.
- Compatible with web and XR applications.

## Installation

### Prerequisites

- Rhino 3D modeling software.
- .NET Framework installed on your system.

### Steps

1. Clone this repository:
   ```bash
   git clone https://github.com/OmkarBhagwat29/rhinoXR.git
   ```
2. Open the solution file `autoXR.sln` in Visual Studio.
3. Build the solution to compile the plugin.
4. Copy the resulting plugin file to Rhino's plugins directory.
5. Launch Rhino and load the `rhinoXR` plugin.

## Usage

1. Start the `rhinoXR` plugin within Rhino.
2. Ensure your external application is set up to receive data via WebSocket.
3. Begin modeling in Rhino; changes will be streamed in real-time to the connected application.

## Demo

For a demonstration of `rhinoXR` in action, refer to the [auto-xr](https://github.com/OmkarBhagwat29/auto-xr) repository, which includes a 3D web viewer that displays the streamed data.

## Related Projects

- [auto-xr](https://github.com/OmkarBhagwat29/auto-xr): A web-based 3D viewer that receives and displays data streamed from Rhino using the `rhinoXR` plugin.

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request with your changes.

## License

This project is licensed under the MIT License.


