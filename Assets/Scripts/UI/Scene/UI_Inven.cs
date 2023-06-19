using UnityEngine;

public class UI_Inven : UI_Scene
{
    enum GameObjects
    {
        GridPanel
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));

        GameObject gridPanel = Get<GameObject>((int)GameObjects.GridPanel);
        foreach (Transform child in gridPanel.transform)
        {
            Managers.Resource.Destroy(child.gameObject);
        }

        for(int i = 0;i < 8;i++)
        {
            UI_Inven_Item item = Managers.UI.MakeSubItem<UI_Inven_Item>(parent: gridPanel.transform);
            item.SetInfo($"БэЗа°Л {i}");
        }
    }
}
