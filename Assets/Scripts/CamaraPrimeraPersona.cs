using UnityEngine;
using UnityEngine.InputSystem;

public class CamaraPrimeraPersona : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidadCaminar = 4f;
    public float sensibilidadGiro = 0.5f;

    private float rotacionX = 0f;
    private float rotacionY = 0f;

    void OnEnable()
    {
        rotacionX = transform.localEulerAngles.x;
        rotacionY = transform.localEulerAngles.y;
    }

    void Update()
    {
        MirarConPantalla();
        Caminar();
    }

    void MirarConPantalla()
    {
        
        if (Pointer.current == null) return;

        
        if (Pointer.current.press.isPressed)
        {
            Vector2 delta = Pointer.current.delta.ReadValue() * sensibilidadGiro;

            rotacionY += delta.x;
            rotacionX -= delta.y;

            rotacionX = Mathf.Clamp(rotacionX, -85f, 85f);
            transform.localRotation = Quaternion.Euler(rotacionX, rotacionY, 0f);
        }
    }

    void Caminar()
    {
        if (Keyboard.current == null) return;

        float moverX = 0f;
        float moverZ = 0f;

        if (Keyboard.current.wKey.isPressed) moverZ = 1f;
        if (Keyboard.current.sKey.isPressed) moverZ = -1f;
        if (Keyboard.current.aKey.isPressed) moverX = -1f;
        if (Keyboard.current.dKey.isPressed) moverX = 1f;

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