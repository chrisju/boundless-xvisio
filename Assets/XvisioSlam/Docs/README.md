# Use XisioTech Slam in Unity

To use the product, you can access it in 3 different ways, game objects, scripts, and API.



  ### 1. Use Game Objects

You can use the prefabs we provide in `XvisioSlam/Prefabs` by following the steps bellow.

1. Drag the XvSlam prefab into the scene.

2. Set the position and rotation of XvSlam to `(0, 0, 0)` and `(0, 0, 0)`. XvSlam will automatically follow the slam's movement when the game starts. 

3. There are some attributes of XvSlam you can set:

   1.  `Pos Scale`: If you set it to `5`, XvSlam will move `5` meters when the slam move `1` meter in the real world.
   2. `Allowed Max Wrong State Frame Numbers`: If you set it to `10`, the slam will be reset when encountering `10` continuous frames of wrong states.
   3. `Slam Reset Cooldown Time`: This is the duration in seconds that the XvSlam stops requesting the movement of the slam and wait for the slam to reset. 
   4. `User Reset Key`: The key to reset XvSlam back to `(0, 0, 0)` and `(0, 0, 0)`.

4. After having one XvSlam in the scene, you can then have multiple game objects follow the movement of XvSlam. For example, drag the XvSlamCamera prefab into the scene. Since this prefab already carries the script `XvSlamBehaviour.cs`, it will follow the movement of XvSlam.

5. There are some attributes of`  XvSlamBehaviour` you can set:

   1.  `Pos Scale`: This is the scale between the object and XvSlam. For example, if you set it to `2` and set the `Pos Scale` in XvSlam to `5`, then the object will move `2 x 5 = 10` meters when the slam moves `1` meter in the real world. In other words, you can consider XvSlam's `Pos Scale` as a global scale to all objects in the scene, and XvSlamBehaviour's `Pos Scale` is for the use of individual objects.

6. Note that XvSlam is reponsible for communicating with the slam. It handles the request, update, and reset of the slam. So always put one and only one XvSlam in the scene and leave it at `(0, 0, 0), (0, 0, 0)`. 

   

------




### 2. Use scripts

Instead of using our prefabs, you can drag the scripts in `XvisioSlam/Scripts` directly to your own objects. Please follow the steps bellow:

1. Create one and only one empty game object. Drag the script `XvSlam.cs` to it, and set the position and rotation to `(0, 0, 0), (0, 0, 0)`. 
2. Drag `XvSlamBehaviour.cs` to any other game objects that you want them to follow the movement of the slam.



#### Details Explanation

1. `XvSlam.cs`: This scripts handles the communication with the slam, it will request, update, and reset the slam automatically. Then it'll reflect the movement of the slam on the object it is attached to. You'll always need one and only one object carrying the script, and set the position and rotation to `(0, 0, 0), (0, 0, 0)`. Instead of directly attach this script to your game objects, you should create an empty object carrying this script and set to `(0, 0, 0), (0, 0, 0)`. 
2. There are some attributes of `XvSlam.cs` you can set:
   1.  `Pos Scale`: If you set it to `5`, the object will move `5` meters when the slam move `1` meter in the real world.
   2. `Allowed Max Wrong State Frame Numbers`: If you set it to `10`, the slam will be reset when encountering `10` continuous frames of wrong states.
   3. `Slam Reset Cooldown Time`: This is the duration in seconds that the XvSlam stops requesting the movement of the slam and wait for the slam to reset. 
   4. `User Reset Key`: The key to reset XvSlam back to `(0, 0, 0)` and `(0, 0, 0)`.
3. `XvSlamBehaviour.cs`: Objects carrying this script will follow the movement of XvSlam. You can have your multiple game objects carrying this script and set different scale to them.
4. There are some attributes of`  XvSlamBehaviour.cs` you can set:
   1.  `Pos Scale`: This is the scale between the object and XvSlam. For example, if you set it to `2` and set the `Pos Scale` in `XvSlam.cs` to `5`, then the object will move `2 x 5 = 10` meters when the slam moves `1` meter in the real world. In other words, you can consider `XvSlam.cs`'s `Pos Scale` as a global scale to all objects in the scene, and `XvSlamBehaviour`'s `Pos Scale` is for the use of individual objects.



------



### 3. Use API

Instead of using `XvSlam.cs` we provide, you can also communicate with the slam using our XvHid API.

When having proper platform backend libraries, you can access a class called `XvHid` and its method when using namespace `Xvisio`.

You should always have only one `XvHid` instance working, multiple objects sending commands to the slam might cause unexpected results.

1. `XvHid()`: When you instantiate a `XvHid`, it will try to open the slam and set the slam to 6 dof output mode. When the hid is successfully set up, it will log  `XvHid set up. ` to the console. You can then call the `update()` method.
2. `void update()`: Every time when you want to get the newest info from the slam, you have to call this method first, and then call `position(), eulerAngles(), rotation(), state(), frameNo()` to get info.
3. `Vector3 position()`: Return the position (displacement) of the slam. The origin is where the slam started or last reset.
4. `Vector3 eulerAngles()`: Return the Euler angles (degree) of the rotation of the slam. The origin is where the slam started or last reset. The rotation is in ZYX sequence.
5.  `Quaternion rotation()`: Return the rotation of the slam. The origin is where the slam started or last reset.
6. `void resetSLAM(bool fullReset)`: Reset the slam. Used when the slam is stuck in a wrong state over a period of time. When `fullReset` is true, the map will be cleared.
7. `void request6Dof(bool enable)`: Start of stop the 6 dof output mode. Note that `request6Dof(true)` was called once in `XvHid()`.
8. `int state() `: Return the state of the slam. States other than 2 imply the slam is not working correctly.
9. `int frameNo()`: Return the frame number of the slam. Stuck frame numbers imply the slam is not working correctly.
10. `bool setup()`: Return true if the device was correctly setup during the construction. If returns false, other operations on XvHid should not be carried on.

Note that shaking the slam heavily will make it stuck in a same frame number or in lost states. In such condition, you have to call the functions below to make it work again:

```c#
xvHid = new XvHid();

...
...
...

// When the slam is stuck

xvHid.request6Dof(false);
xvHid.resetSLAM(true);
xvHid.request6Dof(true);

// Then you can call update() again

xvHid.update();
```

 For clearer usage of `XvHid` API, please refer to `XvSlam.cs`'s codes.