using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanzarAvion : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject prefabAvion;
    public Transform puntoLanzamiento;
    public GameObject imagenUI;

    [Header("Fuerza")]
    public float fuerza = 500f;

    [Header("Tiempos")]
    public float tiempoCooldown = 10f;
    public float tiempoVidaAvion = 5f;

    private bool puedeLanzar = false;

    void Start()
    {
        if (imagenUI != null)
            imagenUI.SetActive(false);

        StartCoroutine(CooldownLoop());
    }

    void Update()
    {
        if (puedeLanzar && Input.GetKeyDown(KeyCode.Q))
        {
            Lanzar();
        }
    }

    IEnumerator CooldownLoop()
    {
        while (true)
        {
           
            yield return new WaitForSeconds(tiempoCooldown);

            puedeLanzar = true;

            if (imagenUI != null)
                imagenUI.SetActive(true);

            
            while (puedeLanzar)
            {
                yield return null;
            }
        }
    }

    void Lanzar()
    {
        
        GameObject avion = Instantiate(prefabAvion, puntoLanzamiento.position, puntoLanzamiento.rotation);

        Rigidbody rb = avion.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(puntoLanzamiento.forward * fuerza);
        }

        
        if (imagenUI != null)
            imagenUI.SetActive(false);

        
        Destroy(avion, tiempoVidaAvion);

        puedeLanzar = false;
    }
}
