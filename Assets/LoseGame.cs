using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseGame : MonoBehaviour {

    public Text loseText;
    public GameObject allPieces;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay2D(Collider2D collision) {

        if (collision.gameObject.transform.parent != null && collision.gameObject.transform.parent.tag.Equals("Foreground"))
        {
            //end game
            loseText.text = "GAME OVER";
            foreach(Transform t in allPieces.transform){
                Destroy(t.gameObject);
            }
            GameObject.Find("pieceSpawner").GetComponent<PieceSpawner>().ready = false;

        }

    }
}
