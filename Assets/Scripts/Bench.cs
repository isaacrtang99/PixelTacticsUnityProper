using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Bench : Object
{
    [SerializeField]
    public List<Node> benchNodes;

    public Bench(int size, GameObject nodePrefab, Transform parent)
    {
        this.benchNodes = new List<Node>();
        for (int i = 0; i < size; i++)
        {
            GameObject newNodeObj = (GameObject)PrefabUtility.InstantiatePrefab(nodePrefab, parent);
            this.benchNodes.Add(newNodeObj.GetComponent<Node>());
        }
    }
}
