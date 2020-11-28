# Unity Parallax 2D

### Written by Jordan Bleu

## Overview

This is my implementation of 2d parallaxing.  I wrote this because tons of the examples you find online are really gross and don't fully work, particularly scrolling in any direction.  

I also wanted a solution that wasn't coupled to any particular use cases.  For example, Some of the examples would require you to add the game object as a child of the camera, etc.  This solution exists totally segregated on its own object, and can exist anywhere in the hierarchy.  This keeps things clean and simple, and it also doesn't clutter up the hierarchy or your codebase at all.


## What is parallaxing?

According to wikipedia:

>...a displacement or difference in the apparent position of an object viewed along two different lines of sight

What that means from a 2D game perspective is we create an illusion of 3D depth by moving separate layers at differing speeds.  Layers that are further in the background move slower, and layers that are closer to the foreground move faster.  This tricks our eyes into seeing real depth in a 2D world, improving the player's immersion and suspension of disbelief.

The best real world example is to think of looking out a car window.  When you look down at the road, it appears to be moving quickly, but looking off into the horizon the world appears to move much slower.  This is the effect we are trying to replicate.

## Examples

Here's some GIFs from some examples from this unity project:

**Note:** These gifs don't loop seamlessly but rest assured they do in the actual projects.

*Example_horizontalParallax scene*
![Side Scroller Example](https://github.com/jordanbleu/unity-parallax-2d/blob/main/Image/gif1.gif?raw=true)

*Example_verticalParallax scene*
![Space Example](https://github.com/jordanbleu/unity-parallax-2d/blob/main/Image/gif2.gif?raw=true)

## How to Use 

1. Create your background layers.  For best results you want the image size to fit in the width of your camera's orthographic size.  In other words, you don't want to see the edges on your image on the camera, or you might see jitters while the parallax effect resets
2. Add these layers to unity as sprites
3. In your hierarchy create empty game objects and add the Parallax Component.  A sprite renderer will magically be added as well.
4. Drag the sprite onto the sprite renderer, and drag your main camera onto the "Follow Camera" inspector spot for the parallax component.  If you're using CineMachine, drag on whatever the active Virtual Camera is at the time.   
5. Repeat for each layer.  Every parallax background layer has its own parallax component. 
6. Set sorting layers for the sprites so they draw in the right order
7. For best results, you want layers that are "closer" to the camera to have a movement offset closer to zero, and layers that are farther to be closer to 1.  See the examples for help on this but basically just tweak it until things look good. 

## Things that don't work (yet)

* Sprite "tiled mode" causes some weirdness.  Maybe someday i'll figure out how to make it work...but since the sprites are already tiled i'm not sure if we need that anyway.






