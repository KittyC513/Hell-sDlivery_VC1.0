using System.Collections;
using System.Collections.Generic;
using System.IO.Packaging;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using Yarn.Unity;

public class RespawnControl : MonoBehaviour
{

    [Header("Respawn Points")]
    [SerializeField]
    public Vector3 respawnPoint;
    [SerializeField]
    private bool resetRespawnP;
    [SerializeField]
    private Transform P1RespawnRotation;
    [SerializeField]
    private Transform P2RespawnRotation;

    [SerializeField]
    private List<GameObject> cps = new List<GameObject>();
    [SerializeField]
    private List<CheckpointControl> cpc = new List<CheckpointControl>();

    public GameObject cpParent;

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private TestCube testCube;

    [SerializeField]
    public bool isDead;

    [SerializeField]
    private ObjectGrabbable objectGrabbable;
    [SerializeField]
    private GameObject package;

    [SerializeField]
    bool isPlayer1;
    [SerializeField]
    bool isPlayer2;
    [SerializeField]
    public bool Player1Die;
    [SerializeField]
    public bool Player2Die;
    [SerializeField]
    string curSceneName;

    string scene1 = "HubStart";
    string scene2 = "PrototypeLevel";
    string scene3 = "TitleScene";
    string scene4 = "MVPLevel";
    string scene5 = "Tutorial";
    string scene9 = "Level1";

    [SerializeField]
    public bool Player1isCarrying;
    [SerializeField]
    public bool Player2isCarrying;
    public GameObject[] Partner;

    [SerializeField]
    GameManager gameManager;
    [SerializeField]
    private bool isAtDoor;
    [SerializeField]
    public DialogueRunner dRP1, dRP2;
    [SerializeField]
    private GameObject dRGameobject, dRGameobject2;

    public GameObject currentActive;


    [SerializeField]
    private GameObject P1Indicator;
    [SerializeField]
    private GameObject P2Indicator;

    [SerializeField]
    private GameObject P1Shade;
    [SerializeField]
    private GameObject P2Shade;

    bool p1Pass;
    bool p2Pass;

    bool p1Pass1;
    bool p2Pass1;

    bool p1Pass2;
    bool p2Pass2;

    bool p1Pass3;
    bool p2Pass3;

    bool p1Pass4;
    bool p2Pass4;

    bool p1Pass5;
    bool p2Pass5;

    bool p1Pass6;
    bool p2Pass6;

    bool p1Pass7;
    bool p2Pass7;

    bool p1Pass8;
    bool p2Pass8;

    bool p1Pass9;
    bool p2Pass9;

    [SerializeField]
    private GameObject lookAround_T1, moveAroundlookAround_T1, throwPackag_T1, Jump_T1, PressurePlate_T1, checkPoint_T1, summoningCircle_T1, push_T1;

    [SerializeField]
    private GameObject lookAround_T2, moveAroundlookAround_T2, throwPackag_T2, Jump_T2, PressurePlate_T2, checkPoint_T2, summoningCircle_T2, push_T2;


    bool entry;

    [SerializeField]
    private GameObject p1Model, p2Model;
    [SerializeField]
    private bool switchPuzzleCam;

    [Header("Delivery Text")]
    [SerializeField]
    Vector3 respawnPos;
    [SerializeField]
    public bool inDeliveryArea;
    [SerializeField]
    public bool p1AtDoor;
    [SerializeField]
    public bool p2AtDoor;

    [Header("Death Screen")]
    [SerializeField]
    private GameObject p1DeadScreen;
    [SerializeField]
    private GameObject p2DeadScreen;
    [SerializeField]
    private Animator p1Anim;
    [SerializeField]
    private Animator p2Anim;
    [SerializeField]
    private GameObject deathCam;
    [SerializeField]
    private Transform camPosition;
    [SerializeField]
    private Transform camOriPosition;
    [SerializeField]
    private GameObject oriCam;
    [SerializeField]
    private bool addScore1;
    [SerializeField]
    private bool addScore2;


    [Header("Bark")]
    [SerializeField]
    public int previousIndex;
    [SerializeField]
    private bool isTriggered;

    [Header("Barrier")]
    [SerializeField]
    private GameObject barrierUI;



    //CheckpointControl activateFCP;


    List<string> PlayerDeath = new List<string>
    {
        "Huhu",
        "Skill",
        "Imagine",
        "Oof",
        "Hmph",
        "Live",
        "Payless",
        "NoPromotion",
        "Employee",
        "WantPromotion",
        "DyingPurpose",
        "NotLiving",
        "Unaliving",
        "Womp",
        "Recurring",
        "Disappointing",
        "DoYourJob",
        "Slay",
        "DoIt",
        "Nothing",
        "Embarrassing",
        "Zero",
        "Hug",
        "Entertaining",
        "DeliveringDisappointment"
    };

    List<string> SabotageChoice = new List<string>
    {
        "Eenie",
        "WhoCares",
        "HardDecision",
        "MightExplode",
        "WeRoll",
        "LetMePick",
        "Accidentally",
        "RealDemon",
        "AnyThoughts",
        "OMMe",
        "Indecisive",
        "PickAlready",
        "BigDemonPants",
        "TooDifficult",
        "GoingToPick",
    };

    List<string> CooperateChoice = new List<string>
    {
        "NoFun",
        "InHell",
        "Nice",
        "Noo",
        "Wow",
        "WorkForMe",
        "GuardianAngel",
        "Sweet",
        "Teamwork",
        "InLove",
        "Respectfully",
        "Real",
        "AChoice",
        "Interesting",
        "WorstThing",
    };

    List<string> WithPackageReminders = new List<string>
    {
        "Selfish",
        "Tired",
        "Promotion",
        "OnePerson",
        "Hogging",
    };

    List<string> WithoutPackageReminders = new List<string>
    {
        "NiceOfYou",
        "Credit",
        "Steal",
        "OnePerson",
        "Hogging",
    };

    public PlayerSoundbank soundBank;

    private void Start()
    {

        
    }

    void SabotageBark()
    {

        if (SceneControl.instance.p1isKilling && SceneControl.instance.p2isInZone1)
        {
            PlayRandomSabotageDialogue1();
        }

        if (SceneControl.instance.p2isKilling && SceneControl.instance.p1isInZone1)
        {
            PlayRandomSabotageDialogue2();
        }
        

    }

    void SavingBark()
    {

        if (SceneControl.instance.p1IsSaving && SceneControl.instance.p2isInZone1)
        {
            PlayRandomCooperationDialogue1();
        }

        if (SceneControl.instance.p2IsSaving && SceneControl.instance.p1isInZone1)
        {
            PlayRandomCooperationDialogue2();
        }
     
    }


    void PackageBark()
    {

        if (SceneControl.instance.play1WithPackageDialogue)
        {
            PlayRandomPackageDialogue1();
            //SceneControl.instance.p1BarkTriggered = true;

        }

        if (SceneControl.instance.play2WithPackageDialogue)
        {
            PlayRandomPackageDialogue2();
            //SceneControl.instance.p2BarkTriggered = true;
        }

        if (SceneControl.instance.play1WithoutPackageDialogue)
        {
            PlayRandomPackageDialogue3();
            //SceneControl.instance.p1BarkTriggered = true;
        }

        if (SceneControl.instance.play2WithoutPackageDialogue)
        {
            PlayRandomPackageDialogue4();
            //SceneControl.instance.p2BarkTriggered = true;
        }


    }

    void SceneCheck()
    {
        if (gameManager == null)
        {
            gameManager = Object.FindAnyObjectByType<GameManager>();
        }
        if (gameManager.sceneChanged)
        {
            curSceneName = GameManager.instance.curSceneName;
            //if (curSceneName ==scene5 ) 
            //{
            //    if (dRP1 == null)
            //    {
            //        dRGameobject = GameObject.FindWithTag("DRP1");
            //        dRP1 = dRGameobject.GetComponent<DialogueRunner>();
            //    }

            //    if(dRP2 == null)
            //    {
            //        dRGameobject2 = GameObject.FindWithTag("DRP2");
            //        dRP2 = dRGameobject2.GetComponent<DialogueRunner>();
            //    }

            //}

            if (curSceneName == scene4 || curSceneName == scene5 || curSceneName == scene9)
            {
                if (objectGrabbable == null)
                {
                    if (curSceneName == scene4 || curSceneName == scene9)
                    {
                        cpParent = GameObject.FindWithTag("cpParent");

                        foreach (Transform child in cpParent.transform)
                        {
                            cps.Add(child.gameObject);

                            CheckpointControl checkpc = child.gameObject.GetComponent<CheckpointControl>();
                            cpc.Add(checkpc);
                        }
                    }

                    if (curSceneName == "Tutorial")
                    {
                        package = SceneControl.instance.packageTutorial;
                    }
                    else
                    {
                        package = GameObject.FindGameObjectWithTag("Package");
                    }

                    //package = GameObject.FindGameObjectWithTag("HeavyPackage");

                    objectGrabbable = package.GetComponent<ObjectGrabbable>();
                }
                GameManager.instance.p1.objectGrabbable = null;
                GameManager.instance.p2.objectGrabbable = null;
            }

            if (curSceneName == scene1)
            {
                if (gameManager.timesEnterHub >= 1)
                {
                    if(SceneControl.instance.showHeavyPackage || SceneControl.instance.showPackage1)
                    {
                        if (objectGrabbable == null)
                        {
                            package = GameObject.FindGameObjectWithTag("Package");

                            objectGrabbable = package.GetComponent<ObjectGrabbable>();
                        }
                    }

                }
            }


            if (curSceneName != scene5)
            {
                lookAround_T1.SetActive(false);
                moveAroundlookAround_T1.SetActive(false);
                throwPackag_T1.SetActive(false);
                Jump_T1.SetActive(false);
                PressurePlate_T1.SetActive(false);
                checkPoint_T1.SetActive(false);
                summoningCircle_T1.SetActive(false);
                push_T1.SetActive(false);

                lookAround_T2.SetActive(false);
                moveAroundlookAround_T2.SetActive(false);
                throwPackag_T2.SetActive(false);
                Jump_T2.SetActive(false);
                PressurePlate_T2.SetActive(false);
                checkPoint_T2.SetActive(false);
                summoningCircle_T2.SetActive(false);
                push_T2.SetActive(false);

            }

        }
    }

    //void FindDR()
    //{
    //    curSceneName = GameManager.instance.curSceneName;
    //    if(curSceneName == scene5 && dR == null)
    //    {

    //        dR = Object.FindAnyObjectByType<DialogueRunner>();
    //        print("DR FOUND");

    //    }

    //}
    void ResetInitialRespawnPoint()
    {

        if (isPlayer1)
        {
            respawnPoint = SceneControl.instance.P1StartPoint.position;
            //P1RespawnRotation = SceneControl.instance.P1Rotation.rotation;

        }
        if (isPlayer2)
        {
            respawnPoint = SceneControl.instance.P2StartPoint.position;
            //P2RespawnRotation = SceneControl.instance.P2Rotation.rotation;
        }




    }

    private void Update()
    {
        if (curSceneName == "Level1" || curSceneName == "MVPLevel")
        {
            if (!GameManager.instance.barkTriggered)
            {
                SabotageBark();
                SavingBark();
                PackageBark();
            }

        }

        Partner = GameObject.FindGameObjectsWithTag("FindScript");
        //FindDR();
        SceneCheck();
        PlayerDetector();
        if (objectGrabbable != null)
        {
            //Debug.Log("check");
            Player1isCarrying = objectGrabbable.P1TakePackage;
            Player2isCarrying = objectGrabbable.P2TakePackage;
        }

        if (!resetRespawnP && curSceneName == scene4)
        {
            ResetInitialRespawnPoint();
            resetRespawnP = true;
        }
        else if (!resetRespawnP && curSceneName == scene9)
        {
            ResetInitialRespawnPoint();
            resetRespawnP = true;
        }

        //Debug.Log("p1dead" + p1dead);
        //Debug.Log("p2dead" + p2dead);
    }

    private void FixedUpdate()
    {
        if (Player1Die)
        {
            P1Respawn();
            Player1Die = false;
            soundBank.shmonkDeath.Post(player.gameObject);
        }

        if (Player2Die)
        {
            P2Respawn();
            Player2Die = false;
            soundBank.shminkDeath.Post(player.gameObject);
        }
    }
    void PlayerDetector()
    {

        if (this.gameObject.layer == LayerMask.NameToLayer("P1") && !isPlayer1)
        {
            isPlayer1 = true;
        }
        if (this.gameObject.layer == LayerMask.NameToLayer("P2") && !isPlayer2)
        {
            isPlayer2 = true;
        }

    }



    public void Respawn(Vector3 respawnPos)
    {
        if (curSceneName == scene5)
        {
            player.transform.position = respawnPos;
            P1RespawnRotation = SceneControl.instance.P1Rotation;
            GameManager.instance.p2.rC.P2RespawnRotation = P1RespawnRotation;
            player.transform.rotation = P1RespawnRotation.rotation;
        }
        else if (curSceneName == scene4 || curSceneName == scene9)
        {
            player.transform.position = respawnPos;
            if (isPlayer1 && P1RespawnRotation != null)
            {
                player.transform.rotation = P1RespawnRotation.rotation;
                GameManager.instance.p2.rC.P2RespawnRotation.rotation = P1RespawnRotation.rotation;
            }

            if (isPlayer2 && P2RespawnRotation != null)
            {

                player.transform.rotation = P2RespawnRotation.rotation;
                GameManager.instance.p1.rC.P1RespawnRotation.rotation = P2RespawnRotation.rotation;
            }

        }
        else if (curSceneName != scene4 && curSceneName != scene5 && curSceneName != scene9)
        {
            player.transform.position = respawnPos;
        }

        //player.transform.rotation = respawnRotation.rotation;
        //Debug.Log("RespawnPoint =" + respawnPos);
    }
    //IEnumerator RespawnTimer(Vector3 respawnPos)
    //{
    //    yield return new WaitForSeconds(3);
    //    if (Player1Die)
    //    {
    //        player.transform.position = respawnPos;
    //        player.transform.eulerAngles = new Vector3(0, 90, 0);
    //        Player1Die = false;
    //    }
    //    if (Player2Die)
    //    {
    //        player.transform.position = respawnPos;
    //        player.transform.eulerAngles = new Vector3(0, 90, 0);
    //        Player2Die = false;
    //    }

    //}

    public bool p1dead;
    public bool p2dead;

    IEnumerator P1RespawnTimer()
    {
        p1dead = true;
        if(p1DeadScreen != null)
        {
            p1DeadScreen.SetActive(true);
        }

        if(p1Anim != null)
        {
            p1Anim.SetBool("isDead", true);
        }

        GameManager.instance.p1.isFreeze = true;
        //p1Model.SetActive(false);
        //p1DeadScreen.SetActive(true);
        P1Indicator.SetActive(false);
        P1Shade.SetActive(false);
        yield return new WaitForSeconds(2f);

        if(p1Anim != null)
        {
            p1Anim.SetBool("isDead", false);
            p1Anim.SetBool("isRespawn", true);
        }

        if (curSceneName == "HubStart")
        {
            Respawn(bM.waypointExit.transform.position);
        }
        else
        {
            Respawn(respawnPoint);
        }
        //p1Model.SetActive(true);
        //p1DeadScreen.SetActive(false);
        P1Indicator.SetActive(true);
        P1Shade.SetActive(true);
        yield return new WaitForSeconds(1f);
        GameManager.instance.p1.isFreeze = false;
        if(p1Anim != null)
        {
            p1Anim.SetBool("isRespawn", false);
        }

        p1dead = false;
        addScore2 = false;

    }

    //void MoveCam(Transform newPos)
    //{
    //    float lerpSpeed = 10f;
    //    deathCam.transform.position = Vector3.Lerp(deathCam.transform.position, newPos.position, Time.deltaTime * lerpSpeed);
    //    deathCam.transform.rotation = Quaternion.Lerp(deathCam.transform.rotation, newPos.rotation, Time.deltaTime * lerpSpeed);
    //    print("Camera is moving");
    //}

    IEnumerator P2RespawnTimer()
    {
        if(p2DeadScreen!= null)
        {
            p2DeadScreen.SetActive(true);
        }

        if (p2Anim != null)
        {
            p2Anim.SetBool("isDead", true);
        }


        p2dead = true;
        GameManager.instance.p2.isFreeze = true;
        //p2Model.SetActive(false);
        //p2DeadScreen.SetActive(true);
        P2Indicator.SetActive(false);
        P2Shade.SetActive(false);
        yield return new WaitForSeconds(2f);

        if(p2Anim != null)
        {
            p2Anim.SetBool("isDead", false);
            p2Anim.SetBool("isRespawn", true);
        }

        if(curSceneName == "HubStart")
        {
            Respawn(bM.waypointExit.transform.position);
        }
        else
        {
            Respawn(respawnPoint);
        }
        //p2Model.SetActive(true);
        //p2DeadScreen.SetActive(false);
        P2Indicator.SetActive(true);
        P2Shade.SetActive(true);
        yield return new WaitForSeconds(1f);
        GameManager.instance.p2.isFreeze = false;

        if(p2Anim != null)
        {
            p2Anim.SetBool("isRespawn", false);
        }

        p2dead = false;
        addScore1 = false;

    }


    void P1Respawn()
    {

        StartCoroutine(P1RespawnTimer());
        if (curSceneName != scene5 && !addScore2)
        {
            if(curSceneName != "HubStart")
            {
                ScoreCount.instance.AddDeathsToP1(5);
                addScore2 = true;
            }

            //StartCoroutine(ActivateP1UIForDuration(3f));
        }

        if (curSceneName == scene9)
        {
            //LevelDialogue.ShowDevilPlayer2();

            if (!gameManager.barkTriggered)
            {
                SceneControl.instance.dRP1.Stop();
                PlayRandomDeathDialogue1();
            }

        }

        if (Player1isCarrying && isPlayer1)
        {
            if(gameManager.curSceneName == "Level1" || gameManager.curSceneName == "MVPLevel")
            {
                if (!boxingMinigame.instance.isboxing)
                {
                    objectGrabbable.Grab(objectGrabbable.p2ItemC.transform);
                    objectGrabbable.P2TakePackage = true;
                    objectGrabbable.P1TakePackage = false;
                    gameManager.p2.objectGrabbable = package.GetComponent<ObjectGrabbable>();
                }

            }
            else if(gameManager.curSceneName != "Level1" && gameManager.curSceneName != "MVPLevel")
            {
                objectGrabbable.Grab(objectGrabbable.p2ItemC.transform);
                objectGrabbable.P2TakePackage = true;
                objectGrabbable.P1TakePackage = false;
                gameManager.p2.objectGrabbable = package.GetComponent<ObjectGrabbable>();
            }

        }

        Player1Die = false;
    }

    void P2Respawn()
    {

        StartCoroutine(P2RespawnTimer());
        if (curSceneName != scene5 && !addScore1)
        {
            if(curSceneName != "HubStart")
            {
                ScoreCount.instance.AddDeathsToP2(5);
                addScore1 = true;
            }

            //StartCoroutine(ActivateP2UIForDuration(3f));
        }

        if (curSceneName == scene9)
        {
            //LevelDialogue.ShowDevilPlayer1();
            if (!gameManager.barkTriggered)
            {
                SceneControl.instance.dRP2.Stop();
                PlayRandomDeathDialogue2();
            }

        }
        if (Player2isCarrying && isPlayer2)
        {
            if(gameManager.curSceneName == "Level1" || gameManager.curSceneName == "MVPLevel")
            {
                if (!boxingMinigame.instance.isboxing)
                {
                    objectGrabbable.Grab(objectGrabbable.p1ItemC.transform);
                    objectGrabbable.P2TakePackage = false;
                    objectGrabbable.P1TakePackage = true;
                    gameManager.p1.objectGrabbable = package.GetComponent<ObjectGrabbable>();
                }
            }
            else if (gameManager.curSceneName != "Level1" && gameManager.curSceneName != "MVPLevel")
            {
                objectGrabbable.Grab(objectGrabbable.p1ItemC.transform);
                objectGrabbable.P2TakePackage = false;
                objectGrabbable.P1TakePackage = true;
                gameManager.p1.objectGrabbable = package.GetComponent<ObjectGrabbable>();
            }

        }


        Player2Die = false;
    }


    //IEnumerator StartPackageDialogue() 
    //{
    //    SceneControl.instance.dRP1.Stop();
    //    SceneControl.instance.dRP2.Stop();
    //    SceneControl.instance.drAll.Stop();
    //    yield return new WaitForSeconds(1f);

    //    LevelDialogue.ShowDevilPlayerAll();
    //    SceneControl.instance.drAll.StartDialogue("Packages");

    //}


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == ("ChooseZone"))
        {
            if (isPlayer1)
            {
                SceneControl.instance.p1isInZone1 = true;
            }

            if (isPlayer2)
            {
                SceneControl.instance.p2isInZone1 = true;
            }
        }
        if (other.gameObject.tag == ("Hazard") || other.gameObject.tag == ("hazard2"))
        {
            //Debug.Log("Hazard name =" + other.gameObject);
            //Respawn(respawnPoint);

            if (isPlayer1)
            {
                Player1Die = true;
                //print("Player1Die" + Player1Die);
            }
            else
            {
                //dR.Stop();
            }

            if (isPlayer2)
            {
                Player2Die = true;
                //print("Player2Die" + Player1Die);

            }
            else
            {
                //dR.Stop();
            }

        }
        else if (other.tag == "CheckPoint")
        {

            respawnPoint = other.transform.position;
            if (isPlayer1)
            {
                P1RespawnRotation = other.transform.Find("Rotation").transform;

            }

            if (isPlayer2)
            {
                P2RespawnRotation = other.transform.Find("Rotation").transform;

            }

            if (objectGrabbable != null)
            {
                objectGrabbable.respawnPoint = respawnPoint;
            }

            //Debug.Log("RespawnPoint =" + respawnPoint);
        }

        //if (other.tag == "EndingPoint")
        //{
        //    if (isPlayer1 && Player1isCarrying)
        //    {

        //        gameManager.changeSceneTimes += 1;
        //        Loader.Load(Loader.Scene.ScoreCards);
        //    }
        //    if (isPlayer2 && Player2isCarrying)
        //    {
        //        gameManager.changeSceneTimes += 1;
        //        Loader.Load(Loader.Scene.ScoreCards);
        //    }

        //}

        if (other.gameObject.tag == ("Start_Tutorial") && TutorialCamControl.instance.cutsceneIsCompleted)
        {
            if (!p1Pass)
            {

                SceneControl.instance.dRP1.Stop();
                SceneControl.instance.dRP2.Stop();
                LevelDialogue.ShowDevilPlayerAll();
                SceneControl.instance.drAll.StartDialogue("LookAround");
                GameManager.instance.p1.rC.p1Pass = true;
                GameManager.instance.p2.rC.p1Pass = true;


            }
            //if (isPlayer1 && !p1Pass)
            //{
            //    p1Pass = true;
            //    LevelDialogue.ShowDevilPlayer1();
            //    SceneControl.instance.dRP2.Stop();
            //    SceneControl.instance.dRP1.StartDialogue("LookAround");

            //}

            //if (isPlayer2 && !p2Pass)
            //{
            //    p2Pass = true;
            //    LevelDialogue.ShowDevilPlayer2();
            //    SceneControl.instance.dRP1.Stop();
            //    SceneControl.instance.dRP2.StartDialogue("LookAround2");
            //}

            //if (p1Pass && p2Pass)
            //{
            //    Destroy(other.gameObject);
            //}

        }
        if (other.gameObject.tag == ("Jump_Tutorial"))
        {
            if (isPlayer1 && !SceneControl.instance.packageDialogueStart)
            {
                Jump_T1.SetActive(true);

            }

            if (isPlayer2 && !SceneControl.instance.packageDialogueStart)
            {
                Jump_T2.SetActive(true);
            }
        }

        if (other.gameObject.tag == ("Package_Tutorial"))
        {
            if (isPlayer1)
            {
                throwPackag_T1.SetActive(true);
            }
            if (isPlayer2)
            {
                throwPackag_T2.SetActive(true);
            }

            if (!p1Pass1)
            {
                LevelDialogue.ShowDevilPlayerAll();
                SceneControl.instance.drAll.StartDialogue("Packages");
                GameManager.instance.p1.rC.p1Pass1 = true;
                GameManager.instance.p2.rC.p1Pass1 = true;
            }

        }

        if (other.gameObject.tag == ("Push_Tutorial"))
        {

            if (!p1Pass2)
            {
                if (isPlayer1 || isPlayer2)
                {
                    LevelDialogue.ShowDevilPlayerAll();
                    SceneControl.instance.drAll.StartDialogue("Push");
                    GameManager.instance.p1.rC.p1Pass2 = true;
                    GameManager.instance.p2.rC.p1Pass2 = true;

                }
            }

            if (isPlayer1)
            {
                push_T1.SetActive(true);
            }
            if (isPlayer2)
            {
                push_T2.SetActive(true);
            }

        }

        if (other.gameObject.tag == ("Pressure_Tutorial"))
        {
            if (!p1Pass3)
            {
                if (isPlayer1 || isPlayer2)
                {
                    p1Pass3 = true;
                    LevelDialogue.ShowDevilPlayerAll();
                    SceneControl.instance.drAll.StartDialogue("PressurePlate");
                    GameManager.instance.p1.rC.p1Pass3 = true;
                    GameManager.instance.p2.rC.p1Pass3 = true;
                }
            }
        }

        if (other.gameObject.tag == ("Gold_Tutorial"))
        {
            if (!p1Pass9)
            {
                if(isPlayer1 || isPlayer2)
                {
                    p1Pass9 = true;
                    LevelDialogue.ShowDevilPlayerAll();
                    SceneControl.instance.drAll.StartDialogue("P1GoldSummoningSquare");
                    GameManager.instance.p1.rC.p1Pass9 = true;
                    GameManager.instance.p2.rC.p1Pass9 = true;
                }
            }
        }

        if (other.gameObject.tag == ("Checkpoint_Tutorial"))
        {
            if (!p1Pass4)
            {
                if (isPlayer1 || isPlayer2)
                {
                    if (isPlayer1)
                    {
                        StartCoroutine(ActivateP1UIForDuration(3f));
                    }
                    if (isPlayer2)
                    {
                        StartCoroutine(ActivateP2UIForDuration(3f));
                    }
                    LevelDialogue.ShowDevilPlayerAll();
                    SceneControl.instance.drAll.Stop();
                    SceneControl.instance.drAll.StartDialogue("Checkpoints");
                    GameManager.instance.p1.rC.p1Pass4 = true;
                    GameManager.instance.p2.rC.p1Pass4 = true;
                }

            }
        }

        if (other.gameObject.tag == ("SummoningCircle_Tutorial"))
        {
            if (!p1Pass5)
            {
                if (isPlayer1 || isPlayer2)
                {
                    LevelDialogue.ShowDevilPlayerAll();
                    SceneControl.instance.drAll.Stop();
                    SceneControl.instance.drAll.StartDialogue("SummoningCircles");
                    GameManager.instance.p1.rC.p1Pass5 = true;
                    GameManager.instance.p2.rC.p1Pass5 = true;
                }

            }

        }

        if (other.gameObject.tag == ("Dual_Tutorial"))
        {
            if (!p1Pass6)
            {
                if (isPlayer1 || isPlayer2)
                {
                    LevelDialogue.ShowDevilPlayerAll();
                    SceneControl.instance.drAll.Stop();
                    SceneControl.instance.drAll.StartDialogue("DualSummoningCircles");
                    GameManager.instance.p1.rC.p1Pass6 = true;
                    GameManager.instance.p2.rC.p1Pass6 = true;
                }

            }
        }

        if (other.gameObject.tag == ("Specific_Tutorial"))
        {
            if (!p1Pass7)
            {
                if(isPlayer1 || isPlayer2)
                {
                    LevelDialogue.ShowDevilPlayerAll();
                    SceneControl.instance.drAll.Stop();
                    SceneControl.instance.drAll.StartDialogue("P1PlayerSpecific");
                    GameManager.instance.p1.rC.p1Pass7 = true;
                    GameManager.instance.p2.rC.p1Pass7 = true;
                }

            }


        }

        if (other.gameObject.tag == ("Sabotage_Tutorial"))
        {

            if (!p1Pass8)
            {
                if (isPlayer1 || isPlayer2)
                {
                    LevelDialogue.ShowDevilPlayerAll();
                    SceneControl.instance.drAll.Stop();
                    SceneControl.instance.drAll.StartDialogue("Sabotage");
                    GameManager.instance.p1.rC.p1Pass8 = true;
                    GameManager.instance.p2.rC.p1Pass8 = true;
                }
            }

        }
        //Debug.Log("newcheckpoint");
        if (other.gameObject.tag == ("fCheckpoint"))
        {

            GameManager.instance.p1.rC.respawnPoint = other.transform.position;
            GameManager.instance.p2.rC.respawnPoint = other.transform.position;

            if (objectGrabbable != null)
            {
                objectGrabbable.respawnPoint = respawnPoint;
            }

            if (isPlayer1)
            {
                //StartCoroutine(ActivateP1UIForDuration(3f));
                P1RespawnRotation = other.transform.Find("Rotation").transform;
                GameManager.instance.p2.rC.P2RespawnRotation = P1RespawnRotation;
            }

            if (isPlayer2)
            {
                //StartCoroutine(ActivateP2UIForDuration(3f));
                P2RespawnRotation = other.transform.Find("Rotation").transform;
                GameManager.instance.p1.rC.P1RespawnRotation = P2RespawnRotation;
            }


            foreach (CheckpointControl checkpc in cpc)
            {
                Debug.Log("deactivatetrue");
                checkpc.deActivate = true;


            }

            //Debug.Log("RespawnPoint =" + respawnPoint);

            //foreach (GameObject obj in Partner)
            //{
            //    //Debug.Log("loopworking");
            //    RespawnControl partnerScript = obj.GetComponent<RespawnControl>();

            //    if (partnerScript != null)
            //    {
            //        partnerScript.respawnPoint = respawnPoint;
            //        //Debug.Log("Partner Respawn Point" + partnerScript.respawnPoint);
            //    }
            //    //Debug.Log("Partner Respawn Point" + partnerScript.respawnPoint);
            //}

        }


    }

    private void OnTriggerStay(Collider other)
    {
        //if (other.gameObject.tag == ("PostOfficeDoor") && gameManager.player1!= null && gameManager.player2 != null)
        //{
        //    // when scene changed, both players reset start point
        //    if (testCube.ReadActionButton())
        //    {
        //        gameManager.enterOffice = true;
        //        gameManager.sceneChanged = true;

        //        //GameManager.instance.LoadScene(scene1);               

        //    }            
        //}

        if(other.tag == "DeliveryArea")
        {
            if (SceneControl.instance.showHeavyPackage)
            {
                if (isPlayer1 && Player1isCarrying)
                {
                    if (!SceneControl.instance.ConfirmTextisActivated)
                    {
                        SceneControl.instance.ShowConfirmDeliveryText();
                    }
                    SceneControl.instance.CloseDeliveryText();
                    SceneControl.instance.p1AtDoor = true;
                    if (gameManager.p1.ReadActionButton())
                    {
                        if (SceneControl.instance.firstCustomer)
                        {
                            GameManager.instance.changeSceneTimes += 1;
                            Loader.Load(Loader.Scene.Level1);
                        }

                    }

                }
                else if (isPlayer1 && !Player1isCarrying && !SceneControl.instance.deliveryTextisActivated && !SceneControl.instance.p2AtDoor)
                {
                    SceneControl.instance.ShowDeliveryText();
                    SceneControl.instance.CloseConfirmDeliveryText();
                    SceneControl.instance.p1AtDoor = true;
                }

                if (isPlayer2 && Player2isCarrying)
                {
                    if (!SceneControl.instance.ConfirmTextisActivated)
                    {
                        SceneControl.instance.ShowConfirmDeliveryText();
                    }
                    SceneControl.instance.CloseDeliveryText();
                    SceneControl.instance.p2AtDoor = true;
                    if (gameManager.p2.ReadActionButton())
                    {
                        if (SceneControl.instance.firstCustomer)
                        {
                            GameManager.instance.changeSceneTimes += 1;
                            Loader.Load(Loader.Scene.Level1);
                        }
                    }
                }
                else if (isPlayer2 && !Player2isCarrying && !SceneControl.instance.deliveryTextisActivated && !SceneControl.instance.p1AtDoor)
                {
                    SceneControl.instance.ShowDeliveryText();
                    SceneControl.instance.CloseConfirmDeliveryText();
                    SceneControl.instance.p2AtDoor = true;
                }

            }
            else if (SceneControl.instance.showPackage1)
            {
                if (isPlayer1 && Player1isCarrying)
                {
                    if (!SceneControl.instance.ConfirmTextisActivated)
                    {
                        SceneControl.instance.ShowConfirmDeliveryText();
                    }
                    SceneControl.instance.CloseDeliveryText();
                    SceneControl.instance.p1AtDoor = true;

                    if (gameManager.p1.ReadActionButton())
                    {
                        if (SceneControl.instance.secondCustomer)
                        {
                            GameManager.instance.changeSceneTimes += 1;
                            Loader.Load(Loader.Scene.MVPLevel);
                        }

                    }

                }
                else if (isPlayer1 && !Player1isCarrying && !SceneControl.instance.deliveryTextisActivated && !SceneControl.instance.p2AtDoor)
                {
                    SceneControl.instance.ShowDeliveryText();
                    SceneControl.instance.CloseConfirmDeliveryText();
                    SceneControl.instance.p1AtDoor = true;
                }

                if (isPlayer2 && Player2isCarrying)
                {
                    if (!SceneControl.instance.ConfirmTextisActivated)
                    {
                        SceneControl.instance.ShowConfirmDeliveryText();
                    }
                    SceneControl.instance.CloseDeliveryText();
                    SceneControl.instance.p2AtDoor = true;

                    if (gameManager.p2.ReadActionButton())
                    {
                        if (SceneControl.instance.secondCustomer)
                        {
                            GameManager.instance.changeSceneTimes += 1;
                            Loader.Load(Loader.Scene.MVPLevel);
                        }
                    }
                }
                else if (isPlayer2 && !Player2isCarrying && !SceneControl.instance.deliveryTextisActivated && !SceneControl.instance.p1AtDoor)
                {
                    SceneControl.instance.ShowDeliveryText();
                    SceneControl.instance.CloseConfirmDeliveryText();
                    SceneControl.instance.p2AtDoor = true;
                }

            }

        }
    
            
        //if (other.tag == "DeliveryArea")
        //{
        //    //Level 1
        //    if (gameManager.timesEnterHub == 1)
        //    {
        //        if (SceneControl.instance.showPackage || SceneControl.instance.showHeavyPackage)
        //        {
        //            if (isPlayer1 && Player1isCarrying)
        //            {
        //                if (!SceneControl.instance.ConfirmTextisActivated)
        //                {
        //                    SceneControl.instance.ShowConfirmDeliveryText();
        //                }
        //                SceneControl.instance.CloseDeliveryText();
        //                SceneControl.instance.p1AtDoor = true;
        //                if (gameManager.p1.ReadActionButton())
        //                {
        //                    if (SceneControl.instance.firstCustomer)
        //                    {
        //                        GameManager.instance.changeSceneTimes += 1;
        //                        Loader.Load(Loader.Scene.Level1);
        //                    }

        //                }

        //            }
        //            else if (isPlayer1 && !Player1isCarrying && !SceneControl.instance.deliveryTextisActivated && !SceneControl.instance.p2AtDoor)
        //            {
        //                SceneControl.instance.ShowDeliveryText();
        //                SceneControl.instance.CloseConfirmDeliveryText();
        //                SceneControl.instance.p1AtDoor = true;
        //            }

        //            if (isPlayer2 && Player2isCarrying)
        //            {
        //                if (!SceneControl.instance.ConfirmTextisActivated)
        //                {
        //                    SceneControl.instance.ShowConfirmDeliveryText();
        //                }
        //                SceneControl.instance.CloseDeliveryText();
        //                SceneControl.instance.p2AtDoor = true;
        //                if (gameManager.p2.ReadActionButton())
        //                {
        //                    if (SceneControl.instance.firstCustomer)
        //                    {
        //                        GameManager.instance.changeSceneTimes += 1;
        //                        Loader.Load(Loader.Scene.Level1);
        //                    }
        //                }
        //            }
        //            else if (isPlayer2 && !Player2isCarrying && !SceneControl.instance.deliveryTextisActivated && !SceneControl.instance.p1AtDoor)
        //            {
        //                SceneControl.instance.ShowDeliveryText();
        //                SceneControl.instance.CloseConfirmDeliveryText();
        //                SceneControl.instance.p2AtDoor = true;
        //            }

        //        }
        //    }    //MVP Level
        //    else if (gameManager.timesEnterHub == 2)
        //    {
        //        if (SceneControl.instance.showPackage1)
        //        {
        //            if (isPlayer1 && Player1isCarrying)
        //            {
        //                if (!SceneControl.instance.ConfirmTextisActivated)
        //                {
        //                    SceneControl.instance.ShowConfirmDeliveryText();
        //                }
        //                SceneControl.instance.CloseDeliveryText();
        //                SceneControl.instance.p1AtDoor = true;

        //                if (gameManager.p1.ReadActionButton())
        //                {
        //                    if (SceneControl.instance.secondCustomer)
        //                    {
        //                        GameManager.instance.changeSceneTimes += 1;
        //                        Loader.Load(Loader.Scene.MVPLevel);
        //                    }

        //                }

        //            }
        //            else if (isPlayer1 && !Player1isCarrying && !SceneControl.instance.deliveryTextisActivated && !SceneControl.instance.p2AtDoor)
        //            {
        //                SceneControl.instance.ShowDeliveryText();
        //                SceneControl.instance.CloseConfirmDeliveryText();
        //                SceneControl.instance.p1AtDoor = true;
        //            }

        //            if (isPlayer2 && Player2isCarrying)
        //            {
        //                if (!SceneControl.instance.ConfirmTextisActivated)
        //                {
        //                    SceneControl.instance.ShowConfirmDeliveryText();
        //                }
        //                SceneControl.instance.CloseDeliveryText();
        //                SceneControl.instance.p2AtDoor = true;

        //                if (gameManager.p2.ReadActionButton())
        //                {
        //                    if (SceneControl.instance.secondCustomer)
        //                    {
        //                        GameManager.instance.changeSceneTimes += 1;
        //                        Loader.Load(Loader.Scene.MVPLevel);
        //                    }
        //                }
        //            }
        //            else if (isPlayer2 && !Player2isCarrying && !SceneControl.instance.deliveryTextisActivated && !SceneControl.instance.p1AtDoor)
        //            {
        //                SceneControl.instance.ShowDeliveryText();
        //                SceneControl.instance.CloseConfirmDeliveryText();
        //                SceneControl.instance.p2AtDoor = true;
        //            }

        //        }

        //    }
        //}




        if (other.gameObject.tag == ("Start_Tutorial"))
        {


            if (isPlayer1)
            {



            }


            if (isPlayer2)
            {



            }





        }

        if (other.gameObject.tag == ("Package_Tutorial"))
        {
            if (isPlayer1)
            {



            }


            if (isPlayer2)
            {


            }


        }

        if (other.gameObject.tag == ("Jump_Tutorial"))
        {
            if (isPlayer1)
            {



            }


            if (isPlayer2)
            {



            }



        }

        if (other.gameObject.tag == ("Pressure_Tutorial"))
        {
            if (isPlayer1)
            {



            }

            if (isPlayer2)
            {


            }


        }

        if (other.gameObject.tag == ("Checkpoint_Tutorial"))
        {
            if (isPlayer1)
            {


            }

            if (isPlayer2)
            {


            }

        }

        if (other.gameObject.tag == ("SummoningCircle_Tutorial"))
        {
            if (isPlayer1)
            {


            }


            if (isPlayer2)
            {


            }




        }

        if (other.gameObject.tag == ("Cooperation_Tutorial"))
        {
            if (isPlayer1)
            {



            }

            if (isPlayer2)
            {


            }


        }


        if (other.gameObject.tag == ("End_Tutorial"))
        {

            if (isPlayer1)
            {

            }

            if (isPlayer2)
            {


            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == ("ChooseZone"))
        {
            if (isPlayer1)
            {
                SceneControl.instance.p1isInZone1 = false;
            }

            if (isPlayer2)
            {
                SceneControl.instance.p2isInZone1 = false;
            }
        }
        if (other.tag == "DeliveryArea")
        {
            if (SceneControl.instance.showHeavyPackage)
            {
                if (isPlayer1)
                {
                    SceneControl.instance.p1AtDoor = false;
                    //print("not in deliveryArea1");
                }

                if (isPlayer2)
                {
                    SceneControl.instance.p2AtDoor = false;
                    //print("not in deliveryArea2");
                }
            }

            if (SceneControl.instance.showPackage1)
            {
                if (isPlayer1)
                {
                    SceneControl.instance.p1AtDoor = false;
                    //print("not in deliveryArea1");
                }

                if (isPlayer2)
                {
                    SceneControl.instance.p2AtDoor = false;
                    //print("not in deliveryArea2");
                }
            }
            //if (gameManager.timesEnterHub == 1)
            //{
            //    if (SceneControl.instance.showPackage || SceneControl.instance.showHeavyPackage)
            //    {
            //        if (isPlayer1)
            //        {
            //            SceneControl.instance.p1AtDoor = false;
            //            print("not in deliveryArea1");
            //        }

            //        if (isPlayer2)
            //        {
            //            SceneControl.instance.p2AtDoor = false;
            //            print("not in deliveryArea2");
            //        }

            //    }
            //}
            //else if (gameManager.timesEnterHub == 2)
            //{
            //    if (SceneControl.instance.showPackage1)
            //    {
            //        if (isPlayer1)
            //        {
            //            SceneControl.instance.p1AtDoor = false;
            //            print("not in deliveryArea1");
            //        }

            //        if (isPlayer2)
            //        {
            //            SceneControl.instance.p2AtDoor = false;
            //            print("not in deliveryArea2");
            //        }

            //    }

            //}

        }

        if (other.gameObject.tag == ("Start_Tutorial"))
        {


            if (isPlayer1)
            {
                lookAround_T1.SetActive(false);
            }


            if (isPlayer2)
            {
                lookAround_T2.SetActive(false);
            }



        }

        if (other.gameObject.tag == ("Package_Tutorial"))
        {
            if (isPlayer1)
            {
                throwPackag_T1.SetActive(false);
            }


            if (isPlayer2)
            {
                throwPackag_T2.SetActive(false);
            }


        }

        if (other.gameObject.tag == ("Jump_Tutorial"))
        {
            if (isPlayer1)
            {
                Jump_T1.SetActive(false);

            }


            if (isPlayer2)
            {
                Jump_T2.SetActive(false);
            }
        }

        if (other.gameObject.tag == ("Push_Tutorial"))
        {
            if (isPlayer1)
            {
                push_T1.SetActive(false);

            }


            if (isPlayer2)
            {
                push_T2.SetActive(false);
            }
        }

        if (other.gameObject.tag == ("Pressure_Tutorial"))
        {
            if (isPlayer1)
            {
                PressurePlate_T1.SetActive(false);
            }

            if (isPlayer2)
            {
                PressurePlate_T2.SetActive(false);
            }

        }

        if (other.gameObject.tag == ("Checkpoint_Tutorial"))
        {
            if (isPlayer1)
            {
                checkPoint_T1.SetActive(false);
            }

            if (isPlayer2)
            {
                checkPoint_T2.SetActive(false);
            }

        }

        if (other.gameObject.tag == ("SummoningCircle_Tutorial"))
        {
            if (isPlayer1)
            {
                summoningCircle_T1.SetActive(false);
            }


            if (isPlayer2)
            {
                summoningCircle_T2.SetActive(false);
            }

        }

        if (other.gameObject.tag == ("Gold_Tutorial"))
        {
            if (isPlayer1)
            {

                //PressurePlate_T1.SetActive(false);

            }

            if (isPlayer2)
            {
                //PressurePlate_T1.SetActive(false);
            }

        }




        if (other.gameObject.tag == ("End_Tutorial"))
        {

            if (isPlayer1)
            {

            }

            if (isPlayer2)
            {


            }

        }


    }




    public void PlayRandomDeathDialogue1()
    {
        System.Random rnd = new System.Random();
        int index = rnd.Next(PlayerDeath.Count);
        if (previousIndex == index)
        {
            index = rnd.Next(PlayerDeath.Count);
        }
        SceneControl.instance.dRP1.StartDialogue(PlayerDeath[index]);
        previousIndex = index;
        //print("DeathBark1");
    }

    public void PlayRandomDeathDialogue2()
    {
        System.Random rnd = new System.Random();
        int index = rnd.Next(PlayerDeath.Count);
        if (previousIndex == index)
        {
            index = rnd.Next(PlayerDeath.Count);
        }
        SceneControl.instance.dRP2.StartDialogue(PlayerDeath[index]);
        //print("DeathBark2");
    }

    public void PlayRandomSabotageDialogue1()
    {
        System.Random rnd = new System.Random();
        int index = rnd.Next(SabotageChoice.Count);
        SceneControl.instance.dRP1.StartDialogue(SabotageChoice[index]);
    }
    public void PlayRandomSabotageDialogue2()
    {
        System.Random rnd = new System.Random();
        int index = rnd.Next(SabotageChoice.Count);
        SceneControl.instance.dRP2.StartDialogue(SabotageChoice[index]);
    }

    public void PlayRandomCooperationDialogue1()
    {
        System.Random rnd = new System.Random();
        int index = rnd.Next(CooperateChoice.Count);
        SceneControl.instance.dRP1.StartDialogue(CooperateChoice[index]);
    }
    public void PlayRandomCooperationDialogue2()
    {
        System.Random rnd = new System.Random();
        int index = rnd.Next(CooperateChoice.Count);
        SceneControl.instance.dRP2.StartDialogue(CooperateChoice[index]);
    }

    public void PlayRandomPackageDialogue1()
    {
        System.Random rnd = new System.Random();
        int index = rnd.Next(WithPackageReminders.Count);
        SceneControl.instance.dRP1.StartDialogue(WithPackageReminders[index]);
        //SceneControl.instance.p1BarkTriggered = true;
        //SceneControl.instance.p2BarkTriggered = true;
        SceneControl.instance.play1WithPackageDialogue = false;
        SceneControl.instance.play2WithPackageDialogue = false;
        SceneControl.instance.play1WithoutPackageDialogue = false;
        SceneControl.instance.play2WithoutPackageDialogue = false;
    }
    public void PlayRandomPackageDialogue2()
    {
        System.Random rnd = new System.Random();
        int index = rnd.Next(WithPackageReminders.Count);
        SceneControl.instance.dRP2.StartDialogue(WithPackageReminders[index]);
        //SceneControl.instance.p1BarkTriggered = true;
        //SceneControl.instance.p2BarkTriggered = true;
        SceneControl.instance.play1WithPackageDialogue = false;
        SceneControl.instance.play2WithPackageDialogue = false;
        SceneControl.instance.play1WithoutPackageDialogue = false;
        SceneControl.instance.play2WithoutPackageDialogue = false;

    }
    public void PlayRandomPackageDialogue3()
    {
        System.Random rnd = new System.Random();
        int index = rnd.Next(WithPackageReminders.Count);
        SceneControl.instance.dRP1.StartDialogue(WithoutPackageReminders[index]);
        //SceneControl.instance.p1BarkTriggered = true;
        //SceneControl.instance.p2BarkTriggered = true;
        SceneControl.instance.play1WithPackageDialogue = false;
        SceneControl.instance.play2WithPackageDialogue = false;
        SceneControl.instance.play1WithoutPackageDialogue = false;
        SceneControl.instance.play2WithoutPackageDialogue = false;

    }

    public void PlayRandomPackageDialogue4()
    {
        System.Random rnd = new System.Random();
        int index = rnd.Next(WithPackageReminders.Count);
        SceneControl.instance.dRP2.StartDialogue(WithoutPackageReminders[index]);
        //SceneControl.instance.p1BarkTriggered = true;
        //SceneControl.instance.p2BarkTriggered = true;
        SceneControl.instance.play1WithPackageDialogue = false;
        SceneControl.instance.play2WithPackageDialogue = false;
        SceneControl.instance.play1WithoutPackageDialogue = false;
        SceneControl.instance.play2WithoutPackageDialogue = false;

    }


    IEnumerator ActivateP1UIForDuration(float duration)
    {
        gameManager.p1UI.SetActive(true);

        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Deactivate the UI after the specified duration
        gameManager.p1UI.SetActive(false);
    }

    IEnumerator ActivateP2UIForDuration(float duration)
    {
        gameManager.p2UI.SetActive(true);

        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Deactivate the UI after the specified duration
        gameManager.p2UI.SetActive(false);
    }



    IEnumerator DestroyGameObject(GameObject gameObject)
    {
        Destroy(gameObject);
        return null;
    }

    boxingMinigame bM;
    GameObject minigame;

    public void startminigame()
    {
        minigame = GameObject.FindGameObjectWithTag("boxing");
        bM = minigame.GetComponent<boxingMinigame>();
        if (isPlayer1)
        {
            player.transform.position = bM.spawnpointp1;
        }
        if (isPlayer2)
        {
            player.transform.position = bM.spawnpointp2;
        }

    }

    public void startminigameHub()
    {
        minigame = GameObject.FindGameObjectWithTag("boxing");
        bM = minigame.GetComponent<boxingMinigame>();
        if (isPlayer1)
        {
            if (SelectMinigame.instance.chooseOne)
            {
                player.transform.position = bM.spawnpointp1;
            }

            if (SelectMinigame.instance.chooseTwo)
            {
                player.transform.position = bM.spawnpointp3;
            }

        }
        if (isPlayer2)
        {
            if (SelectMinigame.instance.chooseOne)
            {
                player.transform.position = bM.spawnpointp2;
            }

            if (SelectMinigame.instance.chooseTwo)
            {
                player.transform.position = bM.spawnpointp4;
            }

        }

    }

    public void endminigamep2()
    {
        
        P2Respawn();
        //if(GameManager.instance.curSceneName == "Level1" || GameManager.instance.curSceneName == "MVPLevel")
        //{
        //    if (isPlayer2)
        //    {
        //        player.transform.position = bM.spawnpointExit;
        //    }
        //}


    }
    public void endminigamep1()
    {
        P1Respawn();
        //if (GameManager.instance.curSceneName == "Level1" || GameManager.instance.curSceneName == "MVPLevel")
        //{
        //    if (isPlayer1)
        //    {
                
        //        player.transform.position = bM.spawnpointExit;
        //    }
        //}
    }
}
