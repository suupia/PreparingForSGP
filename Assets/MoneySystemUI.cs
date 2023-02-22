using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MoneySystemUI : MonoBehaviour
{
    [SerializeField] UIDocument uiDocument;
    Button button;

    // Start is called before the first frame update
    void Start()
    {
       button = uiDocument.rootVisualElement.Query<Button>().First();
         button.clicked += OnButtonClicked;
        button.text = "Not clicked";
    }

    private void OnButtonClicked()
    {
        button.text = "Clicked";
    }


}
