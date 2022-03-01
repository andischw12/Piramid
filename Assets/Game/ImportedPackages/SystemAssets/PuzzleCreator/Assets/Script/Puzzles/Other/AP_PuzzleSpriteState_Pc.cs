//Description: AP_PuzzleSpriteState_Pc: Display the sprite Access Allow, denied or solved on module Grp_PuzzleState. The script is on object Sprite_PuzzleState
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AP_PuzzleSpriteState_Pc : MonoBehaviour {

    public List<Sprite> listOfSprites = new List<Sprite>();
    private SpriteRenderer sRenderer;

	// Use this for initialization
	void Start () {
        sRenderer = GetComponent<SpriteRenderer>();
	}

	public void AP_ChangeSprite (int spriteNumber) {
        sRenderer = GetComponent<SpriteRenderer>();
        if(sRenderer.sprite != listOfSprites[2])
            sRenderer.sprite = listOfSprites[spriteNumber];
	}
}
