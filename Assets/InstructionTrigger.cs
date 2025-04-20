using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject canvasUI;
    
    // Start is called before the first frame update
    void Start()
    {
        canvasUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Package"))
        {
            if(!SceneControl.instance.firstButtonIsTriggered )
            {
                canvasUI.SetActive(true);
                SceneControl.instance.TurnOnJumpOverUI();
            }
            else 
            {
                canvasUI.SetActive(false);
                SceneControl.instance.TurnOffJumpOverUI();
            }

            SceneControl.instance.inDropArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Package"))
        {
            canvasUI.SetActive(false);
            SceneControl.instance.firstButtonIsTriggered = false;
            SceneControl.instance.TurnOffJumpOverUI();
            SceneControl.instance.inDropArea = false;
        }
    }
}
