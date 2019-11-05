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
    public GameObject parent;
    public int g = int.MaxValue;
    public int h = int.MaxValue;
    public int f = int.MaxValue;
    public bool inClosed = false;
}
public class PathFind : MonoBehaviour
{
    public GameObject mPrevNode;
    public GameObject mNextNode;
    public GameObject mTargetNode;
    List<GameObject> mPath;
    Dictionary<GameObject, PathValues> nodeValues;
    List<GameObject> openSet;
    MeleeState mState;
    float moveTimer;
    // Start is called before the first frame update
    void Start()
    {
        mState = MeleeState.Move;
        mPrevNode = null;
        mNextNode = null;
        mTargetNode = null;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    bool PathFinder(GameObject startNode, GameObject goalNode)
    {
        mPath.Clear();
        openSet.Clear();
        nodeValues.Clear();
        if (goalNode = null) return false;
        GameObject currentNode = startNode;
        nodeValues[currentNode].inClosed = true;
        nodeValues[currentNode].g = 0;
        do
        {
            Node node = currentNode.GetComponent<Node>();
            for (int i = 0; i < node.mAdjacent.Count; i++)
            {
                if(node.mAdjacent[i].GetComponent<Node>().currObject == null)
                {
                    GameObject gO = node.mAdjacent[i];
                    bool inOpen = openSet.Find(o => o == gO);
                    if (nodeValues[gO].inClosed)
                    {
                        continue;
                    }
                    else if (inOpen)
                    {
                        int newG = CalculateG(gO , currentNode);
                        if(newG < nodeValues[gO].g)
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
                        if(gO != mPrevNode)
                        {
                            openSet.Add(gO);
                        }
                    }
                }
            }
            if (openSet.Count == 0) break;
            GameObject smallest = openSet[0];
            int minF = nodeValues[openSet[0]].f;
            foreach(GameObject g in openSet)
            {
                if(nodeValues[g].f < minF)
                {
                    minF = nodeValues[g].f;
                    smallest = g;
                }
            }
            currentNode = smallest;
            nodeValues[smallest].inClosed = true;
            openSet.Remove(smallest);
        } while (currentNode != goalNode);
        if(currentNode == goalNode)
        {
            mTargetNode = goalNode;
            mPrevNode = startNode;
            GameObject n = mTargetNode;
            mPath.Add(mTargetNode);
            while(nodeValues[n].parent != null && n!= startNode)
            {
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
        return nodeValues[currNode].g + (cNode.indices.x - pNode.indices.x + cNode.indices.y - pNode.indices.y);
    }
    int CalculateH(GameObject currNode, GameObject endNode)
    {
        Node cNode = currNode.GetComponent<Node>();
        Node eNode = endNode.GetComponent<Node>();
        return (eNode.indices.x - cNode.indices.x) + (eNode.indices.y - cNode.indices.y);
    }
}
