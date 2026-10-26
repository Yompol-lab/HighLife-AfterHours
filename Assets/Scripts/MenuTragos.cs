using UnityEngine;

public class MenuTragos : MonoBehaviour
{
    [Header("Niveles del Menú")]
    public GameObject menuPrincipal; // Tu objeto "Menu"
    public GameObject menuCategorias; // Tu objeto "Menu-Drink"

    [Header("Las 10 Pantallas de Tragos")]
    public GameObject drink1;
    public GameObject drink2;
    public GameObject drink3;
    public GameObject drink4;
    public GameObject drink5;
    public GameObject drink6;
    public GameObject drink7;
    public GameObject drink8;
    public GameObject drink9;
    public GameObject drink10;

    void Start()
    {
        // Al darle Play, arranca en el primer menú
        AbrirMenuPrincipal();
    }

    // --- NAVEGACIÓN ENTRE LOS MENÚS PRINCIPALES ---
    public void AbrirMenuPrincipal()
    {
        CerrarTodo();
        if (menuPrincipal != null) menuPrincipal.SetActive(true);
    }

    public void AbrirCategoriasTragos()
    {
        CerrarTodo();
        if (menuCategorias != null) menuCategorias.SetActive(true);
    }

    // Esta es la función que usarán los botones de los nombres para volver atrás
    public void VolverACategorias()
    {
        AbrirCategoriasTragos();
    }

    // --- ABRIR CADA TRAGO INDIVIDUAL ---
    public void MostrarDrink1() { CerrarTodo(); if (drink1 != null) drink1.SetActive(true); }
    public void MostrarDrink2() { CerrarTodo(); if (drink2 != null) drink2.SetActive(true); }
    public void MostrarDrink3() { CerrarTodo(); if (drink3 != null) drink3.SetActive(true); }
    public void MostrarDrink4() { CerrarTodo(); if (drink4 != null) drink4.SetActive(true); }
    public void MostrarDrink5() { CerrarTodo(); if (drink5 != null) drink5.SetActive(true); }
    public void MostrarDrink6() { CerrarTodo(); if (drink6 != null) drink6.SetActive(true); }
    public void MostrarDrink7() { CerrarTodo(); if (drink7 != null) drink7.SetActive(true); }
    public void MostrarDrink8() { CerrarTodo(); if (drink8 != null) drink8.SetActive(true); }
    public void MostrarDrink9() { CerrarTodo(); if (drink9 != null) drink9.SetActive(true); }
    public void MostrarDrink10() { CerrarTodo(); if (drink10 != null) drink10.SetActive(true); }

    // --- APAGA TODO PARA LIMPIAR LA PANTALLA ---
    private void CerrarTodo()
    {
        if (menuPrincipal != null) menuPrincipal.SetActive(false);
        if (menuCategorias != null) menuCategorias.SetActive(false);

        if (drink1 != null) drink1.SetActive(false);
        if (drink2 != null) drink2.SetActive(false);
        if (drink3 != null) drink3.SetActive(false);
        if (drink4 != null) drink4.SetActive(false);
        if (drink5 != null) drink5.SetActive(false);
        if (drink6 != null) drink6.SetActive(false);
        if (drink7 != null) drink7.SetActive(false);
        if (drink8 != null) drink8.SetActive(false);
        if (drink9 != null) drink9.SetActive(false);
        if (drink10 != null) drink10.SetActive(false);
    }
}