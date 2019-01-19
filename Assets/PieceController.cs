using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour {

    public float speed;
    public bool downEnabled;
    public bool rightEnabled;
    public bool leftEnabled;
    public bool gravOn;

    public const float GRAV = -15.0f;
    public const float DOWN_SPEED = 2.0f;
    public const float TURN_ANGLE_INTERVAL = 15.0f;

    public GameObject[] chosenPieces;

    private GameObject chosen;
    private Rigidbody2D rigid;
    private float prevRot;
    private float nextRot;
    private bool turning;

    // Use this for initialization
    void Start()
    {
        //rigid = gameObject.GetComponent<Rigidbody2D>();
        prevRot = 0.0f;
        nextRot = 0.0f;
        turning = false;
        downEnabled = true;
        leftEnabled = true;
        rightEnabled = true;
        gravOn = true;
    }

    // Update is called once per frame just used for turning the piece for now
    void Update()
    {
        if (Input.GetButtonDown("Up"))
        {
            StartCoroutine("Turn");
        }
    }

    
    //Update called at a fixed time period
    private void FixedUpdate()
    {

        //gets all the pieces that we want to have control over
        chosenPieces = GameObject.FindGameObjectsWithTag("Chosen");

        //for the present moment only take one of those pieces ******* may change later for other gameplay things
        if (chosenPieces == null || chosenPieces.Length == 0)
        {
            return;
        }

        //first we check if the chosen piece is placed

        //set the chosen piece - doent work if multiple pieces with chosen tag
        chosen = chosenPieces[0];
        //set the rigidbody
        rigid = chosen.GetComponent<Rigidbody2D>();
        RigidCheck();
        //get axises
        float vertical = Input.GetAxis("Vertical") * speed;
        float horizontal = Input.GetAxis("Horizontal") * speed;
        //case where the player is holding down
        if (vertical < 0 && downEnabled)
        {
            RigidCheck();
            rigid.velocity = new Vector2(0.0f, vertical * DOWN_SPEED);
        }
        //case where player is going left or right
        else{
            RigidCheck();
            //checks to see if the player shouldn't be able to move in that direction
            //prevents the player from sticking the piece to the walls
            if ((!leftEnabled && horizontal < 0) || (!rightEnabled && horizontal > 0))
            {
                horizontal = 0f;
            }
            rigid.velocity = new Vector2(horizontal * 0.5f, GRAV * (gravOn ? 1 : 0));
            //rigid.AddForce(new Vector2(horizontal * 2.0f, GRAV * (gravOn ? 1 : 0)));
            
        }
    }

    //checks to make sure the rigid is not null
    void RigidCheck()
    {
        if (rigid == null)
        {
            Debug.Log("Big problem");
            return;
        }
    }

    //subroutine that turns the piece
    IEnumerator Turn()
    {
        turning = true;
        prevRot = chosen.transform.rotation.eulerAngles.z;
        nextRot = 90.0f - (prevRot % 90.0f);
        while (turning) { 
            float interval = nextRot < TURN_ANGLE_INTERVAL ? nextRot : TURN_ANGLE_INTERVAL;
            nextRot = nextRot - interval;
            chosen.transform.Rotate(new Vector3(0, 0, interval));
            if (nextRot <= 0)
            {
                turning = false;
            }
            else
            {
                yield return new WaitForSeconds(0.0001f);
            }
        }
    }
}
