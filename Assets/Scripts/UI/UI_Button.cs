using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Button : UI_Base
{   
    enum Buttons
    {
        PointButton,
    }

    enum Texts
    {
        PointText,
        ScoreText,
    }

    enum GameObjects
    {
        TestObject,
    }

    enum Images
    {
        ItemIcon,
    }

    int _score = 0;

    private void Start()
    {
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        GetText((int)Texts.ScoreText).text = "Bind Text";

        GameObject go = GetImage((int)Images.ItemIcon).gameObject;
        var eventHandler = go.GetComponent<UI_EventHandler>();
        eventHandler.OnDragHandler += (PointerEventData data) => { eventHandler.gameObject.transform.position = data.position; };

    }

    
    public void OnButtonClicked()
    {
        _score++;
    }
}
