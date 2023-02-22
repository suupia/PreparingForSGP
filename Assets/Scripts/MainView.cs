using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public sealed class MainView : MonoBehaviour
{
    [SerializeField] private VisualTreeAsset _elementTemplate;

    private void OnEnable()
    {
        // UIDocumentコンポーネントについては次節で説明
        var uiDocument = GetComponent<UIDocument>();

        var buttonTexts = new List<string>
        {
            "First",
            "Second",
            "Third"
        };

        new FooListController(uiDocument.rootVisualElement, _elementTemplate, buttonTexts);
    }
}