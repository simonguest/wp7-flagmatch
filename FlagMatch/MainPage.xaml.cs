using System;
using System.Windows;
using FlagMatch.Commands;
using FlagMatch.Models;
using FlagMatch.Providers;
using FlagMatch.ViewModels;
using Microsoft.Phone.Controls;

namespace FlagMatch
{
    public partial class MainPage : PhoneApplicationPage
    {
        private readonly MainViewModel _mainViewModel = new MainViewModel();

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // set the data context
            LayoutRoot.DataContext = _mainViewModel;

            // listen to the navigate events from the view model
            CommandEvents.NewGameButtonPressed += new EventHandler(StartGame);
            CommandEvents.ResumeGameButtonPressed += new EventHandler(ResumeGame);
        }

        private void StartGame(object sender, EventArgs e)
        {
            // invalid the game state and move to game page
            _mainViewModel.DeleteGameState();
            NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
        }

        private void ResumeGame(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            // Validate all of the flag images are correct
            foreach (Flag f in Flag.All)
            {
                try
                {
                    if (new ResourceStreamImageProvider().GetImage(f.Name) == null)
                    {
                        throw new Exception("Missing flag image file.");
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show(String.Format("Error in reading file:{0}.  Please re-install application.", f.Name));
                }
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            // update the resume button binding if back button has been pressed
            _mainViewModel.UpdateGameButtonVisibility(this, null);
        }

    }
}
