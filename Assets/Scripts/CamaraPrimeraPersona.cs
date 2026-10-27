using UnityEngine;
using UnityEngine.InputSystem;

public class CamaraPrimeraPersona : MonoBehaviour
{
    [Header("Joysticks en Pantalla")]
    public Joystick joystickMovimiento;
    public Joystick joystickVista;      

    [Header("Configuración de Movimiento")]
    public float velocidadCaminar = 4f;
    public float sensibilidadGiro = 2f;

    private float rotacionX = 0f;
    private float rotacionY = 0f;

    void OnEnable()
    {
        rotacionX = transform.localEulerAngles.x;
        rotacionY = transform.localEulerAngles.y;
    }

    void Update()
    {
        MirarConJoystick();
        Caminar();
    }

    void MirarConJoystick()
    {
        float deltaX = 0f;
        float deltaY = 0f;

        
        if (joystickVista != null)
        {
            deltaX = joystickVista.Horizontal * sensibilidadGiro;
            deltaY = joystickVista.Vertical * sensibilidadGiro;
        }

        rotacionY += deltaX;
        rotacionX -= deltaY;

        rotacionX = Mathf.Clamp(rotacionX, -85f, 85f);
        transform.localRotation = Quaternion.Euler(rotacionX, rotacionY, 0f);
    }

    void Caminar()
    {
        float moverX = 0f;
        float moverZ = 0f;

        
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) moverZ = 1f;
            if (Keyboard.current.sKey.isPressed) moverZ = -1f;
            if (Keyboard.current.aKey.isPressed) moverX = -1f;
            if (Keyboard.current.dKey.isPressed) moverX = 1f;
        }

        
        if (joystickMovimiento != null)
        {
            if (Mathf.Abs(joystickMovimiento.Horizontal) > 0.05f) moverX = joystickMovimiento.Horizontal;
            if (Mathf.Abs(joystickMovimiento.Vertical) > 0.05f) moverZ = joystickMovimiento.Vertical;
        }

        Vector3 adelante = transform.forward;
        adelante.y = 0f;
        adelante.Normalize();

        Vector3 derecha = transform.right;
        derecha.y = 0f;
        derecha.Normalize();

        Vector3 movimiento = (derecha * moverX + adelante * moverZ).normalized;
        transform.position += movimiento * velocidadCaminar * Time.deltaTime;
    }
}