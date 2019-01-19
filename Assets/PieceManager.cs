using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour {

    private bool waiting;

    public int numChildren;
    public bool broken;
	// Use this for initialization
	void Start () {
        waiting = false;
        broken = false;
        numChildren = gameObject.transform.childCount;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.tag.Equals("Chosen"))
        {
            if (collision.gameObject.tag.Equals("Foreground") && !waiting)
            {
                //gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                if (!waiting) { 
                    waiting = true;
                    GameObject.Find("pieceController").GetComponent<PieceController>().gravOn = false;
                    GameObject.Find("pieceController").GetComponent<PieceController>().downEnabled = false;
                    StartCoroutine(Wait());
                    
                }
                
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (gameObject.tag.Equals("Chosen") && collision.gameObject.tag.Equals("Wall"))
        {
            if (collision.gameObject.name.Equals("lwall"))
            {
                GameObject.Find("pieceController").GetComponent<PieceController>().leftEnabled = false;
            }
            else if (collision.gameObject.name.Equals("rwall"))
            {
                GameObject.Find("pieceController").GetComponent<PieceController>().rightEnabled = false;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (gameObject.tag.Equals("Chosen") && collision.gameObject.tag.Equals("Wall"))
        {
            if (collision.gameObject.name.Equals("lwall"))
            {
                GameObject.Find("pieceController").GetComponent<PieceController>().leftEnabled = true;
            }
            else if (collision.gameObject.name.Equals("rwall"))
            {
                GameObject.Find("pieceController").GetComponent<PieceController>().rightEnabled = true;
            }
        }
    }

    //Coroutine that activates once the piece hits the foreground
    IEnumerator Wait()
    {
        //how long we wait for
        yield return new WaitForSeconds(0.25f);
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        //sets that piece to Foreground instead of Chosen
        gameObject.tag = "Foreground";
        gameObject.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Discrete;

        //set various flags on other scripts
        GameObject.Find("pieceController").GetComponent<PieceController>().gravOn = true;
        GameObject.Find("pieceSpawner").GetComponent<PieceSpawner>().ready = true;
        GameObject.Find("pieceDestroyer").GetComponent<PieceModel>().checkLines = true;
        waiting = false;

    } 
}
