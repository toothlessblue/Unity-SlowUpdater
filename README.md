## Unity Slow Updater
This is a kind of alternative to coroutines.

Provides a monobehaviour that can recieve action/framerate pairs, the monobehaviour calls the action
every time `Time.frameCount % framerate` equals 0. 

This is suitable for expensive functions that don't need to be called every frame, only every so often.

Add one instance of the `SlowUpdater` monobehaviour into the first scene on an empty object. The object it
exists on will **not be destroyed between scenes**.

Every time a new scene is loaded, the object is reset.

It uses a singleton design, so getting a reference to the monobehaviour is not necessary.

```
void functionToCall() { ... }

// functionToCall is executed once every 10 frames
SlowUpdater.add(functionToCall, 10); 
```