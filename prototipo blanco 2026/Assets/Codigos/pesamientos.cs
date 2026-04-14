using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class pesamientos : MonoBehaviour
{
    [Header("Imágenes")]
    public List<GameObject> imagenes = new List<GameObject>();

    [Header("Canvas")]
    public RectTransform canvasRect; 

    [Header("Tiempos")]
    public float tiempoInicial = 20f;
    public float tiempoEntreImagenes = 3f;
    public float duracionTotal = 10f;

    [Header("Movimiento")]
    public float velocidad = 100f;

    [Header("Fade")]
    public float duracionFade = 2f;

    private List<RectTransform> activas = new List<RectTransform>();
    private List<Vector2> direcciones = new List<Vector2>();

    private bool enMovimiento = false;

    void Start()
    {
        StartCoroutine(Secuencia());
    }

    void Update()
    {
        if (enMovimiento)
        {
            for (int i = 0; i < activas.Count; i++)
            {
                RectTransform rect = activas[i];

                
                rect.anchoredPosition += direcciones[i] * velocidad * Time.deltaTime;

                
                Vector2 pos = rect.anchoredPosition;

                float halfWidth = canvasRect.rect.width / 2;
                float halfHeight = canvasRect.rect.height / 2;

                float imgWidth = rect.rect.width / 2;
                float imgHeight = rect.rect.height / 2;

                
                if (pos.x + imgWidth > halfWidth || pos.x - imgWidth < -halfWidth)
                {
                    direcciones[i] = new Vector2(-direcciones[i].x, direcciones[i].y);
                }

                
                if (pos.y + imgHeight > halfHeight || pos.y - imgHeight < -halfHeight)
                {
                    direcciones[i] = new Vector2(direcciones[i].x, -direcciones[i].y);
                }
            }
        }
    }

    IEnumerator Secuencia()
    {
        while (true)
        {
            yield return new WaitForSeconds(tiempoInicial);

            activas.Clear();
            direcciones.Clear();

            enMovimiento = true;

            for (int i = 0; i < imagenes.Count; i++)
            {
                GameObject img = imagenes[i];
                img.SetActive(true);

                RectTransform rect = img.GetComponent<RectTransform>();
                Image imageComp = img.GetComponent<Image>();

                rect.anchoredPosition = Vector2.zero;

                Color c = imageComp.color;
                c.a = 1f;
                imageComp.color = c;

                Vector2 dir = Random.insideUnitCircle.normalized;

                activas.Add(rect);
                direcciones.Add(dir);

                yield return new WaitForSeconds(tiempoEntreImagenes);
            }

            yield return new WaitForSeconds(duracionTotal);

            float tFade = 0;

            while (tFade < duracionFade)
            {
                tFade += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, tFade / duracionFade);

                foreach (GameObject img in imagenes)
                {
                    Image imageComp = img.GetComponent<Image>();
                    Color c = imageComp.color;
                    c.a = alpha;
                    imageComp.color = c;
                }

                yield return null;
            }

            foreach (GameObject img in imagenes)
            {
                img.SetActive(false);
            }

            enMovimiento = false;
        }
    }
}
