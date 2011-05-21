using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Phone.Controls;
using FlagMatch.ViewModels;
using System.Windows.Media;

namespace FlagMatch
{
    public partial class GamePage : PhoneApplicationPage
    {
        public GamePage() 
        {
            InitializeComponent();            
        }

        private GameViewModel _gameViewModel = new GameViewModel(20);

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            // setup the layout root datacontext
            LayoutRoot.DataContext = _gameViewModel;

            // Check to see when the game ends 
            _gameViewModel.GameEnded += new EventHandler(GameEnded);
            
            // update the color models to apply to different themes
            if (Resources["PhoneBackgroundColor"].ToString() == "#FF000000")
            {
                flagBorder.Stroke = new SolidColorBrush(Colors.White);
                highScoreGrid.Background = new SolidColorBrush(Colors.Black);                
            }
            else
            {
                flagBorder.Stroke = new SolidColorBrush(Colors.Black);
                highScoreGrid.Background = new SolidColorBrush(Colors.White);
            }
        }

        private void GameEnded(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }
}