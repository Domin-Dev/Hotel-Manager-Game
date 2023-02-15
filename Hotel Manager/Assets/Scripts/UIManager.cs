using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
   [SerializeField] Transform colorPalette;


   [SerializeField] Texture2D wallTexture;
   [SerializeField] Texture2D floorTexture;


   [SerializeField] Sprite sprite;
   [SerializeField] GameObject cell;
   [SerializeField] Transform listTransform;

    


   [SerializeField] Color32 orange;

   [SerializeField] Button wallButton, floorButton, doorButton;
   [SerializeField] Dropdown dropdown;


   [SerializeField] List<BuildingElement> walls = new List<BuildingElement>();
   [SerializeField] List<BuildingElement> floors = new List<BuildingElement>();
   [SerializeField] public List<BuildingObject> doors = new List<BuildingObject>();


    public enum FloorTag
    {
        none,
        floor,
        ground,
    }
     
    public enum ObjectTag
    {
        none,
        Door,
        Desk,
    }

    Shader shader;
    private void Start()
    {
        Transform colorTransform = colorPalette.GetChild(0);
        Debug.Log(colorTransform.name);

        for (int i = 0; i < colorTransform.childCount; i++)
        {
            Color color = colorTransform.GetChild(i).GetComponent<Image>().color;
            colorTransform.GetChild(i).GetComponent<Button>().onClick.AddListener(() =>
            {                
                SetColor(color);
            });
        }   



        foreach (BuildingElement item in floors)
        {
            Rect rect = new Rect(1 + item.Id * 34, 1, 32, 32);
            item.image = Sprite.Create(floorTexture, rect, new Vector2(0.5f, 0.5f));
        }

        foreach (BuildingElement item in walls)
        {
            Rect rect = new Rect(96, item.Id * 32, 32, 32);
            item.image = Sprite.Create(wallTexture, rect, new Vector2(0.5f, 0.5f));
        }


        wallButton.onClick.AddListener(() =>
        {
            colorPalette.gameObject.SetActive(false);
            GameManager.instance.gridManager.isSelected = GridManager.IsSelected.none;
            LoadList(walls, GridManager.IsSelected.wall, FloorTag.none);
            dropdown.options = new List<Dropdown.OptionData>();
            dropdown.captionText.text = "everything";
            dropdown.interactable = false;
            ClearButtons();
            wallButton.GetComponent<Image>().color = Color.white;
            wallButton.transform.GetChild(0).GetComponent<Text>().color = Color.black;
        });

        floorButton.onClick.AddListener(() =>
        {
            colorPalette.gameObject.SetActive(false);
            GameManager.instance.gridManager.isSelected = GridManager.IsSelected.none;
            LoadList(floors, GridManager.IsSelected.floor, FloorTag.none);
            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>() { new Dropdown.OptionData("everything") };
            int count = Enum.GetValues(typeof(FloorTag)).Length;
            for (int i = 1; i < count; i++)
            {
                options.Add(new Dropdown.OptionData(Enum.GetName(typeof(FloorTag), i)));
            }
            options.Add(new Dropdown.OptionData("everything"));
            dropdown.options = options;
            dropdown.interactable = true;
            dropdown.value = 0;
            dropdown.onValueChanged.AddListener((int value) =>
            {
                if (value != dropdown.options.Count - 1) LoadList(floors, GridManager.IsSelected.floor, (FloorTag)value);
                else LoadList(floors, GridManager.IsSelected.floor, (FloorTag)0);
            });


            ClearButtons();
            floorButton.GetComponent<Image>().color = Color.white;
            floorButton.transform.GetChild(0).GetComponent<Text>().color = Color.black;
        });

        doorButton.onClick.AddListener(() =>
        {
            colorPalette.gameObject.SetActive(false);
            GameManager.instance.gridManager.isSelected = GridManager.IsSelected.none;
            LoadList(doors, GridManager.IsSelected.door, ObjectTag.none);
            dropdown.options = new List<Dropdown.OptionData>();
            dropdown.captionText.text = "everything";
            dropdown.interactable = false;

            ClearButtons();
            doorButton.GetComponent<Image>().color = Color.white;
            doorButton.transform.GetChild(0).GetComponent<Text>().color = Color.black;
        });

            

    }


    private void LoadList(List<BuildingElement> list,GridManager.IsSelected selected,FloorTag floorTag)
    {
        ClearList();

        int index = 0;
        int childCount = listTransform.childCount;

        if (selected != GridManager.IsSelected.floor)
        {
            GameObject gObject1;
            if (index >= childCount) gObject1 = Instantiate(cell, listTransform);
            else
            {
                gObject1 = listTransform.GetChild(index).gameObject;
                gObject1.SetActive(true);
                index++;
            }
            gObject1.transform.GetChild(1).GetComponent<Image>().sprite = sprite;
            gObject1.transform.GetChild(0).GetComponent<Text>().text = "demolish";
            gObject1.GetComponent<Button>().onClick.RemoveAllListeners();
            gObject1.GetComponent<Button>().onClick.AddListener(() =>
            {
                GameManager.instance.gridManager.SetStats(-1, 0);
                for (int i = 0; i < listTransform.transform.childCount; i++)
                {
                    listTransform.transform.GetChild(i).GetComponent<Image>().color = orange;
                }
                gObject1.GetComponent<Image>().color = Color.white;
                GameManager.instance.gridManager.isSelected = selected;
            });
        }

        foreach (BuildingElement obj in list)
        {
            GameObject gObject;

            if(floorTag == FloorTag.none || CheckTag(obj.Tags,floorTag))
            {
                if (index >= childCount) gObject = Instantiate(cell, listTransform);
                else
                {
                    gObject = listTransform.GetChild(index).gameObject;
                    gObject.SetActive(true);
                    index++;
                }
                gObject.transform.GetChild(1).GetComponent<Image>().sprite = obj.image;
                gObject.transform.GetChild(0).GetComponent<Text>().text = obj.name + "\n<color=#20E600> " + obj.price + "</color>";
                gObject.GetComponent<Button>().onClick.RemoveAllListeners();
                gObject.GetComponent<Button>().onClick.AddListener(() =>
                {
                    GameManager.instance.gridManager.SetStats(obj.Id, obj.price);
                    for (int i = 0; i < listTransform.transform.childCount; i++)
                    {
                        listTransform.transform.GetChild(i).GetComponent<Image>().color = orange;
                    }
                    GameManager.instance.gridManager.isSelected = selected;
                    gObject.GetComponent<Image>().color = Color.white;
                });


            }
        }
        
    }

    private void LoadList(List<BuildingObject> list, GridManager.IsSelected selected, ObjectTag objectTag)
    {
        ClearList();

        int index = 0;
        int childCount = listTransform.childCount;

            GameObject gObject1;
            if (index >= childCount) gObject1 = Instantiate(cell, listTransform);
            else
            {
                gObject1 = listTransform.GetChild(index).gameObject;
                gObject1.SetActive(true);
                index++;
            }
            gObject1.transform.GetChild(1).GetComponent<Image>().sprite = sprite;
            gObject1.transform.GetChild(0).GetComponent<Text>().text = "demolish";
            gObject1.GetComponent<Button>().onClick.RemoveAllListeners();
            gObject1.GetComponent<Button>().onClick.AddListener(() =>
            {
                colorPalette.gameObject.SetActive(false);
                GameManager.instance.gridManager.SetStats(-1, 0);
                for (int i = 0; i < listTransform.transform.childCount; i++)
                {
                    listTransform.transform.GetChild(i).GetComponent<Image>().color = orange;
                }
                gObject1.GetComponent<Image>().color = Color.white;
                GameManager.instance.gridManager.isSelected = selected;
            });


        foreach (BuildingObject obj in list)
        {
            GameObject gObject;

            if (objectTag == ObjectTag.none || CheckTag(obj.Tags, objectTag))
            {
                if (index >= childCount) gObject = Instantiate(cell, listTransform);
                else
                {
                    gObject = listTransform.GetChild(index).gameObject;
                    gObject.SetActive(true);
                    index++;
                }
                gObject.transform.GetChild(1).GetComponent<Image>().sprite = obj.images[0];
                gObject.transform.GetChild(0).GetComponent<Text>().text = obj.name + "\n<color=#20E600> " + obj.price + "</color>";
                gObject.GetComponent<Button>().onClick.RemoveAllListeners();
                gObject.GetComponent<Button>().onClick.AddListener(() =>
                {
                    colorPalette.gameObject.SetActive(true);
                    GameManager.instance.gridManager.SetStats(obj.Id, obj.price);
                    for (int i = 0; i < listTransform.transform.childCount; i++)
                    {
                        listTransform.transform.GetChild(i).GetComponent<Image>().color = orange;
                    }
                    GameManager.instance.gridManager.isSelected = selected;
                    gObject.GetComponent<Image>().color = Color.white;
                });


            }
        }
    }
    private bool CheckTag(List<FloorTag> tags, FloorTag floorTag)
    {
        foreach (FloorTag item in tags)
        {
            if (item == floorTag) return true;
        }
        return false;
    }   
    private bool CheckTag(List<ObjectTag> tags, ObjectTag objectTag)
    {
        foreach (ObjectTag item in tags)
        {
            if (item == objectTag) return true;
        }
        return false;
    }

    private void ClearList()
    {   
        for (int i = 0; i < listTransform.transform.childCount; i++)
        {
            listTransform.transform.GetChild(i).GetComponent<Image>().color = orange;
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

    public void SwitchOff()
    {
        colorPalette.gameObject.SetActive(false);
        for (int i = 0; i < listTransform.transform.childCount; i++)
        {
            listTransform.transform.GetChild(i).GetComponent<Image>().color = orange;
        }     
    }

    public void SetColor(Color color)
    {
        GameManager.instance.gridManager.SetColor(color);
    }


}
