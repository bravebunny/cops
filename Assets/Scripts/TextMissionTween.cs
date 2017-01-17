using UnityEngine;
using UnityEngine.UI;

public class TextMissionTween : MonoBehaviour {
    public float MidleXPosition;
    public float MidleSlowDistance;
    public float TextMovingTime;
    public float TextSlowMoTime;

    public void StartTextTween() {
        gameObject.SetActive(true);
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-Screen.width * 0.8F, Screen.height * 0.3F);
        LeanTween.moveX(gameObject.GetComponent<RectTransform>(), MidleXPosition - MidleSlowDistance*0.5F, TextMovingTime).setEase(LeanTweenType.easeInExpo).setOnComplete(MiddleTween);
    }

    void MiddleTween() {
        gameObject.GetComponentInParent<Image>().enabled = true;
        LeanTween.moveX(gameObject.GetComponent<RectTransform>(), MidleSlowDistance * 0.5F - MidleXPosition, TextSlowMoTime).setEase(LeanTweenType.linear).setOnComplete(EaseOut);
    }

    void EaseOut() {
        LeanTween.moveX(gameObject.GetComponent<RectTransform>(), Screen.width * 0.8F, TextMovingTime).setEase(LeanTweenType.easeOutExpo).setOnComplete(DisableText);
        gameObject.GetComponentInParent<Image>().enabled = false;
    }

    void DisableText() {
        gameObject.SetActive(false);
    }
}
