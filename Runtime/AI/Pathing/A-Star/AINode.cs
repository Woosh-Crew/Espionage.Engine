using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine.AI
{
    /// <summary>
    /// An AI Node is a node within a graph that can be used to get a NodePath
    /// </summary>
    public class AINode : Entity
    {
        /// <summary>
        /// This is a list of our nodes neighbors, used in the A* algorithm to find a path through a network of nodes.
        /// </summary>
        /// <value></value>
        public List<AINode> Neighbours{ get; set; }

    }
}
