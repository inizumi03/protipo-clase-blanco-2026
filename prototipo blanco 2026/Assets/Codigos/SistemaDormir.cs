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

    [Header("UI Energía")]
    public Image barraCansancio;

    [Header("Energía")]
    public float energia = 100f;
    public float energiaMax = 100f;
    public float perdidaPorParpadeo = 5f;

    [Header("Parpadeo")]
    public float tiempoCierre = 0.15f;
    public float tiempoApertura = 0.2f;
    public float tiempoCerradoBase = 0.05f;

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

    void Update()
    {
        // -------- BARRA DE ENERGÍA --------
        float objetivo = energia / energiaMax;

        barraCansancio.fillAmount = Mathf.Lerp(
            barraCansancio.fillAmount,
            objetivo,
            Time.deltaTime * 5f
        );

        // Verde (lleno)  Rojo (vacío)
        barraCansancio.color = Color.Lerp(Color.red, Color.green, objetivo);

        // -------- ACTIVAR MINIJUEGO --------
        if (energia <= 25f && !enMinijuego)
        {
            ActivarMinijuego();
        }

        if (enMinijuego)
        {
            DetectarInput();
        }
    }

    IEnumerator CicloParpadeo()
    {
        while (true)
        {
            // Intervalo dinámico (menos energía = más parpadeo)
            float factor = energia / energiaMax;
            float intervalo = Mathf.Lerp(1f, 5f, factor);

            yield return new WaitForSeconds(intervalo);

            yield return StartCoroutine(Parpadear());

            // Pierde energía
            energia -= perdidaPorParpadeo;

            if (energia <= 0)
            {
                Debug.Log("SE DURMIÓ - GAME OVER");
                Time.timeScale = 0;
            }
        }
    }

    IEnumerator Parpadear()
    {
        float faltaEnergia = 1f - (energia / energiaMax);
        float tiempoCerrado = tiempoCerradoBase + (faltaEnergia * 0.2f);

        float t = 0;

        // CERRAR
        while (t < tiempoCierre)
        {
            t += Time.deltaTime;
            float alpha = Mathf.SmoothStep(0, 1, t / tiempoCierre);
            parpado.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        // OJO CERRADO
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

                    energia = 60f; // recupera energía
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
                energia -= 5f;

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
