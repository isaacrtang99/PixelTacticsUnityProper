﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinAI : AIBase
{
    public bool jumped;

    public override void attackNode(Node n)
    {
        if (n == null) return;
        if (n.currChar != null)
        {
            n.currChar.TakeDamage(this.mCharacter.damage);
        }
    }

    protected override MeleeState calculateState()
    {
        foreach (Node n in this.mCharacter.currNode.mAdjacent)
        {
            if (n.GetUnit() != null)
            {
                if (n.GetUnit().type == targetType)
                {
                    this.targetAttackNode = n;
                    return MeleeState.Attack;
                }
            }
        }
        if (this.jumped)
        {
            return MeleeState.Move;
        }
        else
        {
            int maxDist = -1;
            Node myNode = this.mCharacter.currNode;
            Node jumpTarget = null;
            foreach (Character c in targets)
            {
                Node n = c.currNode;
                if (n.nType == NodeType.Bench) continue;
                if (n.GetUnit() != null)
                {
                    int dist = Mathf.Abs(n.indices.x - myNode.indices.x) + Mathf.Abs(n.indices.y - myNode.indices.y);
                    if (dist > maxDist)
                    {
                        foreach (Node neighbor in n.mAdjacent)
                        {
                            if (neighbor.currChar != null) continue;

                            maxDist = dist;
                            jumpTarget = neighbor;
                        }
                    }
                }
            }

            if (jumpTarget != null)
            {
                this.jumped = true;
                this.mCharacter.SetNode(jumpTarget);
                this.mCharacter.prevNode = this.mCharacter.currNode;
                this.prevPos = this.transform.position;
                this.currPos = this.mCharacter.currNode.transform.position;
                return MeleeState.Stay;
            }
            else return MeleeState.Move;
        }
        

        return MeleeState.Move;
    }
}
