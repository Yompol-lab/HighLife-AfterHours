using UnityEngine;

public class ItemPancheria : MonoBehaviour
{
    public string nombrePedido = "Pan";

    [Header("Ajuste visual en la mano")]
    public Vector3 posicionEnMano = Vector3.zero;
    public Vector3 rotacionEnMano = Vector3.zero;
    public float escalaEnMano = 1f; 

    private Vector3 escalaOriginal;

    void Awake()
    {
        escalaOriginal = transform.localScale;
    }

    public void BloquearEnMano(Transform puntoMano)
    {
        if (TryGetComponent(out Collider col)) col.enabled = false;

        transform.SetParent(puntoMano, false);
        transform.localPosition = posicionEnMano;
        transform.localRotation = Quaternion.Euler(rotacionEnMano);
        transform.localScale = escalaOriginal * escalaEnMano;
    }

    public void DejarEnMesa(Vector3 puntoDestino)
    {
        if (TryGetComponent(out Collider col)) col.enabled = true;

        transform.SetParent(null);
        transform.position = puntoDestino;
        transform.rotation = Quaternion.Euler(rotacionEnMano);
        transform.localScale = escalaOriginal;
    }
}