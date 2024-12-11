using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector]
    public bool isReadyToRender = false;
    [HideInInspector]
    public bool isActiveToClick = false;

    public ButtonController connectedButton;

    [Header("Sprite properties")]
    [SerializeField, Tooltip("Default renderer, on click sprite will change to activeSprite")] 
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite activeSprite;

    [Header("Rope properties")]
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private float lineRenderAnimationTime = 2f;
    [Header("Text number properties")]
    [SerializeField]
    private TextMeshPro numberText;
    [SerializeField]
    private float textFadeOutAnimationTime = 1f;

    private float _currentRenderTime = 0f;


    public event EventHandler OnPreviousRopeFinishedDrawing;

    private void Start()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(connectedButton == null || !isActiveToClick)
            return;

        spriteRenderer.sprite = activeSprite;


        connectedButton.isActiveToClick = true;

        if (isReadyToRender)
        {
            StartCoroutine(RenderLine());
            return;
        }

        if (isActiveToClick)
        {
            OnPreviousRopeFinishedDrawing += (_, _) => StartCoroutine(RenderLine());
        }
    }
    private IEnumerator FadeOutText()
    {
        yield return null;
    }
    private IEnumerator RenderLine()
    {
        while(_currentRenderTime < lineRenderAnimationTime)
        {
            var endPostion = Vector3.Lerp(lineRenderer.GetPosition(0), connectedButton.transform.position, _currentRenderTime / lineRenderAnimationTime);
            lineRenderer.SetPosition(1, endPostion);
            _currentRenderTime += Time.deltaTime;
            yield return null;
        }
        connectedButton.OnPreviousRopeFinishedDrawing?.Invoke(this, EventArgs.Empty);
        connectedButton.isReadyToRender = true;

    }
}
