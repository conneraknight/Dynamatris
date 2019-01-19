using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceModel : MonoBehaviour {

    public bool checkLines;
    public int score;
    public float multiplier;
    public Text scoreText;

	// Use this for initialization
	void Start () {
        score = 0;
        multiplier = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        //only goes through this fairly intense computation when a chosen piece has settled
        if (checkLines)
        {
            bool line = true;
            List<GameObject> trash = new List<GameObject>();
            //go through each rows on the board and check if there is a line of blocks at each tile
            for (int row = -10; row < 10; row++)
            {
                for (int col = -5; col < 5; col++)
                {
                    //use a circle on each tile to tell if a block is there
                    Collider2D[] contained = Physics2D.OverlapCircleAll(new Vector2(col + 0.5f, row + 0.5f), 0.4f);
                    //if there is any break then line is false and do not delete that row
                    if (contained.Length > 0)
                    {
                        bool l = false;
                        for(int i = 0; i < contained.Length; i++)
                        {
                            if (!contained[i].gameObject.tag.Equals("Wall") && !contained[i].transform.name.StartsWith("Piece"))
                            {
                                l = true;
                                trash.Add(contained[i].gameObject);

                            }
                        }
                        line = line && l;
                    }
                    else
                    {
                        line = false;
                    }
                }
                //case where a line of blocks needs to be deleted
                if (line)
                {
                    foreach(GameObject g in trash)
                    {
                        //adding score for the blocks destroyed
                        score += (int)multiplier;
                        multiplier += 0.01f;
                        //get the parent which should be a Piece
                        GameObject parent = g.transform.parent.gameObject;
                        if (parent != null && parent.transform.name.StartsWith("Piece"))
                        {
                            parent.GetComponent<PieceManager>().numChildren = parent.GetComponent<PieceManager>().numChildren - 1;
                            if (!parent.GetComponent<PieceManager>().broken)
                            {
                                //if a piece that is to be partily destroyed is not already broken then break it
                                foreach (Transform child in parent.transform)
                                {
                                    child.gameObject.tag = "Foreground";
                                    child.gameObject.AddComponent<Rigidbody2D>();
                                    //child.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                                    //child.GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.Extrapolate;
                                }
                                parent.GetComponent<PieceManager>().broken = true;
                            }
                            //destroy the block to be destroyed
                            Destroy(g);
                            //if the parent has no more blocks then destroy it as well
                            if (parent.GetComponent<PieceManager>().numChildren <= 0)
                            {

                                Destroy(parent);
                            }
                        }
                    }
                    //update score
                    if(scoreText != null)
                    {
                        scoreText.text = "Score: " + score;
                    }

                }
                trash = new List<GameObject>();
                line = true;

            }
            //reset multiplier and stop checking lines
            multiplier = 1.0f;
            checkLines = false;
        }


    }
}
