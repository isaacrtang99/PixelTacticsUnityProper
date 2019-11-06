using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Board))]
public class BoardCreator : Editor
{
    GameObject benchNodePrefab;
    GameObject playAreaNodePrefab;
    Vector2Int playAreaSize;
    int benchSize;

    void OnEnable()
    {

    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        this.benchNodePrefab = (GameObject)EditorGUILayout.ObjectField("Bench Node Prefab", this.benchNodePrefab, typeof(GameObject), false);
        this.playAreaNodePrefab = (GameObject)EditorGUILayout.ObjectField("Play Area Node Prefab", this.playAreaNodePrefab, typeof(GameObject), false);

        // get play area and bench size
        this.playAreaSize = EditorGUILayout.Vector2IntField("Play Area Size", this.playAreaSize);
        this.benchSize = EditorGUILayout.IntField("Bench Size", this.benchSize);

        Board board = (Board)this.target;

        if (GUILayout.Button("Create Board"))
        {
            if (this.playAreaSize.magnitude != 0 && this.benchSize != 0 && this.benchNodePrefab != null && this.playAreaNodePrefab != null)
            {
                this.ClearBoard();
                this.CreateBoard();
            }
        }
        else if (GUILayout.Button("Clear Board"))
        {
            this.ClearBoard();
        }
    }

    public void ClearBoard()
    {
        Board board = (Board)this.target;
        List<Transform> toDelete = new List<Transform>();
        foreach(Transform t in board.benchNodesParent)
        {
            toDelete.Add(t);
        }
        
        foreach(Transform t in toDelete)
        {
            Debug.Log("destroy immediate in board creator");
            DestroyImmediate(t.gameObject);
        }
        toDelete.Clear();

        foreach (Transform t in board.playAreaNodesParent)
        {
            toDelete.Add(t);
        }
        foreach(Transform t in toDelete)
        {
            Debug.Log("destroy immediate in board creator");
            DestroyImmediate(t.gameObject);
        }

        board.bench = null;
        board.playArea = null;
        this.serializedObject.ApplyModifiedProperties();
    }

    public void CreateBoard()
    {
        Board board = (Board)this.target;
        PlayArea playArea = new PlayArea(this.playAreaSize.x, this.playAreaSize.y, this.playAreaNodePrefab, board.playAreaNodesParent);
        Bench bench = new Bench(this.benchSize, this.benchNodePrefab, board.benchNodesParent);

        board.playArea = playArea;
        board.bench = bench;
        this.serializedObject.ApplyModifiedProperties();

        float currX = 0;
        float currY = 0;

        for (int i = 0; i < board.bench.benchNodes.Count; i++)
        {
            board.bench.benchNodes[i].transform.position = new Vector3(currX, currY, 0);
            currY += 1;
        }
        currY = 0;
        currX += 3;

        int xIndex = 0;
        int yIndex = 0;
        for (int i = 0; i < board.playArea.width; i++)
        {
            for(int j = 0; j < board.playArea.height; j++)
            {
                board.playArea.boardNodes[i][j].transform.position = new Vector3(currX, currY, 0);
                currY += 1;
            }
            currX += 1;
            currY = 0;
        }
    }
}
