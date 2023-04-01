# QUACC

QUACC is a Windows Presentation Foundation (WPF) application that provides the user with a quick command line interface. Using the command line, users can open files, directories, URLs, create notes, reminders, and more. However, not all features mentioned are currently available. Please refer to the Latest Version section for a list of current features.

> :warning: **Not all of the above-stated features are available!** Please refer to the [Latest version](#latest-version) for a list of all the current features.

## How to Use

To use QUACC, simply build and run the application. From there, you can start typing commands into the main Command Window. To minimize QUACC, press the ESC key. To bring it back up, use the key combination Shift+Ctrl+Q.

## Executers

Executers are classes that hold specific commands and corresponding execution functions. Currently, the only functional executer is the main `QUACCCommandsExecuter`.

## QUACC Commands

All the following commands are provided by the main `QUACCCommandExecuter`.

### Hello World!

> hw __$args__

This command pops up a message box with a "Hello World" message. The `$args` input is also displayed.

### Open

> op __$arg__

This command checks whether you have registered a shortcut to a directory or file with the name `$arg`. If no shortcut is found, QUACC checks whether `$args` can be used as a URL and opens it.

### Add Shortcut

> adds __$name__ __$path__

This command creates a new shortcut under the name `$name` pointing to a directory, file, or URL defined by `$path`.

### Exit

> exit

This command exits the application.

## Latest Version

The latest version, "Armstrong," is the first version that is somewhat usable.

### Features

- Creating shortcuts to files and directories
- Opening URLs
- ...
- That's about it.

## What's to Come?

The first item on the to-do list is to create the `QUACCMath` executer for mathematical expressions. This feature will likely only include basic math operations. After that, the goal is to make QUACC more user-friendly by working on the user interface, creating a settings window, and potentially implementing color templates.
