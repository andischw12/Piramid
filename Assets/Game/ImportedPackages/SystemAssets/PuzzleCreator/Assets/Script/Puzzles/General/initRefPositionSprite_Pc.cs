//Description : initRefPositionSprite_Pc : Use to initialize hand sprite color on puzzle. Use for gear and logics puzzles
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class initRefPositionSprite_Pc : MonoBehaviour {
    public Color startColor;

	void Awake () {
        GetComponent<SpriteRenderer>().color = startColor;
	}
}
