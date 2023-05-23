# BrodUI

[![Version du projet](https://img.shields.io/badge/version-1.0-purple.svg)](https://img.shields.io/badge "Version du projet")

<p align="center">
  <img src="BrodUI logo.svg" width="40%">
</p>

## What is BrodUI ?

BrodUI is an image processing project coded in C# about the creation of an embroidery pattern
from an image. For this it is necessary to reduce the number of colors with the K-Means algorithm so that the embroiderer doesn't need too many different threads.

## Project composition

The project is located in the BrodUI folder. This project was created with Visual Studio 2022. It uses `C#`, `WPFUI` and `.NET 6.0`.  
When you launch the project, you will see the following folders:
- `Assets`: contains the images used in the project (logo, background, etc.)
- `Helpers`: contains the classes used to manage the data (data manipulation such as Enum to Boolean, etc.)
- `Models`: contains the main classes used in the project (like the class)
- `Services` : contains the classes used to manage the background tasks (application host, etc. WPFUI stuff)
- `ViewModels` : contains the background code for the different pages of the application (like the .cs file in the .xaml file in WPF)
- `Views` : contains the different pages of the application (like the .xaml file in WPF with the .cs file in the ViewModels folder)
- `Windows` : contains the different windows of the application (here the main window which contains the different pages)

You can find the unit test project BrodUITests. This project has been created with Visual Studio 2022. It uses `C#`, `xUnit` and `.NET 6.0`.  
In this folder you will find all the unit tests of the project (Models and Helpers classes).

So, as you can see, all the classes you create should be in the `Models` folder and all the data manipulation classes should be in the `Helpers` folder.  
These classes will then be used in the `ViewModel` folder and implemented graphically in the `Views` folder.

## How to build ?

To build the project you simply have to `clone` it, open it in Visual Studio 2022 by the `.sln` file, right click on the `BrodUI project` and Build.
To install BrodUI, you can just download the installer in [Releases](https://github.com/samlach2222/BrodUI/releases/latest).

## Project members

* **Orlane TISSERAND** - [@Rinslet-Tsuina](https://github.com/Rinslet-Tsuina)
* **Zeina-Hélène AL-HALABI** - [@ZeinaAl-Halabi](https://github.com/ZeinaAl-Halabi)
* **Loïs PAZOLA** - [@Mahtwo](https://github.com/Mahtwo)
* **Samuel LACHAUD** - [@samlach2222](https://github.com/samlach2222)
* **Corentin DEBRABANT** - [@CorentinDebrabant](https://github.com/CorentinDebrabant)
* **Vincet GAY** - [@VincentGay01](https://github.com/VincentGay01)

## Credits

* Logo edited from a <a href="https://www.flaticon.com/free-icon/cross-stitch_2818654" title="Cross stitch icon">cross stitch icon created by Freepik - Flaticon</a>
