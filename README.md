# Nightfall Update Pusher

A tool for generating the correct directory structure for the [Nightfall Resource Server](https://github.com/NeqsTech/nightfall-resource-server) to properly include a newly created update of the [Nightfall Client](https://github.com/NeqsTech/nightfall-client)

## Usage

To push* a new update (or the very first one) you pass the path to the folder where the updates should be stored with the `-p` switch.
In that folder, you need another folder exactly called `.new` where you put the files that you want to push.
```
Nightfall.UpdatePusher -p "files/updates/"
```

\* - Pushing means to add a new update so that the Resource Server can recongnize it as a valid update . I couldn't find a better word for it.

## Getting Started

These instructions will give you a copy of the project up and running on
your local machine for development and testing purposes.

### Prerequisites

- Installed .NET 6 (other frameworks might work though they weren't tested)

### Downloading

See [Releases](https://github.com/NeqsTech/nightfall-update-pusher/releases) and download the executable for your platform.

### Developing

Project depends on [CommandLineParser](https://github.com/commandlineparser/commandline/).
Since all the Nightfall projects are developed using JetBrains' Rider I recommend using it too but there shouldn't be a problem with opening the project in other IDEs.

## Versioning

[Semantic Versioning](http://semver.org/) will be followed like in the rest of the Nightfall projects

## License

This project is licensed under the [BSD 3-Clause License](LICENSE.md)

## Acknowledgments

- The name 'Nightfall' is temporary and will be replaced by something more thoughtful in the future. It's more of a codename than a name.
