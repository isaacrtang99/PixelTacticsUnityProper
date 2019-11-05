using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]
    public Bench bench;
    [SerializeField]
    public PlayArea playArea;
    [SerializeField]
    public Transform benchNodesParent;
    [SerializeField]
    public Transform playAreaNodesParent;
}
