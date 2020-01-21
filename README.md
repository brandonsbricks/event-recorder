# Event Recorder

## DEV NOTES
To link required dll dependencies, find the following within your local UnityEditor + UnityProject Folders like so:

* Unity.TextMeshPro, UnityEngine.UI.dll <= UnityProjectFolder/Library/ScriptAssemblies
* UnityEngine.dll, UnityEngine.CoreModule.dll, UnityEngine.InputLegacyModule.dll <= Unity2019.2.9f1/Editor/Data/Managed/UnityEngine

## USE NOTES

Records input events and customizable events that users execute during a play session. Can be used to replay a series of ui events and gather other use data 
Recorded events include 
* IPointer...Enter, Exit, Down, Up, and Click, which includes Button clicks and EventTriggers
* Dropdown selection
* Slider changes
* Toggle changes
* Text Input
* TextMeshPro Dropdown option selection
* TextMeshPro Text Input
* Transform position and rotation
* Custom string event data
* Key down/up presses
* Mouse down/up clicks

## OUTPUT
* A json file will automatically be written to disk if the custom Editor Window is open
* To add custom events, add transform recordings, remove keyboard recordings, extract the recording data for storage and analysis
* To preserve strong typing and minimize deserialization type-casting cost, distinct types of event data are stored in separate type-grouped arrays. 
* For any non-uniquely named items in the scene hierarchy, sibling indexing is automatically added to the gameobject name for distinction. The name format will appear as `parent/childName;i=0/subChild/babyName;i=3` where the index follows `;i=` 
* To record events in a live build, you must interface with the UnityEventGatherer component. This will hook your event recorders into the unity lifecycle, expose configuration for recordings (remove certain recordings, add custom recorders)

## EXAMPLE PROJECT INTEGRATION
* Examples to come
