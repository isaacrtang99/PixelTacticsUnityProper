using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MeleeState
{
    Move,
    Attack
}
public class PathValues
{
    public GameObject parent = null;
    public int g = int.MaxValue;
    public int h = int.MaxValue;
    public int f = int.MaxValue;
    public bool inClosed = false;
}
public class Pathfind : MonoBehaviour
{
    public GameObject mPrevNode;
    public GameObject mNextNode;
    public GameObject mTargetNode;
    public List<GameObject> mPath;
    Dictionary<GameObject, PathValues> nodeValues;
    List<GameObject> openSet;
    public MeleeState mState;
    float moveTimer;
    // Start is called before the first frame update
    void Start()
    {
        openSet = new List<GameObject>();
        mState = MeleeState.Move;
        mPrevNode = null;
        mNextNode = null;
        mTargetNode = null;
        nodeValues = new Dictionary<GameObject, PathValues>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    public bool PathFinder(GameObject startNode, GameObject goalNode)
    {
        mPath.Clear();
        openSet.Clear();
        nodeValues.Clear();
        if (goalNode == null) return false;
        GameObject currentNode = startNode;
        PathValues pV = new PathValues();
        nodeValues.Add(currentNode, pV);
        nodeValues[currentNode].inClosed = true;
        nodeValues[currentNode].g = 0;
        do
        {
            Node node = currentNode.GetComponent<Node>();
            for (int i = 0; i < node.mAdjacent.Count; i++)
            {
                if (node.mAdjacent[i].GetComponent<Node>().currChar == null)
                {
                    GameObject gO = node.mAdjacent[i];
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
            GameObject smallest = openSet[0];
            int minF = nodeValues[openSet[0]].f;
            foreach (GameObject g in openSet)
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
            GameObject n = mTargetNode;
            mPath.Add(mTargetNode);
            while (nodeValues[n].parent != null && n.GetComponent<Node>() != startNode.GetComponent<Node>())
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
    int CalculateG(GameObject currNode, GameObject parentNode)
    {
        Node cNode = currNode.GetComponent<Node>();
        Node pNode = parentNode.GetComponent<Node>();
        return nodeValues[parentNode].g + (cNode.indices.x - pNode.indices.x + cNode.indices.y - pNode.indices.y);
    }
    int CalculateH(GameObject currNode, GameObject endNode)
    {
        Node cNode = currNode.GetComponent<Node>();
        Node eNode = endNode.GetComponent<Node>();
        return (eNode.indices.x - cNode.indices.x) + (eNode.indices.y - cNode.indices.y);
    }
}
