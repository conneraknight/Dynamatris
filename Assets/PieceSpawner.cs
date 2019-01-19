using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour {

    public List<GameObject> pieces;

    public bool ready;
    private int numPiece;

	// Use this for initialization
	void Start () {
        numPiece = 1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        
        if (ready)
        {
            //sanity check to ensure correct number of pieces
            GameObject[] chosenObjects = GameObject.FindGameObjectsWithTag("Chosen");
            foreach(GameObject c in chosenObjects)
            {
                c.tag = "Foreground";
            }

            //gets a random piece from the possible pieces
            int num = Random.Range(0, pieces.Count);
            GameObject chosenPiece = pieces[num];
            //sets all the correct fields for that piece and enables everything
            chosenPiece.tag = "Chosen";
            chosenPiece.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            GameObject.Find("pieceController").GetComponent<PieceController>().downEnabled = true;
            GameObject.Find("pieceController").GetComponent<PieceController>().leftEnabled = true;
            GameObject.Find("pieceController").GetComponent<PieceController>().rightEnabled = true;
            //places the piece on the board
            GameObject ga = Instantiate(chosenPiece, new Vector3(-0.5f,12,0), Quaternion.identity);
            ga.transform.parent = GameObject.Find("allPieces").transform;
            ga.name = "Piece " + numPiece;
            //allPieces.Add(chosenPiece, 4);
            ready = false;
            numPiece++;
        }
        
    }
}
