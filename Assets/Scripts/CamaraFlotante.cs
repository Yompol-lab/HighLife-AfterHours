using UnityEngine;
using UnityEngine.InputSystem;

public class CamaraFlotante : MonoBehaviour
{
    [Header("Controles Móviles")]
    public Joystick joystickMovimiento; 

    [Header("Configuración")]
    public float velocidad = 5f;
    public float sensibilidadGiro = 0.5f;
    public float limiteMirarArribaAbajo = 80f;

    private float rotacionX = 0f;
    private float rotacionY = 0f;

    void Start()
    {
        rotacionX = transform.localEulerAngles.x;
        rotacionY = transform.localEulerAngles.y;
    }

    void Update()
    {
        MirarArribaAbajo();
        MoverLados();
    }

    void MirarArribaAbajo()
    {
        if (Pointer.current == null) return;

        if (Pointer.current.press.isPressed)
        {
            float deltaY = Pointer.current.delta.y.ReadValue() * sensibilidadGiro;
            rotacionX -= deltaY;
            rotacionX = Mathf.Clamp(rotacionX, -limiteMirarArribaAbajo, limiteMirarArribaAbajo);
            transform.localRotation = Quaternion.Euler(rotacionX, rotacionY, 0f);
        }
    }

    void MoverLados()
    {
        float moverX = 0f;

       
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed) moverX = -1f;
            if (Keyboard.current.dKey.isPressed) moverX = 1f;
        }

       
        if (joystickMovimiento != null && Mathf.Abs(joystickMovimiento.Horizontal) > 0.05f)
        {
            moverX = joystickMovimiento.Horizontal;
        }

        transform.Translate(Vector3.right * moverX * velocidad * Time.deltaTime);
    }
}