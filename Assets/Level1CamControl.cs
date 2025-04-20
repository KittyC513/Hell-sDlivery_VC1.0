using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1CamControl : MonoBehaviour
{
    public static Level1CamControl instance;

    [SerializeField]
    private float waitingTime;
    [SerializeField]
    private GameObject indicator;
    [SerializeField]
    private GameObject indicatorCanvas;
    [SerializeField]
    private GameObject dialogueCanvasP1;
    [SerializeField]
    private GameObject dialogueCanvasP2;

    [Header("Level 1")]
    [SerializeField]
    public bool atStart;
    [SerializeField]
    private Transform[] cameraPositions;
    [SerializeField]
    private int currentPositionIndex = 0;
    [SerializeField]
    public float transitionSpeed = 1f;
    [SerializeField]
    private Camera mainCam;
    [SerializeField]
    public Camera miniCam;
    [SerializeField]
    public Transform cutSceneMiniCam;
    [SerializeField]
    public bool cutsceneIsCompleted;
    [SerializeField]
    public bool endCutScene;
    string sceneString;

    [Header("MVP Level")]
    [SerializeField]
    private Transform cam;
    [SerializeField]
    private Transform oriCam;
    [SerializeField]
    private bool gameStart;
    [SerializeField]
    private bool gameEnd;

    // Start is called before the first frame update
    private void Awake()
    {

    }
    void Start()
    {

        instance = this;

        Scene scene = SceneManager.GetActiveScene();


        atStart = true;

        sceneString = scene.name;


    }

    // Update is called once per frame
    void Update()
    {

        //if (GameManager.instance.curSceneName == "Level1")
        //{
        //    AtStartCam();
        //}
        //if (GameManager.instance.curSceneName == "MVP")
        //{
            
        //}
        AtStartCam();
        if (gameStart)
        {
            StartCoroutine(minigameCutscene());
        } 

        if (gameEnd)
        {
            StartCoroutine(endminigameCutscene());
        }
        ///AtStartCam();
    }


    #region Level 1
    void AtStartCam()
    {
        if (atStart)
        {
            indicator.SetActive(false);
            indicatorCanvas.SetActive(false);
            GameManager.instance.cam1.SetActive(false);
            GameManager.instance.cam2.SetActive(false);
            GameManager.instance.p1.isFreeze = true;
            GameManager.instance.p2.isFreeze = true;
            mainCam.gameObject.SetActive(true);
            StartCoroutine(MoveToNextCamera());
            atStart = false;
        }

        if (currentPositionIndex >= cameraPositions.Length - 1 && !endCutScene)
        {
            StartCoroutine(StopMoveCam());
            
        }


    }

    IEnumerator MoveToNextCamera()
    {
        yield return new WaitForSeconds(1f);
        while (currentPositionIndex < cameraPositions.Length)
        {
            Transform targetPosition = cameraPositions[currentPositionIndex];
            yield return StartCoroutine(MoveCamera(targetPosition, transitionSpeed, mainCam));
            currentPositionIndex++;
        }
    }

    IEnumerator StopMoveCam()
    {
        yield return new WaitForSeconds(3f);
        GameManager.instance.cam1.SetActive(true);
        GameManager.instance.cam2.SetActive(true);
        mainCam.gameObject.SetActive(false);

        GameManager.instance.p1.isFreeze = false;
        GameManager.instance.p2.isFreeze = false;
        cutsceneIsCompleted = true;
        indicator.SetActive(true);
        indicatorCanvas.SetActive(true);
        endCutScene = true;
    }

    public IEnumerator StopMoveCam1()
    {
        StopCoroutine(MoveToNextCamera());
        yield return new WaitForSeconds(0.2f);
        GameManager.instance.cam1.SetActive(true);
        GameManager.instance.cam2.SetActive(true);
        mainCam.gameObject.SetActive(false);

        GameManager.instance.p1.isFreeze = false;
        GameManager.instance.p2.isFreeze = false;
        cutsceneIsCompleted = true;
        indicator.SetActive(true);
        indicatorCanvas.SetActive(true);
        endCutScene = true;
    }

    IEnumerator MoveCamera(Transform targetPos, float lerpSpeed, Camera cam)
    {
        float startTime = Time.time;
        float journeyLength = Vector3.Distance(cam.transform.position, targetPos.position);

        while (Vector3.Distance(cam.transform.position, targetPos.position) > 0.1f)
        {
            float distCovered = (Time.time - startTime) * lerpSpeed;
            float fractionOfJourney = distCovered / journeyLength;

            cam.transform.position = Vector3.Lerp(cam.transform.position, targetPos.position, fractionOfJourney);
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, targetPos.rotation, fractionOfJourney);

            yield return null;
        }
    }

    #endregion

    public void minigameCam()
    {
        indicator.SetActive(false);
        indicatorCanvas.SetActive(false);
        gameStart = true;


    }

    public void endminigameCam()
    {
        gameEnd = true;
    }

    IEnumerator minigameCutscene()
    {
        GameManager.instance.p1.isFreeze = true;
        GameManager.instance.p2.isFreeze = true;
        GameManager.instance.cam1.SetActive(false);
        GameManager.instance.cam2.SetActive(false);
        if(GameManager.instance.curSceneName == "MVPLevel")
        {
            miniCam.gameObject.SetActive(true);
            StartCoroutine(movingCam());
        }
        else
        {
            miniCam.gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(3f);
        GameManager.instance.p1.isFreeze = false;
        GameManager.instance.p2.isFreeze = false;
        gameStart = false;
    }

    IEnumerator movingCam()
    {
        print("MovingCam");
        yield return new WaitForSeconds(1f);
        MoveCamera(cam);
    }


    IEnumerator movingCamBack()
    {
        yield return new WaitForSeconds(1f);
        MoveCamera(oriCam);
    }

    public void MoveCamera(Transform newPos)
    {
        float lerpSpeed = 1f;
        miniCam.transform.position = Vector3.Lerp(miniCam.transform.position, newPos.position, Time.deltaTime * lerpSpeed);
        miniCam.transform.rotation = Quaternion.Lerp(miniCam.transform.rotation, newPos.rotation, Time.deltaTime * lerpSpeed);
        
    }



    IEnumerator endminigameCutscene()
    {
        StartCoroutine(movingCamBack());
        GameManager.instance.p1.isFreeze = true;
        GameManager.instance.p2.isFreeze = true;
        if (dialogueCanvasP1 != null)
        {
            dialogueCanvasP1.SetActive(false);
        }
        if (dialogueCanvasP2 != null)
        {
            dialogueCanvasP2.SetActive(false);
        }
        yield return new WaitForSeconds(3f);
        indicator.SetActive(true);
        indicatorCanvas.SetActive(true);
        GameManager.instance.cam1.SetActive(true);
        GameManager.instance.cam2.SetActive(true);
        miniCam.gameObject.SetActive(false);
        GameManager.instance.p1.isFreeze = false;
        GameManager.instance.p2.isFreeze = false;
        if (dialogueCanvasP1 != null)
        {
            dialogueCanvasP1.SetActive(true);
        }
        if (dialogueCanvasP2 != null)
        {
            dialogueCanvasP2.SetActive(true);
        }
        if (sceneString == "Level1")
        {

            GameManager.instance.p1.transform.position = new Vector3(393.5f, 18, -271);
            GameManager.instance.p2.transform.position = new Vector3(393.5f, 18, -271);


        } else if (sceneString == "MVPLevel")
        {
            GameManager.instance.p1.transform.position = new Vector3(-173, 61, -297);
            GameManager.instance.p2.transform.position = new Vector3(-173, 61, -297);
        }
        gameEnd = false;
    }

}
