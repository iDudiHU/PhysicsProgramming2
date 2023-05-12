using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenSpaceUIElement : MonoBehaviour
{
    private Slider slider;
    public Transform target;
    private Camera cam;
    private float scaleFactor = 0.03f;
    private RectTransform rectTransform;
    private Coroutine animateSliderCoroutine;

    #region Unity Functions
    private void Start()
    {
        cam = Camera.main;
        rectTransform = GetComponent<RectTransform>();
        slider = GetComponent<Slider>();
    }

    public void LateUpdate()
    {
        if (target)
        {
            UpdateCanvasPositionAndScale();
        }
    }
    #endregion

    #region Private Functions
    void UpdateCanvasPositionAndScale()
    {
        Vector3 screenPos = cam.WorldToScreenPoint(target.position);
        rectTransform.position = screenPos;

        float distance = Vector3.Distance(target.position, cam.transform.position);
        float scale = 1.5f + distance * scaleFactor;
        rectTransform.localScale = new Vector3(scale, scale, scale);
    }

    IEnumerator AnimateSlider()
    {
        float startTime = Time.time;
        while (Time.time - startTime < 1f)
        {
            slider.value = Time.time - startTime;
            yield return null;
        }
        slider.value = 1f;
    }
    #endregion

    #region Public Functions

    public void StartSliderAnimation()
    {
        animateSliderCoroutine = StartCoroutine(AnimateSlider());
    }

    public void StopSliderAnimation(bool resetValue = true)
    {
        if (animateSliderCoroutine != null)
        {
            StopCoroutine(animateSliderCoroutine);
            if (slider.value == 1f)
			{
                transform.gameObject.SetActive(false);
			}
            if (resetValue)
            {
                slider.value = 0f;
            }
        }
    }
    #endregion
}
