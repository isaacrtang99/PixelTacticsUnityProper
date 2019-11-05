using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class PlayArea : Object
{
    [SerializeField]
    public List<List<Node>> boardNodes;
    public int width;
    public int height;

    public PlayArea(int x, int y, GameObject nodePrefab, Transform parent)
    {
        this.boardNodes = new List<List<Node>>();
        for(int i = 0; i < x; i++)
        {
            this.boardNodes.Add(new List<Node>());
            for(int j = 0; j < y; j++)
            {
                GameObject newNodeObj = (GameObject)PrefabUtility.InstantiatePrefab(nodePrefab, parent);
                this.boardNodes[i].Add(newNodeObj.GetComponent<Node>());
            }
        }
        this.width = x;
        this.height = y;
    }
}
