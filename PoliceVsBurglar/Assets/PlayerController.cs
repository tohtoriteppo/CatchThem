using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public int playerNum;
    public Transform selectionArrow;
    private int offSet = 490;
    private float leftOffset;
    private int selection;
    private bool move = false;
    private bool characterLocked = false;
    private GameController controller;
	// Use this for initialization
	void Start () {
        selection = playerNum - 1;
        leftOffset = selectionArrow.transform.localPosition.x;
        Debug.Log("leftpos " + leftOffset);
        selectionArrow.localPosition = new Vector2(leftOffset + selection * offSet, selectionArrow.localPosition.y);
        controller = Camera.main.GetComponent<GameController>();
    }
	
	// Update is called once per frame
	void Update () {
        if(!characterLocked)
        {
            float dir = Input.GetAxis("p" + playerNum.ToString() + "_joystick_horizontal");
            if (dir > 0 && !move)
            {
                move = true;
                selection = Mathf.Min(3, selection + 1);
                selectionArrow.localPosition = new Vector2(leftOffset + selection * offSet, selectionArrow.localPosition.y);
            }
            else if (dir < 0 && !move)
            {
                move = true;
                selection = Mathf.Max(0, selection - 1);
                selectionArrow.localPosition = new Vector2(leftOffset + selection * offSet, selectionArrow.localPosition.y);
            }
            else if(dir == 0)
            {
                move = false;
            }
            if (Input.GetButtonDown("p" + playerNum.ToString() + "_button_b"))
            {
                GameObject character = controller.LockCharacter(selection);
                if (character != null)
                {
                    characterLocked = true;
                    character.GetComponent<Movement>().SetPlayerNum(playerNum);
                }
            }
        }
        else if (Input.GetButtonDown("p" + playerNum.ToString() + "_button_a"))
        {
            if (controller.UnlockCharacter(selection))
            {
                characterLocked = false;
            }
        }

    }
}
