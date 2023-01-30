using System.Windows.Input;
using System.Windows.Media;

namespace Inscription_mp.Scenes.MainScene.SettingsScene
{
    internal class SettingScene : Scene
    {
        public SettingScene() : base(new SettingsView())
        {
            
        }

        public override void Scene_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    MainWindow.Scene = new MainScene();
                    break;
            }
        }
    }
}
