using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Parpadeo : MonoBehaviour
{
    public Image parpado;

    [Header("Tiempos")]
    public float tiempoCierre = 0.15f;
    public float tiempoCerrado = 0.05f;
    public float tiempoApertura = 0.2f;

    [Header("Intervalo")]
    public float intervaloMin = 3f;
    public float intervaloMax = 7f;

    void Start()
    {
        StartCoroutine(CicloParpadeo());
    }

    IEnumerator CicloParpadeo()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(intervaloMin, intervaloMax));
            yield return StartCoroutine(Parpadear());
        }
    }

    IEnumerator Parpadear()
    {
        // CERRAR (suave tipo easing)
        float t = 0;
        while (t < tiempoCierre)
        {
            t += Time.deltaTime;
            float progreso = t / tiempoCierre;

            // curva suave (ease in)
            float alpha = Mathf.SmoothStep(0, 1, progreso);

            parpado.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        // pequeña pausa (ojo cerrado)
        yield return new WaitForSeconds(tiempoCerrado);

        // ABRIR (un poco más lento y natural)
        t = 0;
        while (t < tiempoApertura)
        {
            t += Time.deltaTime;
            float progreso = t / tiempoApertura;

            // curva suave (ease out)
            float alpha = Mathf.SmoothStep(1, 0, progreso);

            parpado.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }
}
