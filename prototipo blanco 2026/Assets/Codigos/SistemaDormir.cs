using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SistemaDormir : MonoBehaviour
{
    [Header("Referencia")]
    public Image parpado;

    [Header("UI Flechas")]
    public List<Image> flechasUI = new List<Image>();

    public Color colorNormal = Color.gray;
    public Color colorActivo = Color.white;
    public Color colorCorrecto = Color.green;
    public Color colorError = Color.red;

    [Header("Sprites Flechas")]
    public Sprite flechaArriba;
    public Sprite flechaAbajo;
    public Sprite flechaIzquierda;
    public Sprite flechaDerecha;

    [Header("Parpadeo")]
    public float tiempoCierre = 0.15f;
    public float tiempoApertura = 0.2f;
    public float tiempoCerradoBase = 0.05f;

    [Header("Cansancio")]
    public float cansancio = 0f;
    public float aumentoPorParpadeo = 0.1f;
    public float maxCansancio = 1f;

    [Header("Intervalo")]
    public float intervaloMin = 3f;
    public float intervaloMax = 6f;

    [Header("Minijuego")]
    public List<KeyCode> secuencia = new List<KeyCode>();
    private int inputIndex = 0;
    private bool enMinijuego = false;

    void Start()
    {
        StartCoroutine(CicloParpadeo());
        GenerarSecuencia(4);
        OcultarFlechas();
    }

    IEnumerator CicloParpadeo()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(intervaloMin, intervaloMax));

            yield return StartCoroutine(Parpadear());

            cansancio += aumentoPorParpadeo;

            if (cansancio >= maxCansancio)
            {
                Debug.Log("SE DURMIÓ - GAME OVER");
                Time.timeScale = 0;
            }
        }
    }

    IEnumerator Parpadear()
    {
        float tiempoCerrado = tiempoCerradoBase + cansancio;

        float t = 0;

        // CERRAR
        while (t < tiempoCierre)
        {
            t += Time.deltaTime;
            float alpha = Mathf.SmoothStep(0, 1, t / tiempoCierre);
            parpado.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        yield return new WaitForSeconds(tiempoCerrado);

        // ABRIR
        t = 0;
        while (t < tiempoApertura)
        {
            t += Time.deltaTime;
            float alpha = Mathf.SmoothStep(1, 0, t / tiempoApertura);
            parpado.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }

    void Update()
    {
        if (cansancio > 0.5f && !enMinijuego)
        {
            ActivarMinijuego();
        }

        if (enMinijuego)
        {
            DetectarInput();
        }
    }

    // ---------------- MINIJUEGO ----------------

    void GenerarSecuencia(int longitud)
    {
        secuencia.Clear();
        KeyCode[] opciones = { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow };

        for (int i = 0; i < longitud; i++)
        {
            secuencia.Add(opciones[Random.Range(0, opciones.Length)]);
        }
    }

    void ActivarMinijuego()
    {
        enMinijuego = true;
        inputIndex = 0;

        MostrarFlechas();
        ActualizarUI();
        ActualizarIndicador();

        Debug.Log("MINIJUEGO ACTIVADO");
    }

    void DetectarInput()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(secuencia[inputIndex]))
            {
                flechasUI[inputIndex].color = colorCorrecto;
                inputIndex++;

                if (inputIndex >= secuencia.Count)
                {
                    Debug.Log("SE DESPERTÓ");

                    cansancio = 0.2f;
                    enMinijuego = false;

                    OcultarFlechas();
                    GenerarSecuencia(4);
                }
                else
                {
                    ActualizarIndicador();
                }
            }
            else
            {
                flechasUI[inputIndex].color = colorError;

                Debug.Log("ERROR");
                cansancio += 0.1f;

                inputIndex = 0;
                ActualizarIndicador();
            }
        }
    }

    // ---------------- UI ----------------

    void MostrarFlechas()
    {
        for (int i = 0; i < flechasUI.Count; i++)
        {
            if (i < secuencia.Count)
                flechasUI[i].gameObject.SetActive(true);
            else
                flechasUI[i].gameObject.SetActive(false);
        }
    }

    void OcultarFlechas()
    {
        foreach (Image img in flechasUI)
        {
            img.gameObject.SetActive(false);
        }
    }

    void ActualizarUI()
    {
        for (int i = 0; i < secuencia.Count; i++)
        {
            flechasUI[i].sprite = ObtenerSprite(secuencia[i]);
            flechasUI[i].color = colorNormal;
        }
    }

    void ActualizarIndicador()
    {
        for (int i = 0; i < secuencia.Count; i++)
        {
            if (i == inputIndex)
                flechasUI[i].color = colorActivo;
            else if (flechasUI[i].color != colorCorrecto)
                flechasUI[i].color = colorNormal;
        }
    }

    // ---------------- SPRITES ----------------

    Sprite ObtenerSprite(KeyCode tecla)
    {
        switch (tecla)
        {
            case KeyCode.UpArrow: return flechaArriba;
            case KeyCode.DownArrow: return flechaAbajo;
            case KeyCode.LeftArrow: return flechaIzquierda;
            case KeyCode.RightArrow: return flechaDerecha;
        }

        return null;
    }
}
