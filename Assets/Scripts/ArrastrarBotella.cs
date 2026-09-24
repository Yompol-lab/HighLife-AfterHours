using UnityEngine;

public class ArrastrarBotella : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;
    private float alturaMesa;

    void Start()
    {
        
        alturaMesa = transform.position.y;
    }

    void OnMouseDown()
    {
        
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
       
        Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;

       
        transform.position = new Vector3(cursorPosition.x, alturaMesa, cursorPosition.z);
    }
}