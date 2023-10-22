# ChatCsharp 

Read this in other languages: [English](README.md), [Russian/Русский](README.ru.md). 

A simple Client-Server chat written in `C#`.

This project consists of two projects: *client side app* and *server side app*. 
*Client side application* is written using **WPF** and **MVVM** pattern. 
*Server side application* is written as a **console app**. 

The project description is presented at [this link](Docs/Description.md).

## Techonologies 

- .NET frameworks:
    - .NET Core 3.1 (C# 8)
    - .NET 6 (C# 10)
- Databases: 
    - [SQLite](https://github.com/sqlite/sqlite)
    - [PostgreSQL](https://www.postgresql.org/)
- External services (libraries): 
    - [workflow-auth](https://github.com/alexeysp11/workflow-auth)
    - [workflow-lib](https://github.com/alexeysp11/workflow-lib)
- [Swagger](https://swagger.io/tools/swagger-ui)

## How to use 

### Prerequisites

- Windows OS;
- .NET Core 3.1;
- Any text editor (*VS Code*, *Sublime Text*, *Notepad++* etc) or Visual Studio;
- Windows command line (if you do not use Visual Studio).

Dependencies for this application:

- `System.Net.Sockets` for *Socket Programming*;
- `Microsoft.Data.Sqlite` for *SQLite Database*; 
- `System.ComponentModel.INotifyPropertyChanged` for *reporting changes to UI*; 
- `System.Windows.Input.ICommand` for *commands* implementation; 
- `System.IDisposable` for *GC*. 

In order to get the application, run the following command in the CMD:
```
git clone https://github.com/alexeysp11/workflow-auth.git
git clone https://github.com/alexeysp11/workflow-lib.git
git clone https://github.com/alexeysp11/ChatCsharp.git
```

## Code snippets 
