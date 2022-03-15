using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine.AI
{
    /// <summary>
    /// An AI Node is a node within a graph that can be used to get a NodePath
    /// </summary>
    public class AINode : IHeapItem<AINode>
    {
        /// <summary>
        /// This is a list of our nodes neighbors, used in the A* algorithm to find a path through a network of nodes.
        /// </summary>
        /// <value></value>
        public List<AINode> Neighbours{ get; set; }

		/// <summary>
		/// Position within the world
		/// </summary>
		public Vector3 Position;

		/// <summary>
		/// Oversimplification: Distance from starting node in given path
		/// </summary>
		public float GCost;

		/// <summary>
		/// Oversimplification: Distance from the ending node in a given path
		/// </summary>
		public float HCost;

		//Combination between Gcost and HCost
		public float FCost{
			get{
				return HCost + GCost;	
			}
		}

		/// <summary>
		/// Index in the heap
		/// </summary>
		private int _heapIndex;

		/// <summary>
		/// Implementation of our HeapIndex value
		/// </summary>
		/// <value></value>
		public int HeapIndex {
			get {
				return _heapIndex;
			}
			set {
				_heapIndex = value;
			}
		}

		/// <summary>
		/// Implementation of Icomparable to compare the FCOST and hcost of two nodes
		/// </summary>
		public int CompareTo(AINode nodeToCompare) {
			int compare = FCost.CompareTo(nodeToCompare.FCost);
			if (compare == 0) {
				compare = HCost.CompareTo(nodeToCompare.HCost);
			}
			return -compare;
		}
    }
}
