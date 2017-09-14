# sweep-3d-scanner-unity-viewer
A simple unity project to view scans created by the [open source 3D scanner project](https://github.com/scanse/sweep-3d-scanner) in first person, including VR support.


Main Viewer                |  Miniature Viewer
:-------------------------:|:-------------------------:
![main viewer](https://s3.amazonaws.com/scanse/3D-Scanner/docs/imgs/viewer/main_viewer.PNG)  |  ![miniature viewer](https://s3.amazonaws.com/scanse/3D-Scanner/docs/imgs/viewer/miniature_viewer.PNG)

## Download
- You can clone this repository and open the cloned directory as a project in Unity.
- Alternatively, you can download the executables from the latest release [here](https://github.com/scanse/sweep-3d-scanner-unity-viewer/releases).

## Using the Viewers
### Quickstart
- Run the executable `Point_Cloud_Viewer.exe` or `Miniature_Point_Cloud_Viewer.exe`.
- Once the app loads, a file browser should open automatically. 
- Select a PLY (binary) or CSV pointcloud file, with the [expected format](#compatible-file-format).
- Wait for the application to parse the file and render the points. 
> **Note:** binary `.ply` files parse significantly faster than `.csv`
- For the main viewer, use `Q` & `E` to adjust the vertical position of the point cloud so the camera appears to be at head height, and the floor in the scan matches the ground plane. Then walk around inside the scan.
- For the miniature viewer, experiment with the transform controls listed below to view the diorama.
- To exit the application and close the executable, press `ESC`.

### Controls
- `WASD`: movement
- `Mouse`: look around
- `Q` & `E`: adjust the vertical position of the point cloud
- `Z` & `C`: adjust the scale of the point cloud (only available in miniature scene)
- `R` & `T`: adjust the yaw rotation of the point cloud (only available in miniature scene)
- `ESC`: exit application

### Notes on VR
- Support for VR is included.
- Be sure the VR headset is connected and turned on.
- After starting the application executable, select the desired scan file before putting the headset on.
- Large scans can take a little while to load.
- The scan will not appear until you put on the headset.

### Compatible file format
Only files with the expected format will open correctly. Attempting to open unexpected files will terminate the unity application. The application expects either `.csv` or `.ply (binary)` files downloaded from the `sweep-3d-scanner` or exported from the `Sweep Visualizer` desktop application. 

If you want to convert custom data to make a compatible file, create a CSV file where the first line is a header, subsequent rows represent points, and the first 3 columns are `X`, `Y` and `Z`. Optionally, you can include a 4th column called `SIGNAL_STRENGTH`. Example file contents shown below.
```csv
X,Y,Z
6.7,-124.2,-71.3
6.7,-125.4,-69.2
...

or 

X,Y,Z,SIGNAL_STRENGTH
6.7,-124.2,-71.3,199
6.7,-125.4,-69.2,199
...
```

- X,Y,Z: Integer or floating point values representing the x, y and z coordinates for each point. Units are in cm. Assumes a right handed coordinate system, where z is up. Note: this is NOT the same as unity's coordinate system, which is left handed with y up.
- SIGNAL_STRENGTH: integer value between [0:254] indicating strength/confidence in the sensor reading. Higher values are stronger readings, lower values are weaker readings. The color of points in the viewer are determined by this value, by mapping the range [0:254] to the entirety of the HUE spectrum in HSV color space. If you are generating a custom file, and you do NOT have signal strength values for your data points, simply use the same value for every point. 

## Unity Project
### Packages/Dependencies
- `Standard Assets/Characters`: provides first person character controller
- `StandaloneFileBrowser`: enables using the native OS dialog to select a csv file, src available [here](https://github.com/gkngkc/UnityStandaloneFileBrowser).

### Included Assets
- `Scenes/main`: 
  - The main 1 to 1 scale point cloud viewer, best for walking around the scan.
  - Point cloud can be raised or lowered to match the floor height with the ground plane.
- `Scenes/miniature`:
  - Shrinks the point cloud into a diorama size for top down viewing. 
  - Point cloud can be be raised, lowered, rotated and scaled.
- `Scripts/CSVReader`: 
  - Naively parses a csv file of the expected structure into an array of `Vector4` objects. 
  - Each `Vector4` represents a point, where the first 3 elements (x,y,z) are position, and the 4th (w) is normalized signal strength.
- `Scripts/PLYReader`: 
  - Naively parses a ply (binary) file of the expected structure into an array of `Vector4` objects. 
  - Each `Vector4` represents a point, where the first 3 elements (x,y,z) are position, and the 4th (w) is normalized signal strength.
- `Scripts/PointCloudGenerator`: 
  - Requests the user select a `.csv` file.
  - Reads the points from the file.
  - Since no single mesh can hold more than 65000 vertices, splits the points into multiple children each of which is a PointCloud object with a portion of the points. 
- `Scripts/PointCloud`: 
  - Creates a mesh where each vertex is a point, and the color for each vertex is calculated by the normalized signal strength.
- `Scripts/AdjustTransform`: 
  - Allows the user to adjust the transform of the point cloud.
- `Material/Shader_PointCloud`: Shader code, adapted from [here](http://www.kamend.com/2014/05/rendering-a-point-cloud-inside-unity/).

## Contributing:
-------------------
There is a lot of room for improvement, and submitting a pull request is easy! 

So don't hesitate to experiment and modify the code.

To make the process of contributing more efficient, please review the [contribution guidelines](.github/CONTRIBUTING.md).