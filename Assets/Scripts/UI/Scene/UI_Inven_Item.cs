using TMPro;
using UnityEngine;

public class UI_Inven_Item : UI_Base
{
    enum GameObjects
    {
        ItemIcon,
        ItemNameText
    }

    string _name;


    void Start()
    {
        Init();   
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));

        Get<GameObject>((int)GameObjects.ItemNameText).GetComponent<TextMeshProUGUI>().text = _name;

        Get<GameObject>((int)GameObjects.ItemIcon).AddUIEvent(pointerEventData => { Debug.Log($"아이템 클릭! {_name}"); });
    }

    public void SetInfo(string name)
    {
        _name = name;
    }
}
