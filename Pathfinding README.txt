This is a little guide to setting up the pathfinding. First we'll try to get some basic pathfollowing going for the target on the first level.

I have uploaded the scripts needed. They're in main project folder (NOT assets).

Summary:
-Parent the LevelManager object to the Gameplay Object under FullCanvas, and parent the Target to the LevelManager object
-Copy S_Pathfinder, PfNode and Tile scripts to new folder in Scripts called GridScripts and get rid of duplicates (Circle and Movement don't need any location)
-Assign LevelManager script to its respective object, the Tile script to each tile prefab and the Movement script to the Target
-Assign Tile script to each tile prefab (don't worry about values, this is done when generating the level)
-Assign Movement script to the Target and give it the LevelManager object and it's RigidBody2D
-Remember that Y coordinates are inverted, so keep that in mind when testing that the coordinates start at (0, 0) at the top left

Hope this is helpful. If you need to get in touch you can send an email to (saldhous@outlook.com). Also send an email or slack message explaining what's going on like tasks and such. I will check tomorrow morning before I leave.

##########################################################################################################

First make a new folder called GridScripts so we can nicely organize them. Then add the S_Pathfinder, LevelManager, PfNode and Tile scripts to this folder (and make sure the LevelManager script is replaced).

The new LevelManager contains the array of tile objects as well as information about the map and an example in the Start method of how you might use the pathfinding. You simply call the S_Pathfinder which is a static class with a method called PathFind that needs the 2D array of tile objects, the starting coordinate as a Vector2D and a target coordinate, also as a Vector2D. This method will output a queue of tile objects. In the example it works through the queue until its empty, tinting tiles blue as it goes. Note that the actual starting tile is never included in the queue. Remember that the Y coordinates for the map are flipped when trying to test or use the pathfinding. The LevelManager will also have the tile objects parented to it. Make sure that the LevelManager script is actually assigned since we just replaced it.

Now add the Tile script to each tile prefab. Also  This will store general data about the tile so the pathfinder can create a simple fresh grid of nodes to work with (Using A* for enemies means that you cannot simply supply them with the map data itself). The LevelManager script will then alter tile data based on index.

Now with the Target (Or enemy object) add the Movement script. Currently the script Start method has a hardcoded position when it starts, so when the time comes we can get rid of that and instantiate it at the desired location. I'll try and have a crack at this tomorrow so don't worry too much about that. For now just give it the RigidBody2D and the object that contains the LevelManager script. It will attempt to pathfind by getting the inverted Y coordinate (The coordinate according to the grid since the Y axis of the map is upside down). This one works a bit differently to the example in the LevelManager. Here is some pseudo code to give you an idea.

If (PathQueue is not empty)
	If (Movement Circle contains the next Tile Position)
		Snap to tile
		Adjust MovementCircle
		Set velocity to 0
	Else
		Move object to tile at specified speed
Else
	Set Velocity to 0 (Stops dead if there is no more tiles to travel to)



