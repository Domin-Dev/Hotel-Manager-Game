using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    
    [SerializeField] public Sprite interactionSprite;
    [SerializeField] public Material spriteMaterial;
    [SerializeField] Material material;
    [SerializeField] Material material1;
    [SerializeField] Material material2;
    [SerializeField] public Text text;

    public GridManager gridManager;
    public Grid gameGrid { get; private set; }
    public static GameManager instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this);
            instance = this;
            gameGrid = new Grid(100, 120, 3f, Vector3.zero - new Vector3(30,0,0), material,material1,material2);
            gameObject.AddComponent<GridManager>();
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        gridManager = this.GetComponent<GridManager>();
    }
}
