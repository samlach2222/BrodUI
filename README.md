# BrodUI

## Visual Studio project description: BrodUI

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
