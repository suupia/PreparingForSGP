using System.Collections.Generic;
using UnityEngine.UIElements;

public sealed class FooListController
{
    private IReadOnlyList<string> _buttonTexts;
    private readonly VisualTreeAsset _elementTemplate;
    private readonly ListView _fooList;

    public FooListController(VisualElement root, VisualTreeAsset elementTemplate, List<string> buttonTexts)
    {
        _fooList = root.Q<ListView>("FooList");
        _elementTemplate = elementTemplate;

        _fooList.makeItem = () =>
        {
            // リスト要素をインスタンス化して返す
            var element = _elementTemplate.Instantiate();
            var buttonController = new FooButtonController(element);
            element.userData = buttonController; // ControllerはuserDataという汎用データ受け渡し用フィールドに格納しておく

            return element;
        };

        _fooList.bindItem = (item, index) =>
        {
            // リスト要素にデータを設定する
            var controller = (FooButtonController)item.userData;
            controller.SetText(buttonTexts[index]);
        };

        _fooList.fixedItemHeight = 120;
        _fooList.itemsSource = buttonTexts;
    }
}