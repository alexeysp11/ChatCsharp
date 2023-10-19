# ChatCsharp 

Доступно на других языках: [English/Английский](README.md), [Russian/Русский](README.ru.md). 

Простой клиент-серверный чат, написанный на C#.

Этот проект состоит из двух проектов: *клиентское приложение* и *серверное приложение*.
*Клиентское приложение* написано с использованием шаблонов **WPF** и **MVVM**.
*Серверное приложение* написано как **консольное приложение**.

Описание проекта представлено по [данной ссылке](Docs/Description.ru.md).

## Технологии 

- .NET фреймворки:
    - .NET Core 3.1 (C# 8)
    - .NET 6 (C# 10)
- Базы данных: 
    - [SQLite](https://github.com/sqlite/sqlite)
    - [PostgreSQL](https://www.postgresql.org/)
- Внешние сервисы (библиотеки): 
    - [workflow-auth](https://github.com/alexeysp11/workflow-auth)
    - [workflow-lib](https://github.com/alexeysp11/workflow-lib)

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

Для того, чтобы скачать данный репозиторий, необходимо выполнить следующие комманды в командной строке:
```
git clone https://github.com/alexeysp11/workflow-auth.git
git clone https://github.com/alexeysp11/workflow-lib.git
git clone https://github.com/alexeysp11/ChatCsharp.git
```

## Фрагменты кода
