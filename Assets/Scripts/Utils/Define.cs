public class Define
{
    public enum Layer
    {
        Monster = 6,
        Ground = 7,
        Block = 8
    }
    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Game,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum UIEvent
    {
        Click,
        Drag
    }
    public enum MouseEvent
    {
        Press,
        PointerDown,
        PointerUp,
        Click
    }
    public enum CameraMode
    {
        QuarterView
    }

    public enum State
    {
        Idle,
        Moving,
        Die,
        Skill,
    }
}
