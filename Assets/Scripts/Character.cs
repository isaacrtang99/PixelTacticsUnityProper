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
    //public HealthBar healthBar;
    SpriteRenderer sR;
    Color c_start = Color.white;
    Color c;
    Color c_end = Color.red;
    HealthBar mHB;

    void Start()
    {
        sR = gameObject.GetComponent<SpriteRenderer>();
        c_start = sR.color;
        
        //healthBar.SetSize(0.5f);


        
        this.prevPosition = this.transform.position;
        this.prevNode = null;
        this.cState = CharacterState.Paused;
    }
    public void TakeDamage(int i)
    {
        health -= i;
        if(health <= 0)
        {
            if (type == "ally") {
                UnitManager.instance.RemoveAlly(this);
            }
            else
            {
                UnitManager.instance.RemoveEnemy(this);
            }
            Destroy(this.gameObject);
        }
        c = Color.Lerp(c_start, c_end, (max_health - health) / max_health);

        sR.color = c;
    }
    public void SetNode(Node n, bool setTransform = false)
    {
        Node temp = currNode;
        if (currNode != null)
        {
            currNode.currChar = null;
        }
        if (n.GetUnit() == null)
        {
            n.SetUnit(this);
        }
        if(n.GetUnit()==this)
        {
            if (temp != null)
            {
                temp = null;
            }
            currNode = n;
            RangedAI ai = this.GetComponent<RangedAI>();
            if (ai != null) ai.currPos = n.transform.position;
            else
            {
                MeleeAI mai = this.GetComponent<MeleeAI>();
                if (mai != null) mai.currPos = n.transform.position;
            }
            if (setTransform) this.gameObject.transform.position = n.gameObject.transform.position;
        }
    }
}
