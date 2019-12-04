using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector2 startPosition;
    public float moveTime = 0.0f;
    public Node targetNode;
    public Character targetCharacter;
    public string color;
    public float damage;
    SpriteRenderer sR;
    // Start is called before the first frame update
    void Start()
    {
        sR = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(color == "green")
        {
            sR.color = Color.green;
        }
        this.gameObject.transform.position = Vector2.Lerp(startPosition, targetNode.gameObject.transform.position, moveTime* 5);
        if(moveTime >= .2)
        {
            if (targetNode.currChar == targetCharacter && targetCharacter!=null)
            {
                targetCharacter.TakeDamage(damage);
            }
            Destroy(this.gameObject);
        }
        moveTime += Time.deltaTime;
    }
}
