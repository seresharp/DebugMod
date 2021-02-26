Original mod by KeinZantezuken. Remaking using canvas instead of imgui and the modding framework created by myself and MyEyes/Firzen.

----------------------------------------------------------------------------------------
                                       FEATURES
----------------------------------------------------------------------------------------
* A completely new toggleable UI in game that provides the following functions:
* Cheats such as invincibility and noclip
* The ability to unlock all charms or repair broken ones
* Change which skills the player has
* Change which items the player has
* Give the player more of consumable resources such as geo and essence
* Respawn bosses
* Hold multiple dream gate positions
* Change the player's respawn point to anywhere in the current scene
* Recall to the set respawn point
* Kill all enemies
* Add HP bars to enemies
* Draw collision boxes for enemies
* Clone or delete any enemy
* Set an enemy's health to 9999
* Change the player's nail damage
* Damage the player
* Change the camera zoom level
* Disable the in game HUD
* Make the player invisible
* Disable the lighting around the player
* Disable the vignette drawn around the player
* Change the time scale of the game
----------------------------------------------------------------------------------------
                                  INSTALLATION
----------------------------------------------------------------------------------------
##(STEAM, WINDOWS)
1) Download the modding API from here: https://cdn.discordapp.com/attachments/298798821402607618/797126249457385482/Assembly-CSharp.dll
2) Right click Hollow Knight in Steam -> Properties -> Local Files -> Browse Local Files
3) Create a backup of the game files located here
4) Copy the contents of the modding API zip into this folder (Overwrite files when asked)
5) Copy the contents of this zip into the folder (Overwrite files when asked)
6) This mod should not affect saves negatively, but it is a good idea to back them up anyway.
   Saves are located at %AppData%\..\LocalLow\Team Cherry\Hollow Knight\
      
## How to build (for devs):
1) Make a folder `Source/References/`, then 
2) add `Assembly-CSharp.dll` (with modding-api), and `PlayMaker.dll`, `UnityEngine.dll` and `UnityEngine.UI.dll` from your `Hollow Knight/hollow_knight_Data/Managed/`-folder
----------------------------------------------------------------------------------------
                                          CREDITS
----------------------------------------------------------------------------------------
Coding - Seresharp
SaveStates/Current Patch - 56, Yurihaia, Krythom
UI design and graphics - The Embraced One
Assistance with canvas - Katie

----------------------------------------------------------------------------------------
                                      SAVESTATE BASICS
---------------------------------------------------------------------------------------- 
*All savestates are loaded in RAM. If this causes performance issues for you, please report it.*

To use numpad for slot select; after installing debugmod, start and stop the game, 
then go to the Hollow Knight saves-directory and open the `DebugMod.GlobalSettings` json-file.
In that file find `"NumPadForSaveStates"`, and change the corresponding value from 0 to 1.

Savestates files are located in `%APPDATA%\..\LocalLow\Team Cherry\Hollow Knight\Savestates-1221\`. They use the name format `savestate<slot>.json`.
After saving a savestate to file, you can edit the name of that savestate. To do this, open the file in any text-editor, and the first variable/line should be something like `"saveStateIdentifier": "<timestamp+area/scene name>",`. Change `<timestamp+area/scene name>` in the second pair of `"`-s to whatever you want that savestate named in the select savestate in-game menu.

## Quickslot: 
The main savestate used. Not saved permanently.

## Quickslot save to file
Specifies slot number, then saves the current Quickslot from temporary memory to a numbered json-file in the game save directory, overwriting any files with identical number as the selected one.

## Load file to quickslot
Asks to specify slot number, then reads the json-file with that number from the game save directory and loads it into the Quickslot, overwriting any current savestate there.

## Save new state to file 
Specifies slot number, then makes a new savestate and saves to a json-file with the given slot number.

## Load new state from file 
Specifies slot number, then loads savestate from that file directly.

----------------------------------------------------------------------------------------
                                      Known Issues
---------------------------------------------------------------------------------------- 
## Savestates:
* Charm effect lingering after loading a different savestate
* UI not refreshing properly to remove obsolete vessel fragments
* Soul meter stuck at full if in that state before loading
* Softlocks if loading savestate during dream transitions