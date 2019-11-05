using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Vector2 prevPosition;
    public Node prevNode;

    private void Start()
    {
        this.prevPosition = this.transform.position;
        this.prevNode = null;
    }
}
