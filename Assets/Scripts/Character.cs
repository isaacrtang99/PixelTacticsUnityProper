using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterState
{
    Paused,
    Active
}
public class Character : MonoBehaviour
{
    public Vector2 prevPosition;
    public Node prevNode;
    public Node currNode;
    public string type;
    public CharacterState cState;
    public int health;
    void Start()
    {
        this.prevPosition = this.transform.position;
        this.prevNode = null;
        this.cState = CharacterState.Paused;
    }
    public void TakeDamage(int i)
    {
        health -= i;
        if(health <= 0)
        {
            Debug.Log("Took Damage");
            Destroy(this.gameObject);
        }
    }
    public void SetNode(Node n)
    {
        Node temp = currNode;
        if (currNode != null)
        {
            currNode.currChar = null;
        }
        if (n.GetUnit() == null || n.GetUnit().Equals(null))
        {
            n.SetUnit(this);
            if (temp != null)
            {
                temp = null;
            }
            currNode = n;
            this.gameObject.transform.position = n.gameObject.transform.position;
            n.SetUnit(this);
        }
        else if(n.GetUnit()==this)
        {
            currNode = n;
            this.gameObject.transform.position = n.gameObject.transform.position;
        }
    }
}
