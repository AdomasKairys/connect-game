using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector]
    public bool isReadyToRender = false;
    [HideInInspector]
    public bool isClicked = false;
    [HideInInspector]
    public bool isRoot = false;
    [HideInInspector]
    public ButtonController previousButton;

    [Header("Sprite properties")]
    [SerializeField, Tooltip("Default renderer, on click sprite will change to activeSprite")] 
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite activeSprite;

    [Header("Rope properties")]
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private float lineRenderAnimationSpeed = 20f;
    [Header("Text number properties")]
    [SerializeField]
    private TextMeshPro numberText;
    [SerializeField]
    private float textFadeOutAnimationTime = 1f;

    public event EventHandler OnRopeFinishedDrawing;

    private void Start()
    {
        if (previousButton == null)
            return;
        lineRenderer.SetPosition(0, previousButton.transform.position);
        lineRenderer.SetPosition(1, previousButton.transform.position);
    }

    public void SetNumberText(string text) => numberText.text = text;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!isRoot && (previousButton == null || isClicked || !previousButton.isClicked))
            return;

        spriteRenderer.sprite = activeSprite;
        isClicked = true;
        StartCoroutine(FadeOutText());

        isReadyToRender = isRoot;

        if (!previousButton.isReadyToRender)
            previousButton.OnRopeFinishedDrawing += (_,_) => StartCoroutine(RenderLine());
        else
            StartCoroutine(RenderLine());
    }
    private IEnumerator FadeOutText()
    {
        float currentFadeOutTime = 0f;
        
        var color = numberText.color;

        while (currentFadeOutTime < textFadeOutAnimationTime)
        {
            color.a = Mathf.Lerp(1, 0, currentFadeOutTime / textFadeOutAnimationTime);
            numberText.color = color;
            currentFadeOutTime += Time.deltaTime;
            yield return null;
        }
        numberText.gameObject.SetActive(false);
    }
    private IEnumerator RenderLine()
    {
        float currentRenderTime = 0f;
        float scaledLineRenderAnimationTime = (transform.position - previousButton.transform.position).magnitude / lineRenderAnimationSpeed;

        while (currentRenderTime < scaledLineRenderAnimationTime)
        {
            var endPostion = Vector3.Lerp(lineRenderer.GetPosition(0), transform.position, currentRenderTime / scaledLineRenderAnimationTime);
            lineRenderer.SetPosition(1, endPostion);
            currentRenderTime += Time.deltaTime;
            yield return null;
        }
        OnRopeFinishedDrawing?.Invoke(this, EventArgs.Empty);
        isReadyToRender = true;
    }
}
