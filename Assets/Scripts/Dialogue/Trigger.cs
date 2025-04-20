using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Trigger : MonoBehaviour
{

    public DialogueRunner dR;
    [SerializeField]
    private LayerMask layer1;
    [SerializeField]
    private LayerMask layer2;
    [SerializeField]
    public bool conversationStart;



    [SerializeField]
    private List<Collider> colliderInTriggerZone = new List<Collider>();


    // Start is called before the first frame update
    void Start()
    {
        conversationStart = GameManager.instance.GMconversationStart;
        layer1 = LayerMask.NameToLayer("P1Collider");
        layer2 = LayerMask.NameToLayer("P2Collider");
    }

    // Update is called once per frame


    private void OnTriggerEnter(Collider other)
    {
        if (!colliderInTriggerZone.Contains(other) && !GameManager.instance.GMconversationStart)
        {
            if((other.gameObject.layer == layer1))
            {
                colliderInTriggerZone.Add(other);
            }
            if ((other.gameObject.layer == layer2))
            {
                //conversationStart = true;
                GameManager.instance.GMconversationStart = true;
                //dR.StartDialogue("HubStart");
                Destroy(this.gameObject);

            }
        }

    }


}