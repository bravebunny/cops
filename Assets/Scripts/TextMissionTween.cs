using UnityEngine;
using UnityEngine.UI;

public class TextMissionTween : MonoBehaviour {
    public float MidleXPosition;
    public float MidleSlowDistance;
    public float YPosition;
    public float TextMovingTime;
    public float TextSlowMoTime;

    public void StartTextTween() {
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-Screen.width, YPosition);
        LeanTween.moveX(gameObject.GetComponent<RectTransform>(), MidleXPosition - MidleSlowDistance*0.5F, TextMovingTime).setEase(LeanTweenType.easeInExpo).setOnComplete(MiddleTween);
    }

    void MiddleTween() {
        LeanTween.moveX(gameObject.GetComponent<RectTransform>(), MidleSlowDistance * 0.5F - MidleXPosition, TextSlowMoTime).setEase(LeanTweenType.linear).setOnComplete(EaseOut);
    }

    void EaseOut() {
        LeanTween.moveX(gameObject.GetComponent<RectTransform>(), Screen.width, TextMovingTime).setEase(LeanTweenType.easeOutExpo);
    }
}
