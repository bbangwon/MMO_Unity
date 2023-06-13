using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    int _score = 0;

    private void Start()
    {
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        GetText((int)Texts.ScoreText).text = "Bind Text";
    }

    
    public void OnButtonClicked()
    {
        _score++;
    }
}
