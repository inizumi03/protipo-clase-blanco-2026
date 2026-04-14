using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SistemaPuntosMirada : MonoBehaviour
{
    [Header("Detección")]
    public float distancia = 10f;
    public LayerMask capaProfesor;

    [Header("Puntos")]
    public float puntos = 0f;
    public float puntosPorSegundo = 5f;
    public float puntosParaGanar = 100f;

    [Header("UI")]
    public TMP_Text textoPuntos; //  CAMBIO
    public GameObject mensajeGanaste;

    private bool gano = false;

    void Start()
    {
        if (mensajeGanaste != null)
            mensajeGanaste.SetActive(false);
    }

    void Update()
    {
        if (gano) return;

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distancia, capaProfesor))
        {
            puntos += puntosPorSegundo * Time.deltaTime;
        }

        // Actualizar UI
        if (textoPuntos != null)
        {
            textoPuntos.text = "" + Mathf.FloorToInt(puntos);
        }

        //  Ganar
        if (puntos >= puntosParaGanar)
        {
            Ganar();
        }
    }

    void Ganar()
    {
        gano = true;

        Time.timeScale = 0f;

        if (mensajeGanaste != null)
            mensajeGanaste.SetActive(true);

        Debug.Log("GANASTE");
    }
}
