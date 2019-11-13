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
    public float health;
    public float max_health;
    SpriteRenderer sR;
    Color c_start = Color.white;
    Color c;
    Color c_end = Color.red;

    void Start()
    {
        sR = gameObject.GetComponent<SpriteRenderer>();
        c_start = sR.color;
        this.prevPosition = this.transform.position;
        this.prevNode = null;
        this.cState = CharacterState.Paused;
    }
    public void TakeDamage(int i)
    {
        health -= i;
        if(health <= 0)
        {
            GameObject unitManager = GameObject.Find("UnitManager");
            if (type == "ally") {
                unitManager.GetComponent<UnitManager>().RemoveAlly(this);
            }
            else
            {
                unitManager.GetComponent<UnitManager>().RemoveEnemy(this);
            }
            Destroy(this.gameObject);
        }
        c = Color.Lerp(c_start, c_end, (max_health - health) / max_health);
        sR.color = c;
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
