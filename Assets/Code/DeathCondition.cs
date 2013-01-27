using UnityEngine;
using System.Collections;

public class DeathCondition : MonoBehaviour {

    private Heart heart;
    private Rigidbody dcRigidbody;
    private Transform transform;
    public GameObject _gui;
    private bool zoom = false;
    private bool zoomed = false;
    private float originalCameraSize;
    private float zoomCameraSize = .2f;
    private const float ZOOM_INCREMENT = .1f;
    private const string GUI_NAME = "GUI Text";

    public int defibrilatorClicks = 0;
    public bool chargeDefibrilator = false;

    public float deathTriggerVelocity;
    public float deathTriggerHeight;
    public float deathTriggerTimer;
    public const int numsplats = 3; //might make modifiable
    public int numLives = 1; // "lives"
    public Vector3[] splatvecs;

	// Use this for initialization
	void Start () {
        heart = (Heart)GetComponent<Heart>();
        dcRigidbody = (Rigidbody)GetComponent<Rigidbody>();
        transform = (Transform)GetComponent<Transform>();
        originalCameraSize = Camera.main.orthographicSize;
        _gui = GameObject.Find(GUI_NAME);
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("con1: " + heart.isThrown + ", con2: " +
        //    (dcRigidbody.velocity.magnitude < deathTriggerVelocity) + ", con3: " +
        //    (transform.position.y < deathTriggerHeight));

        if (zoom)
        {
            if (Camera.main.orthographicSize > zoomCameraSize)
            {
                Camera.main.orthographicSize -= ZOOM_INCREMENT;
            }

            if (Camera.main.orthographicSize < .3f) zoomed = true;

        }

        else
        {
            if (Camera.main.orthographicSize < originalCameraSize)
            {
                zoomed = false;
                Camera.main.orthographicSize += ZOOM_INCREMENT;
            }

        }

	    if (heart.isThrown &&
            dcRigidbody.velocity.magnitude < deathTriggerVelocity &&
            transform.position.y < deathTriggerHeight)
        {
            if (zoomed) _gui.GetComponent<MakeText>().message = "NOT TODAY!";
            StartCoroutine(WaitForDeath());
            //_gui.GetComponent<MakeText>().message = "";
        }
	}

    IEnumerator WaitForDeath()
    {
        yield return new WaitForSeconds(deathTriggerTimer);
        if (numLives > 0)
        {
            numLives--;
            
            Defibrilate();
            
        }

        

    }

    IEnumerator AccumulateClicks()
    {
        yield return new WaitForSeconds(deathTriggerTimer * 2);
        GetComponent<Heart>().Beat(defibrilatorClicks);
        _gui.GetComponent<MakeText>().message = "";
        zoom = false;
        chargeDefibrilator = false;
    }

    void Defibrilate()
    {
        Debug.Log("NOT TODAY");
        
        zoom = true;
        chargeDefibrilator = true;

        StartCoroutine(AccumulateClicks());
    }

    void BeginDeath()
    {
        CreateSplatter();
        Debug.Log("YOU'VE DIED. GOOD JOB.");
        Object.Destroy(gameObject);
    }

    void CreateSplatter()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y);
        Quaternion rot = new Quaternion();
        Rigidbody[] splats = new Rigidbody[splatvecs.Length];

        for (int i = 0; i < splats.Length; ++i)
        {
            splats[i] = (Rigidbody)Instantiate(heart.splatterPrefab, pos, rot);
            splats[i].velocity = splatvecs[i];
        }
    }
}
