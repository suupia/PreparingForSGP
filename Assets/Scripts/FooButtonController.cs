using UnityEngine;
using UnityEngine.UIElements;

public sealed class FooButtonController
{
    private readonly Button _button;

    public FooButtonController(VisualElement visualElement)
    {
        // VisualElementからButtonを取得する
        _button = visualElement.Q<Button>("FooButton");

        _button.clicked += OnClick;
    }

    /// <summary>
    ///     ボタンの文字列を設定する。
    /// </summary>
    /// <param name="text"></param>
    public void SetText(string text)
    {
        _button.text = text;
    }

    private void OnClick()
    {
        // クリックされたログを出力する
        Debug.Log($"Clicked: {_button.text}");
    }
}