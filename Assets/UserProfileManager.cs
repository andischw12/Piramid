using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserProfileManager : MonoBehaviour
{
    public static UserProfileManager instance; 
    public TextMeshProUGUI ProfileName;
    public Slider Health;
    public float SliderTime = 1.5f;
    [SerializeField] Image ColorOverScreen;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        ColorOverScreen.enabled = false;
        SetSlider(100);
    }

    public void SetSlider(float newVal) 
    {
        StartCoroutine(AnimateSliderOverTime(SliderTime, newVal));
    }
    IEnumerator AnimateSliderOverTime(float seconds,float NewSliderVal)
    {
        float animationTime = 0f;
        while (animationTime < seconds)
        {
            animationTime += Time.deltaTime;
            float lerpValue = animationTime / seconds;
            Health.value = Mathf.Lerp(Health.value, NewSliderVal, lerpValue);
            yield return null;
        }
    }

    public void HitEffect() 
    {
        StartCoroutine(HitEffectProcess());
    }

    IEnumerator HitEffectProcess() 
    {
        ColorOverScreen.enabled = true;

        float val = 0; 
        while(val<0.3f)
        {
            val += 0.075f;
            ColorOverScreen.color = new Color(ColorOverScreen.color.r, ColorOverScreen.color.g, ColorOverScreen.color.b, val);
            yield return new WaitForSeconds(0.05f);
        }
         
        yield return new WaitForSeconds(0.5f);

        while (val > 0f)
        {
            val -= 0.02f;
            ColorOverScreen.color = new Color(ColorOverScreen.color.r, ColorOverScreen.color.g, ColorOverScreen.color.b, val);
            yield return new WaitForSeconds(0.05f);
        }
        ColorOverScreen.enabled = false;
        // ColorOverScreen.enabled = false;
    }

     
}
