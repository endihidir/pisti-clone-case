using UnityBase.Service;
using UnityBase.UI.ButtonCore;
using VContainer;

public class PlayAgainButton : ButtonBehaviour
{
    [Inject] 
    private readonly ISceneGroupLoadService _sceneGroupLoadService;

    protected override void OnClick()
    {
        _sceneGroupLoadService.LoadSceneAsync(SceneType.Gameplay, true);
    }
}