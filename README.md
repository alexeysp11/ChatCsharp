# ChatCsharp 

A simple Client-Server chat written in `C#`.

This project consists of two projects: *client side app* and *server side app*. 
*Client side application* is written using **WPF** and **MVVM** pattern. 
*Server side application* is written as a **console app**. 

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
