# ChatCsharp 

Простой клиент-серверный чат, написанный на C#.

Этот проект состоит из двух проектов: *клиентское приложение* и *серверное приложение*.
*Клиентское приложение* написано с использованием шаблонов **WPF** и **MVVM**.
*Серверное приложение* написано как **консольное приложение**.

Описание проекта представлено по [данной ссылке](Docs/Description.ru.md).

## Как использовать

### Предварительные условия

- ОС Windows;
- .NET Core 3.1;
- Любой текстовый редактор (*VS Code*, *Sublime Text*, *Notepad++* и т.д.) или Visual Studio;
- Командная строка Windows (если вы не используете Visual Studio).

Зависимости для этого приложения:

- `System.Net.Sockets` для *программирования сокетов*;
- `Microsoft.Data.Sqlite` для *базы данных SQLite*;
- `System.ComponentModel.INotifyPropertyChanged` для *сообщений об изменениях в пользовательском интерфейсе*;
- `System.Windows.Input.ICommand` для реализации *команд*;
- `System.IDisposable` для *GC*.

## Фрагменты кода