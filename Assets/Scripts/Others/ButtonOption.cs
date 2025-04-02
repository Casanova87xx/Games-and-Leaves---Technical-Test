using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class ButtonOption : MonoBehaviour
{
    public int Option;

    private Image _image;
    private Button _button;

    void Awake()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();
    }

    public void Reset()
    {
        _image.color = Color.white;
        _button.interactable = true;
    }
    
    public void Select()
    {
        _image.color = Color.green;
    }

    public void Disable()
    {
        _button.interactable = false;
    }
}
