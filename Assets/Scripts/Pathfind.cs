using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MeleeState
{
    Move,
    Attack,
    Stay
}
public class PathValues
{
    public Node parent = null;
    public int g = int.MaxValue;
    public int h = int.MaxValue;
    public int f = int.MaxValue;
    public bool inClosed = false;
}
public class Pathfind : MonoBehaviour
{
    public Node mPrevNode;
    public Node mNextNode;
    public Node mTargetNode;
    public List<Node> mPath;
    Dictionary<Node, PathValues> nodeValues;
    List<Node> openSet;
    public MeleeState mState;
    float moveTimer;
    // Start is called before the first frame update
    void Start()
    {
        openSet = new List<Node>();
        mState = MeleeState.Move;
        mPrevNode = null;
        mNextNode = null;
        mTargetNode = null;
        nodeValues = new Dictionary<Node, PathValues>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    public bool PathFinder(Node startNode, Node goalNode)
    {
        mPath.Clear();
        openSet.Clear();
        nodeValues.Clear();
        if (goalNode == null) return false;
        Node currentNode = startNode;
        PathValues pV = new PathValues();
        nodeValues.Add(currentNode, pV);
        nodeValues[currentNode].inClosed = true;
        nodeValues[currentNode].g = 0;
        do
        {
            Node node = currentNode;
            for (int i = 0; i < node.mAdjacent.Count; i++)
            {
                if (node.mAdjacent[i].currChar == null)
                {
                    Node gO = node.mAdjacent[i];
                    if (!nodeValues.ContainsKey(gO))
                    {
                        PathValues pv = new PathValues();
                        nodeValues.Add(gO, pv);
                    }
                    bool inOpen = openSet.Find(o => o == gO);
                    if (nodeValues[gO].inClosed)
                    {
                        continue;
                    }
                    else if (inOpen)
                    {
                        int newG = CalculateG(gO, currentNode);
                        if (newG < nodeValues[gO].g)
                        {
                            nodeValues[gO].parent = currentNode;
                            nodeValues[gO].g = newG;
                            nodeValues[gO].f = nodeValues[gO].g + nodeValues[gO].h;
                        }
                    }
                    else
                    {
                        nodeValues[gO].parent = currentNode;
                        nodeValues[gO].g = CalculateG(gO, currentNode);
                        nodeValues[gO].h = CalculateH(gO, goalNode);
                        nodeValues[gO].f = nodeValues[gO].g + nodeValues[gO].h;
                        if (gO != mPrevNode)
                        {
                            openSet.Add(gO);
                        }
                    }
                }
            }
            if (openSet.Count == 0) break;
            Node smallest = openSet[0];
            int minF = nodeValues[openSet[0]].f;
            foreach (Node g in openSet)
            {
                if (!nodeValues.ContainsKey(g))
                {
                    PathValues pv = new PathValues();
                    nodeValues.Add(g, pv);
                }
                if (nodeValues[g].f < minF)
                {
                    minF = nodeValues[g].f;
                    smallest = g;
                }
            }
            currentNode = smallest;
            nodeValues[smallest].inClosed = true;
            openSet.Remove(smallest);
        } while (currentNode != goalNode);
        if (currentNode == goalNode)
        {
            mTargetNode = goalNode;
            mPrevNode = startNode;
            Node n = mTargetNode;
            mPath.Add(mTargetNode);
            while (nodeValues[n].parent != null && n != startNode)
            {
                Debug.Log("Added to mPath");
                n = nodeValues[n].parent;
                mPath.Add(n);
            }
            mNextNode = mPath[mPath.Count - 1];
            mPath.RemoveAt(mPath.Count - 1);
            return true;
        }
        return false;
    }
    int CalculateG(Node currNode, Node parentNode)
    {
        return nodeValues[parentNode].g + (currNode.indices.x - parentNode.indices.x + currNode.indices.y - parentNode.indices.y);
    }
    int CalculateH(Node currNode, Node endNode)
    {
        return (endNode.indices.x - currNode.indices.x) + (endNode.indices.y - currNode.indices.y);
    }
}
