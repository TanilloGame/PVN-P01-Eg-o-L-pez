using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlataformController : MonoBehaviour
{
    [SerializeField] private float startXPosition;
    [SerializeField] private float endXPosition;
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;
    [SerializeField] private float movementTime;
    [SerializeField] private float colorTime;
    [SerializeField] private Ease movementEase;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update

    private void Awake()
    {
        
        spriteRenderer = GetComponent<SpriteRenderer>();   
    }
    void Start()
    {
        spriteRenderer.color = startColor;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(spriteRenderer.DOColor(endColor, colorTime));
        sequence.Append(transform.DOMoveX(endXPosition, movementTime).SetEase(movementEase));
        sequence.Append(spriteRenderer.DOColor(startColor, colorTime));
        sequence.Append(transform.DOMoveX(startXPosition, movementTime).SetEase(movementEase));
        sequence.Append(spriteRenderer.DOColor(endColor, colorTime));
        sequence.SetLoops(-1);

        spriteRenderer.DOFade(0, 10).OnComplete(() =>
        {
            sequence.Kill();
            Destroy(gameObject);



        });
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
