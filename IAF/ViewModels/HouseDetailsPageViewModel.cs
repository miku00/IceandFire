using IAF.Models;
using IAF.Services;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace IAF.ViewModels
{
    public class HouseDetailsPageViewModel : ViewModelBase
    {
        public House DetailedHouse { get; set; }
        public Character CurrentLord { get; set; } = new Character();
        public ObservableCollection<Character> GOTSwornHouseMembers { get; set; } = new ObservableCollection<Character>();
        public ObservableCollection<House> Cadets { get; set; } = new ObservableCollection<House>();

        INavigationService _navigationService;
        public HouseDetailsPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            DetailedHouse = (House)e.Parameter;
            var service = new IAFService();
            if (DetailedHouse.CurrentLord != "")
            {
                CurrentLord = await service.GetCharacterAsync(DetailedHouse.CurrentLord);
            }
            else { CurrentLord.Name = ""; }

            if(DetailedHouse.SwornMembers.Count <= 10)
            {
                foreach(var url in DetailedHouse.SwornMembers)
                {
                    var q = await service.GetCharacterAsync(url);
                    GOTSwornHouseMembers.Add(q);
                }
            }
            else
            {
                for(int i=0; i<10; i++)
                {
                    var q = await service.GetCharacterAsync(DetailedHouse.SwornMembers[i]);
                    GOTSwornHouseMembers.Add(q);
                }
            }
            foreach (var item in DetailedHouse.CadetBranches)
            {
                var q = await service.GetHouseAsync(item);
                Cadets.Add(q);
            }
        }
        public void CadetClick(object sender, ItemClickEventArgs e)
        {
            if (Cadets.Count > 0)
            {
                _navigationService.Navigate("HouseDetails", (House)e.ClickedItem);
            }
        }

        public void CurrentLord_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentLord.Name != "")
            {
                _navigationService.Navigate("CharacterDetails", CurrentLord);
            }
        }

        public void SwornMemebersListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            _navigationService.Navigate("CharacterDetails", (Character)e.ClickedItem);
        }

    }
}
