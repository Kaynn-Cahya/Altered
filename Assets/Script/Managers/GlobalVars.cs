/// <summary>
///  Session based; For persistence check SaveManager.cs
/// </summary>
public class GlobalVars : Singleton<GlobalVars> {
    public bool IsRetrying {
        get; set;
    }

    public bool TutorialFinished {
        get; set;
    }

    private void Start() {
        IsRetrying = false;
        TutorialFinished = false;
    }
}
