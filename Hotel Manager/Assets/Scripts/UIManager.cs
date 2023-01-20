using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
   [SerializeField] GameObject cell;
   [SerializeField] Transform wallsTransform;

   [SerializeField] List<Wall> walls = new List<Wall>();


    private void Start()
    {
        foreach(Wall obj in walls)
        {
            Instantiate(cell, wallsTransform).transform.GetChild(0).GetComponent<Image>().sprite = obj.Image;            
        }     
    }

}
