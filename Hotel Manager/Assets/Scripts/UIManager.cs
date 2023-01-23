using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
   [SerializeField] Sprite sprite;
   [SerializeField] GameObject cell;
   [SerializeField] Transform listTransform;

    Color32 orange = new Color32(230, 125, 0, 255);

   [SerializeField] Button wallButton, floorButton, doorButton;


   [SerializeField] List<BuildingElement> walls = new List<BuildingElement>();
   [SerializeField] List<BuildingElement> floors = new List<BuildingElement>();


    private void Start()
    {
        wallButton.onClick.AddListener(() => 
        { 
            LoadList(walls,GridManager.IsSelected.wall); 
            ClearButtons();
            wallButton.GetComponent<Image>().color = Color.white; 
            wallButton.transform.GetChild(0).GetComponent<Text>().color = Color.black; 
        });
        floorButton.onClick.AddListener(() => 
        { 
            LoadList(floors,GridManager.IsSelected.floor); 
            ClearButtons(); 
            floorButton.GetComponent<Image>().color = Color.white; 
            floorButton.transform.GetChild(0).GetComponent<Text>().color = Color.black; 
        });

    }

    private void Update()
    {      
        
        if(Input.GetKeyDown(KeyCode.O))
        {
            ClearList();
        }
    }

    private void LoadList(List<BuildingElement> list,GridManager.IsSelected selected)
    {
        ClearList();
        GameManager.instance.gridManager.isSelected = selected;
        GameObject gObject1 = Instantiate(cell, listTransform);
        gObject1.transform.GetChild(1).GetComponent<Image>().sprite = sprite;
        gObject1.transform.GetChild(0).GetComponent<Text>().text = "demolish";
        gObject1.GetComponent<Button>().onClick.AddListener(() => {
            GameManager.instance.gridManager.SetWall(-1, 0);
            for (int i = 0; i < listTransform.transform.childCount; i++)
            {
                listTransform.transform.GetChild(i).GetComponent<Image>().color = orange;
            }
            gObject1.GetComponent<Image>().color = Color.white;
        });


        foreach (BuildingElement obj in list)
        {
            GameObject gObject = Instantiate(cell, listTransform);
            gObject.transform.GetChild(1).GetComponent<Image>().sprite = obj.image;
            gObject.transform.GetChild(0).GetComponent<Text>().text = obj.name + "\n<color=#20E600> " + obj.price + "</color>";
            gObject.GetComponent<Button>().onClick.AddListener(() => {
                GameManager.instance.gridManager.SetWall(obj.Id, obj.price);
                for (int i = 0; i < listTransform.transform.childCount; i++)
                {
                    listTransform.transform.GetChild(i).GetComponent<Image>().color = orange;
                }
                gObject.GetComponent<Image>().color = Color.white;
            });


        }
    }

    private void ClearList()
    {   
        for (int i = 0; i < listTransform.transform.childCount; i++)
        {
            listTransform.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void ClearButtons()
    {
        wallButton.GetComponent<Image>().color = orange;
        wallButton.transform.GetChild(0).GetComponent<Text>().color = Color.white;
        floorButton.GetComponent<Image>().color = orange;
        floorButton.transform.GetChild(0).GetComponent<Text>().color = Color.white;
        doorButton.GetComponent<Image>().color = orange;
        doorButton.transform.GetChild(0).GetComponent<Text>().color = Color.white;
    }

}
