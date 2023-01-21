using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovement : MonoBehaviour
{
    Camera camera;
    [SerializeField] float speedCamera;
    Vector2 min;
    Vector2 max;
    int edgeBorder = 5;
    float h, v, x, y;
    bool isDown = false;

    Vector2 startPosition;
    Vector2 currentPosition;
    

    private void Start()
    {
        camera = GetComponent<Camera>();
        min = GameManager.instance.gameGrid.gridPosition;
        max = GameManager.instance.gameGrid.GetLimit();
    }

    private void Update()
    {

        x = 0;
        y = 0;    
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        if(Input.mouseScrollDelta.y > 0 && !mouseIsOverUI())
        {
            camera.orthographicSize = Mathf.Clamp(camera.orthographicSize - 160 * Time.deltaTime, 20, 120);
        }else if(Input.mouseScrollDelta.y < 0 && !mouseIsOverUI())
        {
            camera.orthographicSize = Mathf.Clamp(camera.orthographicSize + 160 * Time.deltaTime, 20, 120);
        }

        if (Input.mousePosition.y > Screen.height - edgeBorder)
        {
            y =  speedCamera * Time.deltaTime * 10;
        } 
        if (Input.mousePosition.y <  edgeBorder)
        {
             y = -speedCamera* Time.deltaTime * 10;
        }
        if (Input.mousePosition.x > Screen.width - edgeBorder)
        {
              x =  speedCamera* Time.deltaTime * 10;
        }
        if (Input.mousePosition.x < edgeBorder)
        {
              x = -speedCamera * Time.deltaTime * 10;
        }

        if (h != 0 || v != 0)
        {
            x = h * speedCamera * Time.deltaTime * 13;
            y = v * speedCamera * Time.deltaTime * 13;
        }

        if(Input.GetMouseButtonDown(2))
        {
            startPosition = GetMousePosition();
        } 
        
        if(Input.GetMouseButton(2))
        {
            transform.position = (Vector2)transform.position + (startPosition - (Vector2)GetMousePosition());
        }







        x = Mathf.Clamp(x  + transform.position.x, min.x, max.x);
        y = Mathf.Clamp(y + transform.position.y, min.y, max.y);
        transform.position = new Vector3(x, y, -60f);
    }

    private Vector3 GetMousePosition()
    {
        Vector3 vector3 = Input.mousePosition;
        return camera.ScreenToWorldPoint(vector3);
    }
        
    static public bool mouseIsOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

}
