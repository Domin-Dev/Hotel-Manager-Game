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
            GameObject gObject = Instantiate(cell, wallsTransform);
            gObject.transform.GetChild(1).GetComponent<Image>().sprite = obj.Image;
            gObject.transform.GetChild(0).GetComponent<Text>().text = obj.name + "\n<color=#20E600> " +obj.price+"</color>";
            gObject.GetComponent<Button>().onClick.AddListener(() => { 
                GameManager.instance.gridManager.id = obj.wallId;
                for (int i = 0; i < wallsTransform.transform.childCount; i++)
                {
                    wallsTransform.transform.GetChild(i).GetComponent<Image>().color = new Color32(230, 125, 0,255);
                    Debug.Log("s");
                }
                gObject.GetComponent<Image>().color = Color.white;             
            });
            
            
        }     
    }

}
