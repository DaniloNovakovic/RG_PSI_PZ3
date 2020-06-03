# RG_PSI_PZ3

RG_PSI_PZ3 is a school project, built using WPF.

## Getting Started

Use these instructions to get the project up and running.

### Prerequisites

You will need the following tools:

- [Visual Studio 2017-2019](https://www.visualstudio.com/downloads/)
- [.NET Framework (>=4.6.1)](https://dotnet.microsoft.com/download/dotnet-framework)

> You also need to have C# version enabled which supports `?.` and `??` operators. (Recommended version is C# 7).

### Setup

Follow these steps to get your development environment set up:

  1. Clone the repository
  1. Build solution in Visual Studio (2017 or 2019)
  1. Start Application
  
---

## Usage

![App Demo](./doc/app-demo.gif)

### Controls

- **Move map** - Left click + Mouse move
- **Rotate map** - Right click + Mouse up/down
- **Highlight connected nodes** - Left click on line
- **Display node info** - Left click on node

### Node colors

| Color | Number of Connections |
| --- | --- |
| PaleVioletRed | < 3 |
| MediumVioletRed | <= 5 |
| Red | > 5 |

---

Copyright 2020 © [DaniloNovakovic](https://github.com/DaniloNovakovic)
