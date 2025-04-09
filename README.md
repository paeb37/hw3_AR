# a. Your name and UNI.

Brandon Pae, btp2109

# b. Date of submission.

April 9, 2025

# c. Computer and OS version.

AR Section:
- Mac, macOS 15.1 (Sequoia)

VR Section:
- Windows, Windows 11 Home (OS Build 26100.3775)

# d. Mobile device and OS version.

iPhone, iOS 18.3.2

# e. Project title.

AR Section: hw3_AR
VR_Section: hw3_VR

# f. Project directory overview.

hw3_AR:
- Assets/Scenes/SampleScene: This is the main scene
- Assets/Scripts: All necessary scripts (excluding ObjectSpawner)
- Assets/Prefabs folder: Contains our prefabs
- Other relevant imported assets are contained in folders like "Mobile AR Template Assets"

hw3_VR:
- Assets/Scenes/SampleScene: This is the main scene
- Assets/Resources/Prefabs: Contains our prefabs
- Assets/StreamingAssets: Contains the JSON

# g. Special Instructions, if any, for deploying your apps.

To play the game, connect your iPhone and then build and run the AR project using XCode onto your phone. Then in the app, build your dungeon, and press “Export” to save the JSON. Then to get the JSON:

Go to XCode -> Window -> Devices and simulators -> Click on your phone -> Click on your project (hw3_AR) -> Click on the 3 dots -> Download container. Then go to that file (should be a .xcappdata file)- > Show package contents. Then navigate to AppData -> Documents -> data1.json:

Now for the VR portion, take that data1.json and put it under the “Assets/Streaming Assets” folder (you may have to replace the existing file). 

For play testing, make sure that “Oculus” and “Initialize XR on Startup” are selected in Project Settings. Then connect to Windows laptop via Quest Link, then click Play!

# h. Unlisted video URL.

https://youtu.be/jIV-8dpCcOM

# i. Missing features.

I implemented everything in the instructions - but there were two features with small bugs (more details provided in the attached PDF)

1. Scaling entire dungeon view
2. AI navigation for the monsters

# j. Explanation of bugs you found in your code and in any technology you used.

1. Ran into a bug where it does not programmatically select the MainTransform, even though all the other logic was there (explained more in the PDF). If this step worked, then the player would be able to scale the dungeon view easily (by scaling MainTransform, which thus scales all of its children)

2. Ran into an occassional bug for monster AI navigation where monster would start to follow the player, but then teleport to a corner of the floor (explained more in the PDF). So I added a fallback method which would make the monster track and follow the player based on its position and orientation values

# k. Asset attributions.

Assets:
- https://assetstore.unity.com/packages/3d/characters/creatures/dragon-for-boss-monster-pbr-78923?srsltid=AfmBOoozqV4XF3hhFVKJU3mVrIxxRIaBfDWNUDU4CKRNJhw3bjdzcRpH
- https://assetstore.unity.com/packages/3d/characters/creatures/rpg-monster-duo-pbr-polyart-157762
- https://assetstore.unity.com/packages/3d/props/weapons/free-rpg-weapons-199738
- https://assetstore.unity.com/packages/3d/characters/hypercasual-simple-female-male-characters-209163
- https://assetstore.unity.com/packages/3d/props/rust-key-167590

Sounds:
https://freesound.org/people/LilMati/sounds/495541/
https://freesound.org/people/nicktermer/sounds/320247/
https://freesound.org/people/EVRetro/sounds/495005/
https://freesound.org/people/DracoN12/sounds/501846/