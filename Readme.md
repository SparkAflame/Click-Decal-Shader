# Mouse Click Decal Shader

This is a simple pulsing circle decal shader for Unity's URP to visualise the
position of a mouse click, finger tap, etc.  It resembles the effect seen in
some top-down click-to-move games.  It is written using Shader Graph and full
source is included.

Decal shaders were first added to URP in version 12.0.  Consequently this
project requires Unity 2021.2.7 or later with URP 12.1.2 or later.
Unfortunately the decal feature in URP 12 is incomplete hence there are some
minor issues with the shader.  I expect some if not all of these issues to be
fixed eventually but they are beyond my control - we are entirely reliant on
Unity to make the necessary changes.

## Quick Start 1 - Full Project

If you want to download the full project so that you can run the demo scene and
see the shader in action:

1. Clone this entire repository.
   
   ```shell
   > git clone "https://github.com/SparkAflame/Click-Decal-Shader.git"
   ```

1. Open and run the project.  The "Mouse Click Decal Shader Demo" scene should already
   be open; if not then open it before running the project.

1. Using the left mouse button click on any surface.

1. To see how the shader can be customised select the "Mouse Click Decal" object in the scene hierarchy.

   * Play with the shader properties on the "Mouse Click Decal" material.  These control attributes such as   colour, pulse size, number of pulses, etc.
   * Play with the properties on the "Mouse Click Decal Handler" script.  These control the playback speed and whether the animation plays once or continuously when the mouse button is held down.
   
1. The shader source can be found in "Packages/Click Point Decal Shader".

Copy the folder "Packages/Click Point Decal Shader" to the Packages folder of
your project if you would like to use the shader in your own project.

## Quick Start 2 - Just the Shaders

If you just want the package containing the shader:

1. Ensure your project is correctly set up:

   * URP is installed, is version 12.1.2 or later, and is correctly configured.
   * The Decal Renderer feature is installed:
      [https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@12.0/manual/renderer-feature-decal.html]()

1. Note that currently it isn't possible to use the Package Manager to add the
   package from this repo to your project; Package Manager's git support is
   simply too crude.

1. Open a git command line and change working directory to your project's
"Packages" folder.

1. Do a sparse checkout of this repository (git 2.25.0 and later):
   
   ```shell
   > mkdir "Click Decal Shader"
   > cd "Click Decal Shader"
   > git init
   > git remote add -f origin "https://github.com/SparkAflame/Click-Decal-Shader.git"
   > git sparse-checkout init
   > git sparse-checkout set "Packages/Click Point Decal Shader"
   > git pull origin master
   ```
   
   See this Stack Overflow question for more information or if you are using a
   git version >= 1.7.0 and < 2.25.0:
   [https://stackoverflow.com/questions/600079/how-do-i-clone-a-subdirectory-only-of-a-git-repository]().

   If you are using git < 1.7.0 you cannot perform a sparse checkout.  Your
   only option is to clone the entire repository as described above to a
   temporary folder and then copy the folder "Packages/Click Point Decal Shader"
   from that project to "Packages" in yours.
