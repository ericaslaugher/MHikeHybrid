using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MHikeHybrid.Models;
using MHikeHybrid.Services;
using MHikeHybrid.Views;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MHikeHybrid.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly DatabaseService _dbService;

        [ObservableProperty]
        private ObservableCollection<Hike> hikes;

        public MainViewModel(DatabaseService dbService)
        {
            _dbService = dbService;
            hikes = new ObservableCollection<Hike>();
        }


        [RelayCommand]
        private async Task LoadHikes()
        {
            var hikeList = await _dbService.GetHikesAsync();
            Hikes.Clear();
            foreach (var hike in hikeList)
            {
                Hikes.Add(hike);
            }
        }

        [RelayCommand]
        private async Task GoToAddHike()
        {

            await Shell.Current.GoToAsync(nameof(AddHikePage));
        }



        [RelayCommand]
        private async Task DeleteHike(Hike hike)
        {
            if (hike == null) return;


            bool confirmed = await Shell.Current.DisplayAlert("Confirm Delete", $"Are you sure you want to delete '{hike.Name}'?", "Delete", "Cancel");

            if (confirmed)
            {
                await _dbService.DeleteHikeAsync(hike);

                Hikes.Remove(hike);
            }
        }

        [RelayCommand]
        private async Task GoToEditHike(Hike hike)
        {
            if (hike == null) return;


            await Shell.Current.GoToAsync($"{nameof(AddHikePage)}?hikeId={hike.Id}");
        }

        [RelayCommand]
        private async Task DeleteAllHikes()
        {

            bool confirmed = await Shell.Current.DisplayAlert("Confirm Delete", "Are you sure you want to delete ALL hikes? This action cannot be undone.", "Delete All", "Cancel");

            if (confirmed)
            {
                await _dbService.DeleteAllHikesAsync();

                Hikes.Clear();
            }
        }

    }
}