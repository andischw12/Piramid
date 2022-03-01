using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FlipHebrew : MonoBehaviour
{
    
    public static string Flip(string input) 
    {
        string toReturn = "";
        int l = input.Length - 1;
        for (int i = l; i >= 0; i--)
        {
           
            toReturn+= input[i];
            



        }
        return toReturn;
    }
}
