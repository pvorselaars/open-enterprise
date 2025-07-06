# Open Enterprise

Open Enterprise is an experimental .NET interactive web application for modeling and simulating enterprise communication and production processes using an ontology-driven approach.

## Features

- **Ontology-based domain model**: Core concepts like `Role`, `ProductionFact`, `CommunicationFact`, and `Intention` are automatically generated as C# classes from [Protocol Buffers (Protobuf)](https://protobuf.dev/) definitions.
- **Blazor interactive UI**: The frontend is built with Blazor, allowing real-time interaction and visualization of enterprise facts and communications.
- **Dynamic communication acts**: Actors can perform and respond to various communication acts (e.g., Request, Promise, State, Accept, Reject) based on the current state of production and communication facts.

## Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)

### Running the Application

1. Clone the repository:
   ```sh
   git clone https://github.com/pvorselaars/open-enterprise.git
   cd open-enterprise
   ```
2. Run the application:
   ```sh
   dotnet run
   ```
3. Open your browser and navigate to `https://localhost`

## Project Structure

- `Ontology/` — Core `.proto` files (Role, Facts, etc.)
- `UI/` — Blazor components and pages
- `wwwroot/` — Static assets (CSS, etc.)
- `Program.cs` — Application entry point and DI setup

## Background

This project is based on enterprise ontology, a formal approach to modeling the structure and communication within organizations. For more information, see:

- [Enterprise Ontology](https://link.springer.com/book/10.1007/3-540-33149-2)
- [DEMO (Design & Engineering Methodology for Organizations)](https://en.wikipedia.org/wiki/Design_%26_Engineering_Methodology_for_Organizations)

## Roadmap

- [x] User authentication and multi-actor simulation
- [ ] Full transaction implementation with cancellation patterns
- [ ] Persistent storage
- [ ] Visualize and edit models
- [ ] UI generation using state model
- [ ] Import/export models using plain text files

## License

This project is licensed under the MIT License. See [LICENSE](./LICENSE) for details.

## Contributing

Contributions, suggestions, and issues are welcome. Please open a PR or file an issue in GitHub.

## Contact

Questions? Reach out on GitHub or submit an issue.
