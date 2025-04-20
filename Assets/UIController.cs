using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private Canvas canvas2;

    public List<TargetIndicator> targetIndicators = new List<TargetIndicator>();
    public List<PlayerIndicator> playerIndicators = new List<PlayerIndicator>();

    private GameObject player1;
    private GameObject player2;

    [SerializeField]
    private Camera p1Cam;
    [SerializeField]
    private Camera p2Cam;


    [SerializeField]
    private GameObject TargetIndicatorPrefab;
    [SerializeField]
    private GameObject PlayerIndicatorPrefab;

    // Start is called before the first frame update
    void Start()
    {
        p1Cam = GameManager.instance.cam1.GetComponent<Camera>();
        p2Cam = GameManager.instance.cam2.GetComponent<Camera>();

    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.instance.curSceneName == "Level1" || GameManager.instance.curSceneName == "MVPLevel" || GameManager.instance.curSceneName == "Tutorial")
        {
            if (canvas == null)
            {
                canvas = GameObject.FindWithTag("IndicatorCanvas").GetComponent<Canvas>();
            }
            if (targetIndicators.Count > 0)
            {
                for (int i = 0; i < targetIndicators.Count; i++)
                {
                    targetIndicators[i].UpdateIndicator();
                }
            }

            if (playerIndicators.Count > 0)
            {
                for (int i = 0; i < playerIndicators.Count; i++)
                {
                    playerIndicators[i].UpdateIndicator();
                }
            }
        }
    }

    public void AddTargetIndicator(GameObject target, GameObject player2)
    {
        TargetIndicator indicator = GameObject.Instantiate(TargetIndicatorPrefab, canvas.transform).GetComponent<TargetIndicator>();
        indicator.InitializeIndicator(target, player1, player2, p1Cam, p2Cam, canvas, canvas2);
        targetIndicators.Add(indicator);
    }

    public void AddPlayerIndicator(GameObject target, GameObject player1)
    {
        PlayerIndicator indicator = GameObject.Instantiate(PlayerIndicatorPrefab, canvas.transform).GetComponent<PlayerIndicator>();
        indicator.InitializeIndicator(target, player1, player2, p1Cam, p2Cam, canvas, canvas2);
        playerIndicators.Add(indicator);
    }
}
