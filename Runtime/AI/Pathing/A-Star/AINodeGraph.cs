using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Espionage.Engine.AI
{
	/// <summary>
	/// An AINodeGraph holds a list of all the AINodes in a given graph and is responsible for returning and calculating path queries
	/// TODO: Split class into AINodeGraphs and AIPathFinders
	/// Want to do some additional reading on the A* Algorithm, check out this website:
	/// https://www.educative.io/edpresso/what-is-the-a-star-algorithm
	/// </summary>
    public class AINodeGraph : Component<World>
    {
		/// <summary>
		/// The list of Nodes in our graph
		/// TODO: Probably separate out node graphs and a 'pathfinder' class down the line
		/// </summary>
		private List<AINode> Nodes{get; set;}

		/// <summary>
		/// This is responsible for genrating the nodegraph for a given type of AINode
		/// </summary>
		public virtual void Generate<AINode>(){

		}

		/// <summary>
		/// Returns the node in the graph nearest to the given point
		/// </summary>
		public virtual AINode NodeFromWorldPoint(Vector3 worldPosition){
			//Return the closest node to the given world position, regardless of anything in the way of 
			return Nodes.OrderBy( x => Vector3.Distance( x.Position, worldPosition ) ).FirstOrDefault();
		}

		/// <summary>
		/// From a starting position to end ending position
		/// </summary>
		public virtual List<AINode> FindPath(Vector3 startPosition, Vector3 endPosition){
			//Get refernece to start and ending nodes
			AINode start = NodeFromWorldPoint(startPosition);
			AINode target = NodeFromWorldPoint(endPosition);

			//Create a heap for our open set
			Heap<AINode> openSet = new Heap<AINode>(Nodes.Count);
			//Create a hash set for our closed set, ensures unique values
			HashSet<AINode> closedSet = new HashSet<AINode>();
			//Add the start node to the open set
			openSet.Add(start);

			while(openSet.Count > 0){
				//Get the current node in the open set
				AINode current = openSet.RemoveFirst();
				//Add the current node to the 'searched' set
				closedSet.Add(current);

				//If we've reached the target node, return the list of points recursively using each AI nodes parent
				if(current  == target){
					return Retrace(current,target);
				}

				//Now we're gonna iterate over each of the current nodes neighbors and calculate their costs
				foreach(AINode neighbour in current.Neighbours){
					//Skip checking this node if it's already in the closed set
					if(closedSet.Contains(neighbour)){
						continue;
					}
					//Get the distance between the current node and the neighbour and calculate the cost to move to that node
					float movementCostToNeighbour = current.GCost + GetDistance(current,neighbour);
					//If the cost to move to the neighbour is less than the neighbours gcost, or if the neighbour doesn't exist in the open set
					if(movementCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour)){
						//Important:
						//Set the neighbours Gcost (The distance from the starting node)
						neighbour.GCost = movementCostToNeighbour;
						//Set the neighbours HCost (The distance to the ending node)
						neighbour.HCost = GetDistance(neighbour,target);
						//Then set that neighbours parent node to this current node
						neighbour.Parent = current;
						//If our open set does not contain our neighbor, add it
						if(!openSet.Contains(neighbour)){
							openSet.Add(neighbour);
						}
					}
				}
			}
			return null;
		}

		/// <summary>
		/// This will return a list of nodes recursively from their parents, retracing a path from an end node to a start node
		/// </summary>
		public List<AINode> Retrace(AINode start, AINode end){
			//Declare an empty list
			List<AINode> path = new List<AINode>();

			//Keep reference to current node as our end node
			AINode current = end;

			//Add each nodes parent then set the current node to the parent
			while (current != start) {
				path.Add(current);
				current = current.Parent;
			}

			//Reverse the path so it is in order from beginning to end
			path.Reverse();

			return path;
		}

		/// <summary>
		/// Returns the distance between the positions of two nodes
		/// </summary>
		public virtual float GetDistance(AINode a, AINode b){
			return Vector3.Distance(a.Position,b.Position);
		}

    }
}
