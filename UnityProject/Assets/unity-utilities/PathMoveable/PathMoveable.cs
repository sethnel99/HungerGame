// Copyright (c) 2012, Jørgen P. Tjernø <jorgenpt@gmail.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

// Uncomment the following line to see a lot of details in the Debug window when we get a new path.
//#define DEBUG_PATH

using UnityEngine;
using System.Collections;
using ExtensionMethods;

#if !NO_PATH

[RequireComponent (typeof (CharacterController))]
[RequireComponent (typeof (Navigator))]
public class PathMoveable : MonoBehaviour
{
	// Whether or not we ignore the y component (height) of incoming 'move' requests.
	public bool shouldIgnoreHeightOfDestination = false;

	// If we're less than this many seconds from arriving at a destination, we say it's good enough.
	public float remainingDistanceTolerance = 0.05f;

	// State about current destination, currentPath == null means we're not moving anywhere.
	Path currentPath;
	Waypoint currentWaypoint;
	Vector3 currentDestination;

	// You can override this in a subclass, see EditorSpeedMoveable.
	protected virtual float GetSpeed() { return 2f; }

	// We use this to make sure we "fall" to the ground even if we stop moving.
	bool isGrounded = false;
	
	bool autoRotate = true;
	float maxRotatonSpeed = 10f;

	// We use these to ensure that we stop moving if we're stuck.
	float movedDistanceSince;
	double movedDistance;
	
	private CharacterController controller;
	
	void Start()
	{
		controller = GetComponent<CharacterController>() as CharacterController;
	}

	void Update ()
	{		
		if (IsMoving ())
		{
			Vector3 toDestination = (currentDestination - transform.position).WithY (0f);;
			float speed = GetSpeed();
			
			//If we are close enough
			if (Vector3.Distance (transform.position, toDestination) < remainingDistanceTolerance * speed)
			{
				Debug.Log ("Tolerance stop moving");
				
				StopMoving ("Top of update");
				gameObject.SendMessage("WanderingDone", 1);
			}
			//else if we arrived
			else if (toDestination.magnitude < remainingDistanceTolerance * speed)
			{
				ArrivedAtDestination ();
			}
			else
			{
				Vector3 oldPosition = transform.position;
				isGrounded = controller.SimpleMove (toDestination.normalized * speed);
				
				// Test to see if we're stuck.
				movedDistance += Vector3.Distance(transform.position, oldPosition);
				float nowTime = Time.time;
				float elapsedTime = nowTime - movedDistanceSince;
				if (elapsedTime > 1f)
				{
					if (movedDistance < speed * elapsedTime / 10f)
					{
						Debug.Log ("Stop from inside moving, we are stuck");
						
						StopMoving ("stuck from inside moving");
						gameObject.SendMessage("WanderingDone", -1);
					}

					movedDistance = 0f;
					movedDistanceSince = nowTime;
				}
			}
			
			Vector3 wtf = new Vector3(toDestination.x, toDestination.y, toDestination.z);
			if(autoRotate && wtf.magnitude > .2) {
				//Add a conditional to prevent snapping back
				Vector3 newForward = ConstantSlerp(
					transform.forward,
					wtf,
					10f*maxRotatonSpeed*Time.deltaTime
				);
				//newForward = Vector3.Project(newForward, transform.up);
	    		transform.forward = newForward;
				//transform.rotation = Quaternion.LookRotation(newForward, transform.up);
			}
		}
		else if (!isGrounded)
		{
			isGrounded = controller.SimpleMove (Vector3.zero);
		}
		
		
		
//		if (autoRotate && toDestination.sqrMagnitude > 0.01) {
//			Vector3 newForward = ConstantSlerp(
//				transform.forward,
//				toDestination,
//				maxRotatonSpeed*Time.deltaTime
//			);
//			newForward = Vector3.Project(newForward, transform.up);
//			transform.rotation = Quaternion.LookRotation(newForward, transform.up);
//		}
	}
	
	Vector3 ConstantSlerp (Vector3 fromVec, Vector3 toVec, float angle) {
 		float val = Mathf.Min(1, angle / Vector3.Angle(fromVec, toVec));
 		return Vector3.Slerp(fromVec, toVec, val);
	}

	void ArrivedAtDestination ()
	{
		if (currentWaypoint == null)
		{
			// This means there are no more waypoints, so we're there. Yay!
			StopMoving ("arrived at destination");
			gameObject.SendMessage("WanderingDone", 1);
		}
		else
		{
			// This removes currentWaypoint from currentPaht.Segments.
			currentPath.ArrivedAt (currentWaypoint);

			if (currentPath.Segments.Count > 0)
			{
				// If there are any Segments of this Path left, then navigate to the end of the first remaining segment.
				currentWaypoint = currentPath.Segments[0].To;
				currentDestination = currentWaypoint.Position;
			}
			else
			{
				// Otherwise, we're through navigating the segments and can go directly to the end position.
				currentWaypoint = null;
				currentDestination = currentPath.EndPosition;
			}
		}
	}

	// Move to a given position.
	public void StartMovingTo (Vector3 position)
	{
        //Debug.Log("start moving to: " + position);
		if (shouldIgnoreHeightOfDestination)
			position.y = transform.position.y;

		GetComponent<Navigator> ().targetPosition = position;
	}

	// Stop moving completely.
	public void StopMoving (string sourceText="")
	{
		//Debug.Log ("STOP MOVING! " + sourceText);
		GetComponent<Navigator> ().targetPosition = transform.position;
		currentPath = null;
	}

	void OnNewPath (Path path)
	{
		if (path == null)
		{
			Debug.Log ("OnNewPath(null), stopping movement.");
			StopMoving ("on new path");
			gameObject.SendMessage("WanderingDone", -1);
			return;
		}

		movedDistanceSince = Time.time;
		movedDistance = 0f;
		
		
		currentPath = path;
		// For some reason, it sometimes gives us direct paths that go via an arbitrary waypoint.
		if (path.StartNode == null || GetComponent<Navigator> ().DirectPath (path.EndPosition))
		{
			currentWaypoint = null;
			currentDestination = path.EndPosition;
		}
		else
		{
			currentWaypoint = path.StartNode;
			currentDestination = currentWaypoint.Position;
		}

#if DEBUG_PATH
		Debug.Log ("Start position: " + path.StartPosition);
		Debug.Log ("Start node: " + path.StartNode + (path.StartNode != null ? ", position:" + path.StartNode.Position : ""));

		int i = 1;
		foreach (Connection conn in path.Segments)
			Debug.Log (" Segment #" + i++ + ": " + conn.From + " -> " + conn.To);

		Debug.Log ("End position: " + path.EndPosition);
		Debug.Log ("End node: " + path.EndNode + (path.EndNode != null ? ", position:" + path.EndNode.Position : ""));
#endif
	}

	void OnTargetUnreachable ()
	{
		currentPath = null;
		gameObject.SendMessage("WanderingDone", -1);
		Debug.Log ("Could not pathfind to target position");
	}

	void OnPathInvalidated (Path path)
	{
		Debug.Log ("The path is no longer valid.");
		if (path == currentPath)
		{
			// Try to recalculate the path.
			StopMoving ("on path invalidated");
			GetComponent<Navigator> ().ReSeek ();
		}
	}

	void OnDrawGizmos ()
	{
		if (IsMoving ())
			currentPath.OnDrawGizmos ();
	}

	public bool IsMoving ()
	{
		return currentPath != null;
	}
}

#endif

// vim: set noexpandtab sw=8 sts=8 ts=8:
