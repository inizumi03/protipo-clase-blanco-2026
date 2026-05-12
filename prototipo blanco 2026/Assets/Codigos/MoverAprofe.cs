using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverAprofe : MonoBehaviour
{
    [Header("Puntos de movimiento")]
    public Transform[] puntos;

    [Header("Movimiento")]
    public float velocidad = 2f;

    private int indiceActual = 0;

    void Update()
    {
        // Evita errores si no hay puntos
        if (puntos.Length == 0)
            return;

        // Punto actual al que va
        Transform objetivo = puntos[indiceActual];

        // Movimiento
        transform.position = Vector3.MoveTowards(
            transform.position,
            objetivo.position,
            velocidad * Time.deltaTime
        );

        // Cuando llega al punto
        if (Vector3.Distance(transform.position, objetivo.position) < 0.1f)
        {
            indiceActual++;

            // Si llega al final vuelve al inicio
            if (indiceActual >= puntos.Length)
            {
                indiceActual = 0;
            }
        }
    }
}

