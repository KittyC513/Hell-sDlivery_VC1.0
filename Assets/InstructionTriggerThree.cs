using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionTriggerThree : MonoBehaviour
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
            if (!SceneControl.instance.firstButtonIsTriggered3)
            {
                canvasUI.SetActive(true);

            }
            else
            {
                canvasUI.SetActive(false);
            }

            SceneControl.instance.inDropArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Package"))
        {
            canvasUI.SetActive(false);
            SceneControl.instance.firstButtonIsTriggered3 = false;
        }
    }
}
