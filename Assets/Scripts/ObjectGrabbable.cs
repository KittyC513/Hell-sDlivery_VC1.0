using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.Rendering.ReloadAttribute;

public class ObjectGrabbable : MonoBehaviour
{
    public Rigidbody rb;
    public BoxCollider bC;
    public BoxCollider TriggerbC;
    private Transform objectGrabpo;
    [SerializeField]
    public float dropForce;
    [SerializeField]
    public float dropUpForce;
    [SerializeField]
    private float stealForce, stealUpForce;
    public GameObject player;
    public Transform playerDir;
    public GameObject player2;
    public Transform player2Dir;

    public string layerNameToFind1 = "P1Collider";
    public string layerNameToFind2 = "P2Collider";
    public string tagToFind = "PlayerDir";
    public string tagToFindPlayer = "Player";
    public Item item;

    public string layerNameToFind3 = "P1ItemContainer";
    public string layerNameToFind4 = "P2ItemContainer";



    [SerializeField]
    bool findPlayer1;
    [SerializeField]
    bool findPlayer2;
    [SerializeField]
    bool findP1Container;
    [SerializeField]
    bool findP2Container;
    [SerializeField]
    bool isDropped;
    [SerializeField]
    public GameObject p1ItemC;
    [SerializeField]
    public GameObject p2ItemC;
    [SerializeField]
    public bool P1TakePackage;
    [SerializeField]
    public bool P2TakePackage;
    [SerializeField]
    public GameObject checkPoint;
    [SerializeField]
    public Vector3 respawnPoint;
    [SerializeField]
    public GameObject p1Package;
    [SerializeField]
    public GameObject p2Package;
    [SerializeField]
    public GameObject PackageP1;
    [SerializeField]
    public GameObject PackageP2;
    [SerializeField]
    private TestCube player1TC;
    [SerializeField]
    private TestCube player2TC;
    [SerializeField]
    private float time;
    private float lastPickTime = 0;
    private bool reachedPos = false;

    [Header("Indicator")]
    [SerializeField]
    private GameObject indicator;

    [Header("Package Types")]
    [SerializeField]
    public bool isHeavy;
    [SerializeField]
    private float HeavyDropForce;
    [SerializeField]
    private float HeavyDropUpForce;
    [SerializeField]
    private float divideFactor;

    [Header("Interact Button")]
    [SerializeField]
    private Transform buttonPos;
    [SerializeField]
    private Transform buttonOriPos;

    [Header("Ground Check")]
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float groundCheckRadius = 0.33f;
    [SerializeField]
    private float groundCheckDist = 0.75f;
    [SerializeField]
    LayerMask groundLayer;

    [Header("Package in Hubstart")]
    [SerializeField]
    private BoxCollider normalPackageCollider;
    [SerializeField]
    private GameObject deliveryArea;
    [SerializeField]
    private BoxCollider heavyPackageCollider;

    [Header("Level 1")]
    public Transform world;

    [Header("Cooldown Timer")]
    private float timer;
    private bool backToLocation;
    [SerializeField]
    private Animator anim;
    private bool getRespawned;


    [SerializeField] private AK.Wwise.Event packageImpact;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        if(this.transform.localScale.x > 1 && !GameManager.instance.LalahRequestWasCompleted)
        {
            isHeavy = true;
        } 

        if(GameManager.instance.curSceneName == "HubStart")
        {
            if (isHeavy)
            {
                heavyPackageCollider.gameObject.SetActive(true);
            }
            else
            {
                normalPackageCollider.enabled = true;
            }

        }

        buttonOriPos = buttonPos;

    }

    private void Start()
    {
        respawnPoint = this.transform.position;
        if (GameManager.instance.curSceneName == "HubStart")
        {
            deliveryArea.SetActive(false);
            print("deliveryAreafalse");
        }

    }


    // Start is called before the first frame update

    //when a pickable item is picked, it transform to player's head
    public void Grab(Transform objectGrabpo)
    {
        if(normalPackageCollider != null)
        {
            normalPackageCollider.enabled = false;
        }
        if(heavyPackageCollider != null)
        {
            heavyPackageCollider.gameObject.SetActive(false);
        }

        this.objectGrabpo = objectGrabpo;
        //set the gravity to zero when it's picked
        rb.useGravity = false;
        rb.isKinematic = true;
        bC.enabled = false;

        if(buttonPos != null)
        {
            buttonPos.parent = null;
        }

        if (isHeavy)
        {
            TriggerbC.enabled = false;
        }

       
        lastPickTime = Time.time;
        //InventoryManager.Instance.Add(item);

    }



    private void Update()
    {
        FindGameObject();
        FindItemContainer();
        IndicatorControl();

        Move();
        P1Steal();
        P2Steal();
        ShowDeliveryArea();
        PackageCooldown();
        //CheckGrounded();
        DetectPackage();
    }

    private void FixedUpdate()
    {
        if(GameManager.instance.curSceneName == "Level1" || GameManager.instance.curSceneName == "MVPLevel")
        {
            AddScore();
        }

        //buttonPos = buttonOriPos;


        //PackageIcon();
    }

    void PackageIcon()
    {
        if (P1TakePackage)
        {
            PackageP1.SetActive(true);
        }
        else
        {
            PackageP1.SetActive(false);
        }

        if (P2TakePackage)
        {
            PackageP2.SetActive(true);
        }
        else
        {
            PackageP2.SetActive(false);
        }
    }

    void DetectPackage()
    {
        if (!GameManager.instance.LalahRequestWasCompleted)
        {
            if (P1TakePackage)
            {
                GameManager.instance.p1.objectGrabbable = this;
                GameManager.instance.p2.objectGrabbable = null;
                if (isHeavy)
                {
                    if (GameManager.instance.curSceneName == "Level1" || GameManager.instance.curSceneName == "HubStart")
                    {
                        if (boxingMinigame.instance.isboxing)
                        {
                            GameManager.instance.p1.tooHeavy = false;
                        }
                        else
                        {
                            GameManager.instance.p1.tooHeavy = true;
                        }
                    }
                    else
                    {
                        GameManager.instance.p1.tooHeavy = true;
                    }

                }
                else
                {
                    GameManager.instance.p1.tooHeavy = false;
                }
            }
            else
            {
                GameManager.instance.p1.tooHeavy = false;
            }

            if (P2TakePackage)
            {
                GameManager.instance.p2.objectGrabbable = this;
                GameManager.instance.p1.objectGrabbable = null;
                if (isHeavy)
                {
                    if (GameManager.instance.curSceneName == "Level1" || GameManager.instance.curSceneName == "HubStart")
                    {
                        if (boxingMinigame.instance.isboxing)
                        {
                            GameManager.instance.p2.tooHeavy = false;
                        }
                        else
                        {
                            GameManager.instance.p2.tooHeavy = true;
                        }
                    }
                    else
                    {
                        GameManager.instance.p2.tooHeavy = true;
                    }

                }
                else
                {
                    GameManager.instance.p2.tooHeavy = false;
                }
            }
            else
            {
                GameManager.instance.p2.tooHeavy = false;
            }
        }
        else
        {
            GameManager.instance.p2.tooHeavy = false;
            GameManager.instance.p1.tooHeavy = false;
        }
        

    }

    private void Move()
    {
        float lerpSpeed = 0;

        if (objectGrabpo != null)
        {
            if (isHeavy && !reachedPos)
            {
                lerpSpeed = 8;
            }
            else if (isHeavy && reachedPos)
            {
                lerpSpeed = 22;
            }
            else
            {
                lerpSpeed = 30;
            }

            float percent = (lastPickTime - Time.time) * lerpSpeed;

            //smooth moving
            Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabpo.position, Time.deltaTime * lerpSpeed);
            rb.MovePosition(newPosition);
           
            if (Mathf.Abs(transform.position.y - objectGrabpo.position.y) < 0.2f)
            {
                reachedPos = true;
            }
            
        

            if (objectGrabpo.position == p1ItemC.transform.position)
            {
                P1TakePackage = true;
                P2TakePackage = false;
                GameManager.instance.p2.objectGrabbable = null;


            }
            else if (objectGrabpo.position == p2ItemC.transform.position)
            {
                P1TakePackage = false;
                P2TakePackage = true;
                GameManager.instance.p1.objectGrabbable = null;
            }
        }
    }

    void AddScore()
    {
        if(!P1TakePackage && !P2TakePackage)
        {
            time = 0;
        }
        if (P1TakePackage)
        {
            StartCoroutine(P1Timer());

        }
        else if (P2TakePackage)
        {
            StartCoroutine(P2Timer());
        }

    }

    IEnumerator P1Timer()
    {
        time += Time.deltaTime;

        ScoreCount.instance.AddTimeToP1Package(time);

        ScoreCount.instance.AddPointToP1Package(3);
        //StartCoroutine(ActivateP1UIForDuration(3));

        yield return null;
    }

    IEnumerator P2Timer()
    {
      
        time += Time.deltaTime;
        ScoreCount.instance.AddTimeToP2Package(time);

        ScoreCount.instance.AddPointToP2Package(3);
        //StartCoroutine(ActivateP2UIForDuration(3));

        yield return null;
    }

    void IndicatorControl()
    {
        if (!P1TakePackage && !P2TakePackage)
        {
            indicator.SetActive(true);
        }
        else if(P1TakePackage)
        {
            indicator.SetActive(false);
        } 
        else if (P2TakePackage)
        {
            indicator.SetActive(false);
        }
    }

    /*
    player2 = GameObject.FindGameObjectWithTag("Player2");
    player2Dir = GameObject.FindGameObjectWithTag("Player2Dir").transform;
    */

    void P1Steal()
    {
        if (player2TC.p2Steal)
        {
            this.objectGrabpo = null;
            rb.useGravity = true;
            rb.isKinematic = false;
            bC.enabled = true;

            rb.velocity = player.GetComponent<Rigidbody>().velocity;

            rb.AddForce(playerDir.forward * stealForce, ForceMode.Impulse);
            rb.AddForce(playerDir.up * stealUpForce, ForceMode.Impulse);

            player2TC.p2Steal = false;

            float random = Random.Range(-1, 1);
            rb.AddTorque(new Vector3(random, random, random));

            P1TakePackage = false;
            P2TakePackage = true;

            if (isHeavy)
            {
                TriggerbC.enabled = false;
            }
            time = 0;
            //GameManager.instance.p2.objectGrabbable = this;
            //GameManager.instance.p1.objectGrabbable = null;
        }
    }

    void P2Steal()
    {
        if (player1TC.p1Steal)
        {
            this.objectGrabpo = null;
            rb.useGravity = true;
            rb.isKinematic = false;
            bC.enabled = true;

            rb.velocity = player2.GetComponent<Rigidbody>().velocity;

            rb.AddForce(player2Dir.forward * stealForce, ForceMode.Impulse);
            rb.AddForce(player2Dir.up * stealUpForce, ForceMode.Impulse);

            player1TC.p1Steal = false;

            float random = Random.Range(-1, 1);
            rb.AddTorque(new Vector3(random, random, random));

            P1TakePackage = true;
            P2TakePackage = false;
            ScoreCount.instance.time = 0;


            if (isHeavy)
            {
                TriggerbC.enabled = false;
            }
            time = 0;

            //GameManager.instance.p2.objectGrabbable = null;
            //GameManager.instance.p1.objectGrabbable = this;
        }
    }


    public void P1Drop()
    {

        this.objectGrabpo = null;
        rb.useGravity = true;
        rb.isKinematic = false;
        bC.enabled = true;

        rb.velocity = player.GetComponent<Rigidbody>().velocity / divideFactor;

        if (isHeavy)
        {

            rb.AddForce(playerDir.forward * HeavyDropForce, ForceMode.Impulse);
            rb.AddForce(playerDir.up * HeavyDropUpForce, ForceMode.Impulse);

            //print("DropForce" + HeavyDropForce);
            //print("DropUpForce" + HeavyDropUpForce);

        }
        else
        {


            rb.AddForce(playerDir.forward * dropForce, ForceMode.Impulse);

            rb.AddForce(playerDir.up * dropUpForce, ForceMode.Impulse);

            //print("DropForce" + dropForce);
            //print("DropUpForce" + dropUpForce);
        }

        if (isHeavy)
        {
            TriggerbC.enabled = true;
        }
        else
        {
            if(normalPackageCollider != null)
            normalPackageCollider.enabled = true;
        }




        float random = Random.Range(-1, 1);
        rb.AddTorque(new Vector3(random, random, random));

        P1TakePackage = false;
        P2TakePackage = false;

        //ScoreCount.instance.time = 0;
        //InventoryManager.Instance.Remove(item);
        time = 0;

        reachedPos = false;
        GameManager.instance.p1.objectGrabbable = null;
        GameManager.instance.p2.objectGrabbable = null;
    }

    public void P2Drop()
    {
        this.objectGrabpo = null;
        rb.useGravity = true;
        rb.isKinematic = false;
        bC.enabled = true;

        rb.velocity = player2.GetComponent<Rigidbody>().velocity / divideFactor;

        if (isHeavy)
        {

            rb.AddForce(player2Dir.forward * HeavyDropForce, ForceMode.Impulse);

            rb.AddForce(player2Dir.up * HeavyDropUpForce, ForceMode.Impulse);

           // print("DropForce" + HeavyDropForce);
            //print("DropUpForce" + HeavyDropUpForce);

        }
        else
        {
            rb.AddForce(player2Dir.forward * dropForce, ForceMode.Impulse);

            rb.AddForce(player2Dir.up * dropUpForce, ForceMode.Impulse);

            //print("DropForce" + dropForce);
            //print("DropUpForce" + dropUpForce);
        }

        if (isHeavy)
        {
            TriggerbC.enabled = true;
        }
        else
        {
            if (normalPackageCollider != null)
                normalPackageCollider.enabled = true;
        }



        float random = Random.Range(-1, 1);
        rb.AddTorque(new Vector3(random, random, random));


        P1TakePackage = false;
        P2TakePackage = false;

        //InventoryManager.Instance.Remove(item);
        time = 0;

        reachedPos = false;
        GameManager.instance.p1.objectGrabbable = null;
        GameManager.instance.p2.objectGrabbable = null;
    }

    void FindGameObject()
    {
        int layerToFind1 = LayerMask.NameToLayer(layerNameToFind1);
        int layerToFind2 = LayerMask.NameToLayer(layerNameToFind2);
        GameObject[] objectsInLayer = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in objectsInLayer)
        {
            if (obj.layer == layerToFind1 && !findPlayer1)
            {
                player = obj;
                player1TC = player.GetComponent<TestCube>();

                Transform parentTransform = player.transform;

                foreach (Transform child in parentTransform)
                {
                    if (child.CompareTag(tagToFind))
                    {
                        playerDir = child;
                      
                        findPlayer1 = true;
                    }
                }

                //rb.velocity = player.GetComponent<Rigidbody>().velocity;
                //rb.AddForce(playerDir.forward * dropForce, ForceMode.Impulse);
                //rb.AddForce(playerDir.up * dropUpForce, ForceMode.Impulse);

            }

            if (obj.layer == layerToFind2 && !findPlayer2)
            {
                player2 = obj;

                player2TC = player2.GetComponent<TestCube>();


                Transform parentTransform = player2.transform;

                foreach (Transform child in parentTransform)
                {
                    if (child.CompareTag(tagToFind))
                    {
                        player2Dir = child;
                        
                        findPlayer2 = true;
                    }

                }
                //rb.velocity = player2.GetComponent<Rigidbody>().velocity;
                //rb.AddForce(player2Dir.forward * dropForce, ForceMode.Impulse);
                //rb.AddForce(player2Dir.up * dropUpForce, ForceMode.Impulse);

            }
        }

    }


    void FindItemContainer()
    {
        int layerToFind1 = LayerMask.NameToLayer(layerNameToFind3);
        int layerToFind2 = LayerMask.NameToLayer(layerNameToFind4);
        GameObject[] objectsInLayer = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in objectsInLayer)
        {
            if (obj.layer == layerToFind1 && !findP1Container)
            {
                p1ItemC = obj;
                findP1Container = true;
                //Debug.Log("Found GameObject on layer: " + obj.name);
            }

            if (obj.layer == layerToFind2 && !findP2Container)
            {
                p2ItemC = obj;
                findP2Container = true;
                //Debug.Log("Found GameObject on layer: " + obj.name);
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hazard"))
        {
            if (!P1TakePackage && !P1TakePackage)
            {
                this.transform.position = respawnPoint;

            }
        }
        else if (other.tag == "CheckPoint")
        {
            respawnPoint = other.transform.position;
        }

        if (other.transform.tag == "MovingPlat" && !P1TakePackage && !P2TakePackage)
        {

            

            platformTransform = other.transform;

            boxTransform.parent = platformTransform;
        }

    }

    IEnumerator ActivateP1UIForDuration(float duration)
    {
        GameManager.instance.p1UI.SetActive(true);

        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Deactivate the UI after the specified duration
        GameManager.instance.p1UI.SetActive(false);
    }

    IEnumerator ActivateP2UIForDuration(float duration)
    {
        GameManager.instance.p2UI.SetActive(true);

        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Deactivate the UI after the specified duration
        GameManager.instance.p2UI.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            packageImpact.Post(this.gameObject);
        }
    }

    public Transform platformTransform;
    public Transform boxTransform;

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "MovingPlat")
        {
            if(GameManager.instance.curSceneName == "Level1")
            {
                boxTransform.parent = world;
            }


        }
    }
    //private bool CheckGrounded()
    //{

    //    RaycastHit hit;
    //    if (Physics.SphereCast(groundCheck.position, groundCheckRadius, Vector3.down, out hit, groundCheckDist, groundLayer)) return true;
    //    else return false;
    //}


    private void ShowDeliveryArea()
    {
        if(GameManager.instance.curSceneName == GameManager.instance.scene1)
        {
            if(P1TakePackage || P2TakePackage)
            {
                deliveryArea.SetActive(true);
                //print("deliveryAreaTrue");
            }
        }
    }

    #region Package Cooldown Timer
    void PackageCooldown()
    {
        if (GameManager.instance.curSceneName == "Level1" || GameManager.instance.curSceneName == "MVPLevel")
        {
            if (P1TakePackage == false && P2TakePackage == false && Level1CamControl.instance.cutsceneIsCompleted)
            {
                if (timer < 30 && !boxingMinigame.instance.isboxing)
                {
                    timer += Time.deltaTime;

                    if(timer >= 20 && timer < 30)
                    {
                        anim.SetBool("DisppearWarning", true);
                        TargetIndicator.instance.anim.SetBool("DisppearWarning", true);
                        PlayerIndicator.instance.anim.SetBool("DisppearWarning", true);
                    }
                }
                else if(timer >= 30 && !boxingMinigame.instance.isboxing)
                {
                    if (!backToLocation)
                    {                
                        this.transform.position = respawnPoint;
                        backToLocation = true;
                        getRespawned = true;
                    
                    }

                }
            
            }
            else if(P1TakePackage || P2TakePackage)
            {
                backToLocation = false;
                timer = 0;

            }
        }

        if(GameManager.instance.curSceneName == "Tutorial")
        {
            if (P1TakePackage == false && P2TakePackage == false && SceneControl.instance.canRespawn)
            {
                if (timer < 30)
                {
                    timer += Time.deltaTime;

                    if (timer >= 20 && timer < 30)
                    {
                        anim.SetBool("DisppearWarning", true);
                        TargetIndicator.instance.anim.SetBool("DisppearWarning", true);
                        PlayerIndicator.instance.anim.SetBool("DisppearWarning", true);
                    }
                }
                else if (timer >= 30)
                {
                    if (!backToLocation)
                    {
                        this.transform.position = respawnPoint;
                        P1TakePackage = false;
                        P2TakePackage = false;
                        backToLocation = true;
                        getRespawned = true;

                    }

                }

            }
            else if (P1TakePackage || P2TakePackage)
            {
                backToLocation = false;
                timer = 0;

            }
        }

        if (getRespawned)
        {
            StartCoroutine(ShowRespawnWarning());
            
        }
    }

    IEnumerator ShowRespawnWarning()
    {
        anim.SetBool("DisppearWarning", false);
        TargetIndicator.instance.anim.SetBool("DisppearWarning", false);
        PlayerIndicator.instance.anim.SetBool("DisppearWarning", false);
        ScoreCount.instance.notificationText.SetActive(true);
       
        anim.SetBool("RespawnWarning", true);
        TargetIndicator.instance.anim.SetBool("RespawnWarning", true);
        PlayerIndicator.instance.anim.SetBool("RespawnWarning", true);
        
        yield return new WaitForSeconds(3f);
        
        anim.SetBool("RespawnWarning", false);
        TargetIndicator.instance.anim.SetBool("RespawnWarning", false);
        PlayerIndicator.instance.anim.SetBool("RespawnWarning", false);

        ScoreCount.instance.notificationText.SetActive(false);
        getRespawned = false;
        print("ShowRespawnWarning");
    }

    #endregion

}
