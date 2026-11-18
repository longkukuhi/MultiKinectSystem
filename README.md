# Multi-Kinect human movement tracking

This Windows project explores tracking movement with multiple Kinect sensors. The WPF application detects connected sensors and shows two depth streams side by side. The repository also includes a separate Unity project for arm tracking and simulation.

## Display two depth streams

1. On Windows, use a Visual Studio installation with WPF support and the .NET Framework 4.6.1 targeting pack. The C# project references the `Microsoft.Kinect` assembly version 1.8.0.0, so use the matching Kinect for Windows SDK and compatible sensors.
2. Connect the sensors before starting the application. Discovery runs when the main window is created and assigns devices to the two display slots.
3. Open [MultiKinectSystem.sln](MultiKinectSystem.sln), build the WPF project and run it. Each panel displays a 640 × 480 depth image.

[MainWindow.xaml](MultiKinectSystem/MainWindow.xaml) defines the two panels. [MainWindow.xaml.cs](MultiKinectSystem/MainWindow.xaml.cs) handles device discovery, depth-frame events and rendering.

## Arm tracking and simulation

The [Unity project](Arm%20tracking%20and%20simulation%20system/Boneknitting%20Technique) includes Kinect scripts and recorded motion files. Its [editor version](Arm%20tracking%20and%20simulation%20system/Boneknitting%20Technique/ProjectSettings/ProjectVersion.txt) is 2017.3.0f3. For the original walkthrough, see the [project instructions](Arm%20tracking%20and%20simulation%20system/Project%20instrction.docx).

The original project description also records an OpenPose integration. The entry point documented above opens the Kinect depth viewer.
