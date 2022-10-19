# DECO3801-Team-Mono-2022 (003 New Physical Arcade)

Team project by Shirin, Enrique, Paul, Amit, Jen, and Jaleel

## How to Compile and Run
### Steps

1. Download the repository
2. From Unity Hub, go to 'Projects' and select 'Open'
3. Navigate to the repository and 'click' open. Unity will resolve missing packages and launch the project in the Unity Editor.
4. Read step 1 of Arduino Controller to set up the controls for your device. If you are using serial input, follow the rest of them too. 
5. Move on to Run In-Editor or Run Standalone Build depending on the configuration you want.

### Arduino Controller

1. Navigate to Assets/Scripts/Player/PlayerControls.cs and change boolean UseSerialInput to true if you have serial input hooked up, otherwise false to use keyboard input.
2. On Windows, go to Device Manager and under Ports, find what number COM the Arduino is connected to and change that COM number next to 1. in the code.
3. Connect each black wire to the ground rail on the breadboard. 
4. Connect each red wire to digital ports 2-7 on the arduino in order from the leftmost chair to the rightmost chair when facing the screen. 
(That is, first player left chair is digital port 2, first player forward chair is digital port 3, first player right chair is digital port 4, 
second player left chair is digital port 5, second player forward chair is digital port 6, second player right chair is digital port 7.)
5. Connect GND port on the arduino to the ground rail on the breadboard and the 3.3V port on the arduino to the power rail on the breadboard.
6. Plug the arduino into the computer with the arduino to usb cable.
7. In the Arduino IDE, upload the code found at Assets/Scripts/Arduino/Arduino\ Code.ino. 

### Run In-Editor

1. The project may not load with a built scene. If this is the case, in the 'Project' tab, navigate to 'Scenes' and double-click 'Menu'. This will open the main menu scene.
2. Press the 'Play' icon in the top middle to run the game through the editor.

### Run Standalone Build

1. Navigate to 'Files -> Build Settings'. Make sure the scenes are in the order [Menu, Context, Game].
2. Ensure that the target platform is relevant to your machine.
3. There may be extra settings you want to customise such as fullscreen and resolution. These are available in 'Player Settings' located in the bottom left of the Build Settings window.
4. Click 'Build and Run', which will automatically run the game for you once it is built.

## Data Dependencies
There are no external datasets used in this codebase. All images, videos, sounds etc. are available in this repository.

## Software Dependencies
- Unity Hub >= 3.2.0
- Unity Editor >= 2021.3.8f1
- Arduino IDE >= 2.0.0

## Code References
- [AudioManager](https://www.youtube.com/watch?v=6OT43pvUyfY)