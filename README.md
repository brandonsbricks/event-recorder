# Event Recorder

## DEV NOTES
To link required dll dependencies, find the following within your local UnityEditor + UnityProject Folders like so:

* Unity.TextMeshPro, UnityEngine.UI.dll <= UnityProjectFolder/Library/ScriptAssemblies
* UnityEngine.dll, UnityEngine.CoreModule.dll, UnityEngine.InputLegacyModule.dll <= Unity2019.2.9f1\Editor\Data\Managed\UnityEngine

## USE NOTES

Records ui events that users execute during a play session. Can be used to replay a series of ui events and gather game/app use data 
Recorded events include 
* IPointer...Enter, Exit, Down, Up, and Click, which includes Button clicks and EventTriggers
* Dropdown option selection
* Slider changes
* Toggle changes
* Text Input

* TextMeshPro Dropdown option selection
* TextMeshPro Text Input

## OUTPUT
* A json file will be written to disk only if the custom Editor Window is open
* For dropdown options or any other non-uniquely named item in the scene hierarchy, indexing is automatically added to the gameobject name for distinction. The name format will appear as `parent/gameobjectName;i=#` where the string separator is `;i=` and the index is `#`
* To record ui events in a live build, you must interface with the EventService class

## EXAMPLES
* Examples to come
