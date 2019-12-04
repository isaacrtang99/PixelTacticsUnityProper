using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAI : AIBase
{
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

        return MeleeState.Move;
    }
}
