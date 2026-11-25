using UnityEngine;

public class CamaraZonasBarra : MonoBehaviour
{
    [Header("Cámara y Zonas (en orden de izquierda a derecha)")]
    public Transform camaraBarra;
    public Transform[] zonas;
    public int zonaInicial = 0;

    [Header("Smooth Slide")]
    public float tiempoSlide = 0.25f; 
    public float velocidadGiro = 10f; 
    public bool esCiclico = false;    

    [Header("Flechas (Opcional - se ocultan en los extremos)")]
    public GameObject botonIzquierda;
    public GameObject botonDerecha;

    private int indiceActual;
    private Vector3 velocidadActual = Vector3.zero;
    private bool enMovimiento = false;

    void Start()
    {
        if (camaraBarra == null) camaraBarra = transform;
        if (zonas == null || zonas.Length == 0) return;

        indiceActual = Mathf.Clamp(zonaInicial, 0, zonas.Length - 1);
        camaraBarra.position = zonas[indiceActual].position;
        camaraBarra.rotation = zonas[indiceActual].rotation;
        ActualizarFlechas();
    }

    void Update()
    {
        if (!enMovimiento || zonas == null || zonas.Length == 0) return;

        Transform objetivo = zonas[indiceActual];

        camaraBarra.position = Vector3.SmoothDamp(
            camaraBarra.position,
            objetivo.position,
            ref velocidadActual,
            tiempoSlide
        );

        camaraBarra.rotation = Quaternion.Slerp(
            camaraBarra.rotation,
            objetivo.rotation,
            1f - Mathf.Exp(-velocidadGiro * Time.deltaTime)
        );

        if (Vector3.Distance(camaraBarra.position, objetivo.position) < 0.005f &&
            Quaternion.Angle(camaraBarra.rotation, objetivo.rotation) < 0.1f)
        {
            camaraBarra.position = objetivo.position;
            camaraBarra.rotation = objetivo.rotation;
            velocidadActual = Vector3.zero;
            enMovimiento = false;
        }
    }

    public void IrIzquierda()
    {
        if (zonas == null || zonas.Length == 0) return;

        if (indiceActual > 0)
            indiceActual--;
        else if (esCiclico)
            indiceActual = zonas.Length - 1;
        else
            return;

        enMovimiento = true;
        ActualizarFlechas();
    }

    public void IrDerecha()
    {
        if (zonas == null || zonas.Length == 0) return;

        if (indiceActual < zonas.Length - 1)
            indiceActual++;
        else if (esCiclico)
            indiceActual = 0;
        else
            return;

        enMovimiento = true;
        ActualizarFlechas();
    }

    public void IrAZona(int indice)
    {
        if (zonas == null || indice < 0 || indice >= zonas.Length) return;

        indiceActual = indice;
        enMovimiento = true;
        ActualizarFlechas();
    }

    private void ActualizarFlechas()
    {
        if (esCiclico)
        {
            if (botonIzquierda != null) botonIzquierda.SetActive(true);
            if (botonDerecha != null) botonDerecha.SetActive(true);
            return;
        }

        if (botonIzquierda != null) botonIzquierda.SetActive(indiceActual > 0);
        if (botonDerecha != null) botonDerecha.SetActive(indiceActual < zonas.Length - 1);
    }
}