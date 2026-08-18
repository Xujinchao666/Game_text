using System.Collections;
using UnityEngine;

public class house_color_change : MonoBehaviour
{

    public float originAlpha = 1f; //原始透明度

    public float targetAlpha = 0.5f; //目标透明度
    public float fadeDurtion = 0.2f;//渐变时间
    public SpriteRenderer _sp;//精灵
    public Coroutine _coroutine;

    private void Awake()
    {
        _sp = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }

            _coroutine = StartCoroutine(FadeToAlpha(targetAlpha));
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }

            _coroutine = StartCoroutine(FadeToAlpha(originAlpha));
        }
    }



    public IEnumerator FadeToAlpha(float targetAlpha)
    {
        Color startColor = _sp.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b,targetAlpha);

        float timer = 0;
        while (timer < fadeDurtion)
        {
            _sp.color = Color.Lerp(startColor, endColor, timer / fadeDurtion);
            timer += Time.deltaTime;
            yield return null;
        }

        _sp.color = endColor;//保证透明度255
        _coroutine = null;

    }
}
