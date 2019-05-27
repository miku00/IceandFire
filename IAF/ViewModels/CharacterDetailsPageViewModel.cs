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
    public class CharacterDetailsPageViewModel : ViewModelBase
    {
        public INavigationService _navigationService;
        public Character DetailedCharacter { get; set; }
        public ObservableCollection<House> DetailedCharacterHouses { get; set; } = new ObservableCollection<House>();
        public Character DetailedCharacterMother { get; set; } = new Character();
        public Character DetailedCharacterFather { get; set; } = new Character();
        public Character DetailedCharacterSpouse { get; set; } = new Character();
        private House _clickedHouse;
        public House ClickedHouse
        {
            get
            {
                return _clickedHouse;
            }
            set
            {
                SetProperty(ref _clickedHouse, value);
            }
        }

        public CharacterDetailsPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }


        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            DetailedCharacter = (Character)e.Parameter;
            var service = new IAFService();
            if (DetailedCharacter.Mother != "")
            {
                DetailedCharacterMother = await service.GetCharacterAsync(DetailedCharacter.Mother);
            }
            else
            {
                DetailedCharacterMother.Name = "";
            }
            if (DetailedCharacter.Father != "")
            {
                DetailedCharacterFather = await service.GetCharacterAsync(DetailedCharacter.Father);
            }
            else
            {
                DetailedCharacterFather.Name = "";
            }
            if (DetailedCharacter.Spouse != "")
            {
                DetailedCharacterSpouse = await service.GetCharacterAsync(DetailedCharacter.Spouse);
            }
            else
            {
                DetailedCharacterSpouse.Name = "";
            }

            foreach (var houseUrl in DetailedCharacter.Allegiances)
            {
                var house = await service.GetHouseAsync(houseUrl);
                DetailedCharacterHouses.Add(house);
            }
            
        }

        public void FatherButton_Click(object sender, RoutedEventArgs e)
        {
            if(DetailedCharacterFather.Name != "")
            {
                _navigationService.Navigate("CharacterDetails", DetailedCharacterFather);
            }
        }

        public void MotherButton_Click(object sender, RoutedEventArgs e)
        {
            if (DetailedCharacterMother.Name != "")
            {
                _navigationService.Navigate("CharacterDetails", DetailedCharacterMother);
            }
        }
        public void SpouseButton_Click(object sender, RoutedEventArgs e)
        {
            if (DetailedCharacterSpouse.Name != "")
            {
                _navigationService.Navigate("CharacterDetails", DetailedCharacterSpouse);
            }
        }
        public void HouseDetails_OnClick(object sender, RoutedEventArgs e)
        {
            _navigationService.Navigate("HouseDetails", ClickedHouse);
        }
        public void HouseItem_Clicked(object sender, ItemClickEventArgs e)
        {
            ClickedHouse = (House)e.ClickedItem;
        }
    }
}
