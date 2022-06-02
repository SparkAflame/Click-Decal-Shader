# Mouse Click Decal Shader

This is a simple pulsing circle decal shader for Unity's URP to visualise the
position of a mouse click, finger tap, etc.  It resembles the effect seen in
some top-down click-to-move games.  It is written using Shader Graph and full
source is included.

![A Mouse Click](Packages/Click%20Point%20Decal%20Shader/Docs/MouseClick.png)

Decal shaders were first added to URP in version 12.0.  Consequently this
project requires Unity 2021.3.0 or later with URP 12.1.6 or later.
Unfortunately the decal feature in URP 12 is incomplete hence there are some
minor issues with the shader.  I expect some if not all of these issues to be
fixed eventually but they are beyond my control - we are entirely reliant on
Unity to make the necessary changes.

Details about the scripts and shaders can be found here:
[Readme.md](Packages/Click%20Point%20Decal%20Shader/Readme.md)

Two modes are supported by default: (1) A single decal that follows the mouse
and pulses for as long as the mouse button is pressed; and (2) one or more
static decals - one spawned each time the mouse button is pressed.

> Note: The project is significantly changed since version 1.0.0.  If you are
> using that version in your project and replace it with this one then unless
> you're only using the shader you will likely find that it "breaks" your
> project. I appologise for any inconvenience but I hope you'll find the new
> version better and easier to get working "out of the box".

## Quick Start 1 - Full Project

If you want to download the full project so that you can run the demo scene and
see the shader in action:

1. Clone this entire repository.
   
   ```shell
   > git clone "https://github.com/SparkAflame/Click-Decal-Shader.git"
   ```

1. Open and run the project.  The "Mouse Click Decal Shader Demo" scene should
   already be open; if not then open it before running the project.

1. Using the left mouse button click on any surface.  Mouse button input and
   decal spawning is handled by the "Mouse Button Handler" scene object and
   associated script.

1. To customise the shader select the "Click Decal" material (Packages / Click
   Point Decal Shader / Materials). It controls attributes such as colour, pulse
   size, number of pulses, etc.

1. To change the projected size of the decal change the width, height and
   projection depth on the URP Decal Projector component on the "Click Decal"
   prefab (Packages / Click Point Decal Shader / Prefabs).  The prefab also
   controls the effect's duration and playback speed.

1. To switch between multiple static decals or a single decal that follows the
   mouse: select the "Mouse Button Handler" scene object and drag one of the
   three scene objects named "Click Decal Manager (...)" into the `Decal Manager`
   field.
   
1. The shader source can be found in "Packages/Click Point Decal Shader".

Copy the folder "Packages/Click Point Decal Shader" to the Packages folder of
your project if you would like to use the shader in your own project.

## Quick Start 2 - Just the Package

If you just want the package containing the shader and manager scripts:

1. Ensure your project is correctly set up:

   * URP is installed, is version 12.1.6 or later, and is correctly configured.
   * The Decal Renderer feature is installed:
      [https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@12.0/manual/renderer-feature-decal.html]()

1. Install the package into your project using either Unity's built-in Package
   Manager, or a git command line:

   * Package Manager: Open the Package Manager window and add the package using this URL:
     https://github.com/SparkAflame/Click-Decal-Shader.git?path=Packages/Click%20Point%20Decal%20Shader

   * GIT: Open a git command line, change the working directory to your project's
      "Packages" folder and do a sparse checkout of this repository (git 2.25.0 and
      later):
      
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


# Using

The easiest way to get started is to use one of the three prefabs provided:

* Click Decal Manager (Basic)
  
  Spawns and destroys decal game objects on demand; objects are not pooled or
  cached.  This prefab supports spawning multiple decals when the mouse button
  is pressed again before the first decal has expired.  Once spawned the decals
  do not move with the mouse pointer.

  This version generates GC.

* Click Decal Manager (Moving Pointer)
  
  This decal manager supports just one decal but allows the decal to be moved
  around without interrupting the effect.

* Click Decal Manager (Pooled)
  
  This is an extension to Click Decal Manager (Basic) that pools the decal game
  objects.  To save CPU cycles the decal game objects are disabled when not in
  use.

The demo scene allows you to experiment with each of these.  Just drag the 
appropriate scene object into the Decal Manager field of the Mouse Button
Handler scene object.

Examine the demo scene to see how the prefabs are used.


# Character Controllers

Integrating the effect into your character controller is way beyond the scope
of this document, but the demo scene has the `MouseButtonHandler` script which
shows how the effect might be controlled if you are using the legacy
`InputManager`.

Below is a very basic example using Oscar Gracián's Easy Character Movement 2
(no affiliation, I'm just a customer).

## Example: Easy Character Movement 2

```c#
using EasyCharacterMovement;
using SparkAflame.ClickShader;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SparkAflame.Scripts.Characters.Players.ECM2
{
   /// <summary>
   ///    A minimal navmesh based point-n-click character controller using Oscar Gracián's
   ///    Easy Character Movement 2.
   /// </summary>
   public class PlayerAgentCharacter : AgentCharacter
   {
      [Header( "Player Specific" )]
      [SerializeField]
      private BasicClickDecalManager _decalManager;

      private bool _firstClick;

      protected override void OnMouseClick( InputAction.CallbackContext context )
      {
         base.OnMouseClick( context );

         if ( context.started )
         {
            _firstClick = true;
         }
      }

      public override void MoveToLocation( Vector3 location )
      {
         if ( _firstClick )
         {
            _decalManager.SpawnAt( location, Vector3.up );

            _firstClick = false;
         }
         base.MoveToLocation( location );
      }
   }
}
```
