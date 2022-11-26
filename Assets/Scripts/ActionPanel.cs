using UnityEngine;
using UnityEngine.UI;

public class ActionPanel : MonoBehaviour
{
    [SerializeField] private Button[] changeablButtons;

    public void Activate(bool value)
    {
        gameObject.SetActive(true);
        for (int i = 0; i < changeablButtons.Length; i++)
        {
            changeablButtons[i].interactable = value;
        }
    }
}
