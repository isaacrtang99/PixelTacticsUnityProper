using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAI : AIBase
{
    public GameObject mProjectile;

    public override void attackNode(Node n)
    {
        if (n == null) return;
        if (n.currChar != null)
        {
            GameObject p = Instantiate(mProjectile, this.gameObject.transform.position, Quaternion.identity) as GameObject;
            p.GetComponent<Projectile>().startPosition = this.gameObject.transform.position;
            p.GetComponent<Projectile>().targetNode = n;
            p.GetComponent<Projectile>().targetCharacter = n.currChar;
            p.GetComponent < Projectile >().damage = gameObject.GetComponent<Character>().damage;
            if (this.mCharacter.type == "enemy")
            {
                p.GetComponent<Projectile>().color = "green";
            }
        }
    }

    protected override MeleeState calculateState()
    {
        foreach (Character c in targets)
        {
            Node n = c.currNode;
            if (n.GetUnit() != null)
            {
                Node myNode = this.mCharacter.currNode;
                int dist = Mathf.Abs(n.indices.x - myNode.indices.x) + Mathf.Abs(n.indices.y - myNode.indices.y);
                if (n.GetUnit().type == this.targetType && dist <= 4)
                {
                    Debug.Log("Found enemy to attack");
                    this.targetAttackNode = n;
                    return MeleeState.Attack;
                }
            }
        }

        return MeleeState.Move;
    }
}
