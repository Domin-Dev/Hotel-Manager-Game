using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerGenerator : MonoBehaviour
{
    public Shader shaderBody;
    public Shader shaderHead;
    public Shader shaderHands;

    public List<Sprite> menBodyList = new List<Sprite>();
    public List<Sprite> womenBodyList = new List<Sprite>();

    public List<Sprite> menHeadList = new List<Sprite>();
    public List<Sprite> womenHeadList = new List<Sprite>();

    public List<Color> hairColors = new List<Color>();
    public List<Color> skinColors = new List<Color>();

    private bool isWoman;

    public void GetRandomLookCustomer(Transform customer)
    {
        Color hairColor = hairColors[Random.Range(0, hairColors.Count)];
        Color skinColor = skinColors[Random.Range(0, skinColors.Count)];
        

        if(Random.Range(0,2) == 0)
        {
            isWoman = true;
        }
        else
        {
            isWoman = false;
        }

        if (isWoman)
        {
            customer.GetComponent<SpriteRenderer>().sprite = womenBodyList[Random.Range(0, womenBodyList.Count)];
            customer.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = womenHeadList[Random.Range(0, womenHeadList.Count)];
        }
        else
        {
            customer.GetComponent<SpriteRenderer>().sprite = menBodyList[Random.Range(0, menBodyList.Count)];
            customer.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = menHeadList[Random.Range(0, menHeadList.Count)];
        }

        Material material = new Material(shaderBody);
        material.SetColor("color2", GetRadomColor());
        material.SetColor("color1", GetRadomColor());
        material.SetColor("color3", skinColor);
        customer.GetComponent<Renderer>().material = material;

        material = new Material(shaderHead);
        material.SetColor("color2", skinColor);
        material.SetColor("color1", hairColor);
        customer.GetChild(0).GetComponent<Renderer>().material = material;

        material = new Material(shaderHands);
        material.SetColor("color1", skinColor);

        customer.GetChild(1).GetComponent<Renderer>().material = material;
        customer.GetChild(2).GetComponent<Renderer>().material = material;
    }

    private Color GetRadomColor()
    {
        return Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.8f, 1f);
    }

}
