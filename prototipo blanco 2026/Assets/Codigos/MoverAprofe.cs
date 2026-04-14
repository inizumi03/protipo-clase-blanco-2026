using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverAprofe : MonoBehaviour
{
    public Transform puntoA;
    public Transform puntoB;

    public float velocidad = 2f;

    private Transform objetivo;

    void Start()
    {
        objetivo = puntoB;
    }

    void Update()
    {
        // Mover hacia el objetivo
        transform.position = Vector3.MoveTowards(
            transform.position,
            objetivo.position,
            velocidad * Time.deltaTime
        );

        // Si llega al destino, cambiar
        if (Vector3.Distance(transform.position, objetivo.position) < 0.1f)
        {
            objetivo = (objetivo == puntoA) ? puntoB : puntoA;
        }
    }
}
