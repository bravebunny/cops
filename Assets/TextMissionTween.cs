using UnityEngine;
using UnityEngine.UI;

public class TextMissionTween : MonoBehaviour {
    public float StartXPosition;
    public float MidleXPosition;
    public float EndXPosition;
    public float MidleSlowDistance;
    public float YPosition;

    public void StartTextTween() {
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(StartXPosition, YPosition);
        LeanTween.moveX(gameObject.GetComponent<RectTransform>(), MidleXPosition - MidleSlowDistance*0.5F, 0.5f).setEase(LeanTweenType.easeInExpo).setOnComplete(MiddleTween);
    }

    void MiddleTween() {
        LeanTween.moveX(gameObject.GetComponent<RectTransform>(), MidleSlowDistance * 0.5F - MidleXPosition, 2f).setEase(LeanTweenType.linear).setOnComplete(EaseOut);
    }

    void EaseOut() {
        LeanTween.moveX(gameObject.GetComponent<RectTransform>(), EndXPosition, 0.5f).setEase(LeanTweenType.easeOutExpo);
    }
}
