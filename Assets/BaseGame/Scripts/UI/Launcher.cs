using Cysharp.Threading.Tasks;
using TW.UGUI.Core.Screens;
using TW.UGUI.Core.Views;
using ZBase.UnityScreenNavigator.Core;

public class Launcher : UnityScreenNavigatorLauncher
{
    protected override void Start()
    {
        base.Start();
        ShowLoadingScreen().Forget();
    }

    private async UniTaskVoid ShowLoadingScreen()
    {
        ViewOptions options = new ViewOptions(nameof(ScreenTitle), false, loadAsync: false);
        await ScreenContainer.Find(ContainerKey.Screens).PushAsync(options);
    }
}
