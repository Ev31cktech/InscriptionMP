﻿using System.Windows.Controls;
using System.Windows.Input;

namespace Inscription_mp.Scenes.MainScene
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : Page
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        public void Settings_KeyDown(object sender, KeyEventArgs e)
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
