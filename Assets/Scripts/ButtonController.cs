using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] public bool isReadyToRender = false;
    [HideInInspector] public bool isClicked = false;
    [HideInInspector] public bool isRoot = false;
    [HideInInspector] public int buttonIndex = 0;
    [HideInInspector] public ButtonController previousButton;

    [Header("Sprite properties")]
    [SerializeField, Tooltip("Default renderer, on click sprite will change to activeSprite")] 
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite activeSprite;

    [Header("Rope properties")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField, Tooltip("Speed at which the rope animation plays")]
    private float lineRenderAnimationSpeed = 20f;

    [Header("Text number properties")]
    [SerializeField] private TextMeshPro numberText;
    [SerializeField, Tooltip("Duration (seconds) of the text fade-out animation")]
    private float textFadeOutAnimationDuration = 1f;

    public event EventHandler OnRopeFinishedDrawing;

    private void Start()
    {
        if (previousButton == null || lineRenderer == null || numberText == null)
        {
            Debug.LogError("Critical dependencies are missing. Destroying this ButtonController.");
            Destroy(this);
            return;
        }

        OnRopeFinishedDrawing += (_, _) => GameManager.Instance.IncrementButtonCount();

        numberText.text = $"{buttonIndex+1}";
        Vector3 startPos = previousButton.transform.position;
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, startPos);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Validation: Ensure the button can be clicked
        if (isClicked || (!isRoot && !previousButton.isClicked))
            return;

        spriteRenderer.sprite = activeSprite;
        isClicked = true;
        StartCoroutine(FadeOutText());

        // Handle rope logic
        if (previousButton.isRoot || previousButton.isReadyToRender)
            StartCoroutine(RenderLine());
        else
            previousButton.OnRopeFinishedDrawing += (_, _) => StartCoroutine(RenderLine());

    }
    private IEnumerator FadeOutText()
    {
        float elapsedTime  = 0f;
        var originalColor = numberText.color;
        var targetColor  = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        while (elapsedTime  < textFadeOutAnimationDuration)
        {
            numberText.color = Color.Lerp(originalColor, targetColor , elapsedTime  / textFadeOutAnimationDuration);
            elapsedTime  += Time.deltaTime;
            yield return null;
        }

        numberText.gameObject.SetActive(false);
    }
    private IEnumerator RenderLine()
    {
        float elapsedTime = 0f;
        float animationDuration  = (transform.position - previousButton.transform.position).magnitude / lineRenderAnimationSpeed;

        Vector3 startPosition = lineRenderer.GetPosition(0);
        Debug.Log($"Rendering rope from {startPosition} to {transform.position}");
        while (elapsedTime < animationDuration)
        {
            Vector3 endPostion = Vector3.Lerp(startPosition, transform.position, elapsedTime / animationDuration );
            lineRenderer.SetPosition(1, endPostion);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isReadyToRender = true;
        OnRopeFinishedDrawing?.Invoke(this, EventArgs.Empty);
    }
}
