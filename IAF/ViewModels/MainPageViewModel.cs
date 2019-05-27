using IAF.Models;
using IAF.Services;
using IAF.Views;
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
    public class MainPageViewModel : ViewModelBase
    {
        public ObservableCollection<Book> BookCollection { get; set; } = new ObservableCollection<Book>();
        private Book clickedBook;
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
        public Book ClickedBook
        {
            get
            {
                return clickedBook;
            }
            set
            {
                SetProperty(ref clickedBook, value); 
            }
        }

        public INavigationService _navigationService;
        public MainPageViewModel(INavigationService navigationService)
        {             
            _navigationService = navigationService;
        }
        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            var service = new IAFService();
            var books = await service.GetBooksAsync();
            foreach (var item in books)
            {
                BookCollection.Add(item);
            }
            ClickedBook = books.First();
            ReleaseDate = books.First().Released.Remove(books.First().Released.Length - 9);


        }
        public void BookCollection_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var book = (Book)e.ClickedItem;
            ReleaseDate = book.Released.Remove(book.Released.Length-9);
            ClickedBook = book;
        }
        public void BookDetails_OnClick(object sender, RoutedEventArgs e)
        {
            _navigationService.Navigate("BookDetails", ClickedBook);
        }
    }
}
