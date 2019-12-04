using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    private Character characterUnderMouse = null;
    private Character draggingCharacter = null;
    private CrownObject draggingCrown = null;
    private Node nodeUnderMouse = null;
    private CrownObject crownUnderMouse = null;
    private int NodeLayer;
    private int CharLayer;

    // Start is called before the first frame update
    void Start()
    {
        NodeLayer = LayerMask.NameToLayer("Node");
        CharLayer = LayerMask.NameToLayer("Character");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50f, 1 << CharLayer))
        {
            this.characterUnderMouse = hit.collider.transform.root.GetComponent<Character>();
        }
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50f, 1 << NodeLayer))
        {
            this.nodeUnderMouse = hit.collider.transform.root.GetComponent<Node>();
        }
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50f, 1 << CharLayer)){
            this.crownUnderMouse = hit.collider.transform.root.GetComponent<CrownObject>();
        }
        if (Input.GetMouseButtonDown(0) && this.draggingCharacter == null &&this.draggingCrown == null && this.characterUnderMouse != null && this.characterUnderMouse.type.Equals("ally" ))
        {
            if (!UnitManager.instance.gameStarted)
            {
                this.draggingCharacter = this.characterUnderMouse;
                this.draggingCharacter.prevNode = this.draggingCharacter.currNode;
            }
        }
        else if(Input.GetMouseButtonDown(0) && this.draggingCharacter == null && this.draggingCrown == null && this.crownUnderMouse != null)
        {
            this.draggingCrown = this.crownUnderMouse;
            this.draggingCrown.prevPosition = this.draggingCrown.transform.position;
        }
        if (this.draggingCharacter != null)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (this.nodeUnderMouse != null && this.nodeUnderMouse.currChar == null && (this.nodeUnderMouse.indices.y < 4||this.nodeUnderMouse.nType == NodeType.Bench))
                {
                    this.draggingCharacter.SetNode(this.nodeUnderMouse, true);
                    this.nodeUnderMouse.SetUnit(this.draggingCharacter);
                    /*this.draggingCharacter.transform.position = this.nodeUnderMouse.transform.position;
                    this.draggingCharacter.prevPosition = this.draggingCharacter.transform.position;*/
                    if (this.draggingCharacter.prevNode != null) this.draggingCharacter.prevNode.currChar = null;
                    this.nodeUnderMouse.currChar = this.draggingCharacter;
                    this.draggingCharacter.prevNode = this.nodeUnderMouse;
                }
                else
                {
                    this.draggingCharacter.SetNode(this.draggingCharacter.prevNode, true);
                }
                this.draggingCharacter = null;
            }
            else
            {
                this.draggingCharacter.transform.position = mousePos;
            }
        }
        if(this.draggingCrown != null)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if(this.characterUnderMouse != null)
                {
                    this.characterUnderMouse.AddCrown();
                    Destroy(this.draggingCrown.gameObject);
                    this.crownUnderMouse = null;
                }
            }
        }
    }

    private void LateUpdate()
    {
        this.characterUnderMouse = null;
        this.nodeUnderMouse = null;
    }
}
