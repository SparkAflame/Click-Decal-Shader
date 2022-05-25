# Mouse Click Decal Shader

![A Mouse Click](Docs/MouseClick.png)

## Script : Scripts/AnimateClickDecal.cs

This script animates the decal and is expected to be attached to the same game object or prefab as the
URP decal projector component.  See "Prefabs/Click Decal" for an example of its use.

### Properties

Property          | Description
---               | ---
Show Duration     | How many complete cycles of the effect are shown.  May be fractional.
Speed             | How quickly the animation plays.  Negative speeds play the effect backwards.

## Script : Scripts/***ClickDecalManager.cs

These scripts manage spawning and destroying decals.  Three versions are provided:

1. `BasicClickDecalManager` - multiple decals, no object pooling.
1. `MovingClickDecalManager` - single decal, can follow mouse position.
1. `PoolingClickDecalManager` - BasicClickDecalManager with object pooling.

### Properties

Property          | Description
---               | ---
Decal Prefab      | The prefab containing the decal projector.
Maximum Decals    | The maximum number of simultaneous decals this manager can spawn.

## Shader : "Shaders/Graphs/Decal/PulsingCircleDecal"

All the shader graph does is to colour the output from the subgraph "SDF Circle Pulse" ("Shaders/SubGraphs/SDF Circle Pulse").  The subgraph contains all the code to generate the pulsing circle effect.

### Sub-Graph : "Shaders/SubGraphs/SDF Circle Pulse"

Contains all the code for generating the pulsing circle effect.

The outputs from several of these nodes can be combined to create more interesting composite effects.

### Material : "Materials/Click Decal"

This material exists as a baseline with sensible defaults.

#### Material (Shader) Properties

Property          | Description
---               | ---
Colour            | The pulse colour.
Pulse Width       | [0.0, 1.0] The pulse width.  Note that this essentially clamps the lower bound of the output from the sine function and thus is not linear.
Flatness          | [0.0, 1.0] How much to flatten the sine wave.  0.0 = no flattening, larger values flatten it more.
Radial Fade       | How quickly the effect fades from the centre. 0.0 = no fade; < 1.0 = fade slower; >= 1.0 = fade faster.
Number of Circles | How many circles to draw.
Centre Spot Size  | [0.0, 1.0] Size of the centre spot.  0.0 disables.
Centre Spot Fade  | [0.0, 1.0] Time over which the centre spot fades at the end of one cycle of the effect.
Time Offset       | Controls the effect's animation.  Normally this is just the elapsed time since the effect was triggered.  Decreasing values cause the effect to be reversed with the circle pulsing inwards instead of outwards.
