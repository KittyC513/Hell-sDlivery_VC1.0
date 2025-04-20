using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetIndicator : MonoBehaviour
{
    public static TargetIndicator instance;

    [SerializeField]
    private Image IndicatorImage;
    [SerializeField]
    private Image IndicatorImageOffScreen;
    [SerializeField]
    private Image p1IndicatorImageOffScreen;
    [SerializeField]
    private Image p2IndicatorImageOffScreen;
    [SerializeField]
    private float outOfSigntOffSet;

    private float outOffSignOffset { get { return outOfSigntOffSet; } }

    [SerializeField]
    private GameObject package;
    [SerializeField]
    private GameObject player1;
    [SerializeField]
    private GameObject player2;

    [SerializeField]
    private Camera p1Cam;
    [SerializeField]
    private Camera p2Cam;

    [SerializeField]
    private RectTransform canvasRect;
    [SerializeField]
    private RectTransform canvasRect2;

    private RectTransform rectTransform;

    [SerializeField]
    public Animator anim;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        instance = this;
        p1Cam = GameManager.instance.cam1.GetComponent<Camera>();
        p2Cam = GameManager.instance.cam2.GetComponent<Camera>();
        //p2IndicatorImageOffScreen.gameObject.SetActive(false);
    }

    public void InitializeIndicator(GameObject package, GameObject player1, GameObject player2, Camera cam, Camera cam2, Canvas canvas, Canvas canvas2)
    {
        this.package = package;
        this.player1 = player1;
        this.player2 = player2;
        this.p1Cam = cam;
        this.p2Cam = cam2;

        canvasRect = canvas.GetComponent<RectTransform>();
    }

    public void UpdateIndicator()
    {
        SetIndicatorPos();
    }

    protected void SetIndicatorPos()
    {
        if(package != null)
        {
            IndicatorImage.enabled = false;
            p2IndicatorImageOffScreen.gameObject.SetActive(false);
            Vector3 indicatorPosition = p1Cam.WorldToScreenPoint(package.transform.position);
            //print("indicatorPosition" + indicatorPosition);

            if (indicatorPosition.z >= 0f & indicatorPosition.x <= canvasRect.rect.width / 2 * canvasRect.localScale.x
            & indicatorPosition.y <= canvasRect.rect.height * canvasRect.localScale.x & indicatorPosition.x >= 0f & indicatorPosition.y >= 0f)
            {
                indicatorPosition.z = 0f;
                targetOutOfSight(false, indicatorPosition);
            }
            else if (indicatorPosition.z >= 0f)
            {
                indicatorPosition = OutOfRangeIndicatorPosition(indicatorPosition);
                targetOutOfSight(true, indicatorPosition);
            }
            else
            {
                indicatorPosition *= -1;
                indicatorPosition = OutOfRangeIndicatorPosition(indicatorPosition);
                targetOutOfSight(true, indicatorPosition);
            }

            rectTransform.position = indicatorPosition;
        }

        if(player2 != null)
        {
            IndicatorImage.enabled = false;
            IndicatorImageOffScreen.gameObject.SetActive(false);
            Vector3 indicatorPosition = p1Cam.WorldToScreenPoint(player2.transform.position);
            //print("indicatorPosition" + indicatorPosition);

            if (indicatorPosition.z >= 0f & indicatorPosition.x <= canvasRect.rect.width / 2 * canvasRect.localScale.x
            & indicatorPosition.y <= canvasRect.rect.height * canvasRect.localScale.x & indicatorPosition.x >= 0f & indicatorPosition.y >= 0f)
            {
                indicatorPosition.z = 0f;
                targetOutOfSight(false, indicatorPosition);
            }
            else if (indicatorPosition.z >= 0f)
            {
                indicatorPosition = OutOfRangeIndicatorPosition(indicatorPosition);
                targetOutOfSight(true, indicatorPosition);
            }
            else
            {
                indicatorPosition *= -1;
                indicatorPosition = OutOfRangeIndicatorPosition(indicatorPosition);
                targetOutOfSight(true, indicatorPosition);
            }

            rectTransform.position = indicatorPosition;
        }

    }

    private Vector3 OutOfRangeIndicatorPosition(Vector3 indicatorPosition)
    {
        indicatorPosition.z = 0f;

        Vector3 canvasCenter = new Vector3(canvasRect.rect.width / 4f, canvasRect.rect.height / 2f, 0f) * canvasRect.localScale.x;
        indicatorPosition -= canvasCenter;

        float divX = (canvasRect.rect.width / 4f - outOffSignOffset) / Mathf.Abs(indicatorPosition.x);
        float divY = (canvasRect.rect.height / 2f - outOffSignOffset) / Mathf.Abs(indicatorPosition.y);

        if(divX < divY)
        {
            float angle = Vector3.SignedAngle(Vector3.right, indicatorPosition, Vector3.forward);
            indicatorPosition.x = Mathf.Sign(indicatorPosition.x) * (canvasRect.rect.width * 0.25f - outOffSignOffset) * canvasRect.localScale.x;
            indicatorPosition.y = Mathf.Tan(Mathf.Deg2Rad * angle) * indicatorPosition.x;
        }
        else
        {
            float angle = Vector3.SignedAngle(Vector3.up, indicatorPosition, Vector3.forward);
            indicatorPosition.y = Mathf.Sign(indicatorPosition.y) * (canvasRect.rect.height / 2f - outOffSignOffset) * canvasRect.localScale.y;
            indicatorPosition.x = -Mathf.Tan(Mathf.Deg2Rad * angle) * indicatorPosition.y;
        }

        indicatorPosition += canvasCenter;
        return indicatorPosition;
    }

    private void targetOutOfSight(bool outOfSight, Vector3 indicatorPosition)
    {

        if (package != null)
        {
            if (outOfSight)
            {
                if (IndicatorImageOffScreen.gameObject.activeSelf == false)
                {
                    IndicatorImageOffScreen.gameObject.SetActive(true);
                }

                //if (IndicatorImage.isActiveAndEnabled == true)
                //{
                //    IndicatorImage.enabled = false;
                //}

                IndicatorImageOffScreen.rectTransform.rotation = Quaternion.Euler(rotationOutOfSigntIndicator(indicatorPosition));
            }
            else
            {
                if (IndicatorImageOffScreen.gameObject.activeSelf == true)
                {
                    IndicatorImageOffScreen.gameObject.SetActive(false);
                }
            }           
        }

        if(player2 != null)
        {

            if (outOfSight)
            {
                if (p2IndicatorImageOffScreen.gameObject.activeSelf == false)
                {
                    p2IndicatorImageOffScreen.gameObject.SetActive(true);
                }
                //if (IndicatorImage.isActiveAndEnabled == true)
                //{
                //    IndicatorImage.enabled = false;
                //}

                p2IndicatorImageOffScreen.rectTransform.rotation = Quaternion.Euler(rotationOutOfSigntIndicator(indicatorPosition));
            }
            else
            {
                if (p2IndicatorImageOffScreen.gameObject.activeSelf == true)
                {
                    p2IndicatorImageOffScreen.gameObject.SetActive(false);
                }

            }
        }
    }
               
   

    private Vector3 rotationOutOfSigntIndicator(Vector3 indicatorPosition)
    {
        Vector3 canvasCenter = new Vector3(canvasRect.rect.width / 4f, canvasRect.rect.height / 2f, 0f) * canvasRect.localScale.x;

        float angle = Vector3.SignedAngle(Vector3.up, indicatorPosition - canvasCenter, Vector3.forward);

        return new Vector3(0f, 0f, angle);
    }

}
