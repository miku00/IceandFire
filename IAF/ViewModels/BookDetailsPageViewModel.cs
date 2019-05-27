using IAF.Models;
using IAF.Services;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace IAF.ViewModels
{
    public class BookDetailsPageViewModel : ViewModelBase
    {
        public INavigationService _navigationService;
        public ObservableCollection<Character> POVCharacters { get; set; } = new ObservableCollection<Character>();
        public Book DetailedBook { get; set; }
        private string releaseDate;
        public string ReleaseDate
        {
            get
            {
                return releaseDate;
            }
            set
            {
                SetProperty(ref releaseDate, value);
            }
        }
        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            DetailedBook = (Book)e.Parameter;
            ReleaseDate = DetailedBook.Released.Remove(DetailedBook.Released.Length - 9);
            var service = new IAFService();           
            foreach (var characterUrl in DetailedBook.PovCharacters)
            {
                var povc = await service.GetCharacterAsync(characterUrl);
                POVCharacters.Add(povc);                
            }
        }

        public BookDetailsPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public void POVCharacter_Clicked(object sender, ItemClickEventArgs e)
        {
            var clickedChar = (Character)e.ClickedItem;
            _navigationService.Navigate("CharacterDetails", clickedChar);
        }
    }
}
