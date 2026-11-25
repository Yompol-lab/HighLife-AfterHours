using UnityEngine;
using TMPro;

public class DineroManager : MonoBehaviour
{
    public static DineroManager Instance;

    public int dineroActual = 2000;
    public TMP_Text textoDinero;

    void Awake()
    {
        Instance = this;
        ActualizarUI();
    }

    public void SumarDinero(int cantidad)
    {
        dineroActual += cantidad;
        ActualizarUI();
    }

    void ActualizarUI()
    {
        if (textoDinero != null)
            textoDinero.text = "$" + dineroActual;
    }
}