using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MHikeHybrid.Models;
using MHikeHybrid.Services;
using System.Threading.Tasks;

namespace MHikeHybrid.ViewModels
{
    public partial class AddHikeViewModel : ObservableObject, IQueryAttributable
    {
        private readonly DatabaseService _dbService;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsEditMode))]
        int hikeId;

        [ObservableProperty]
        string pageTitle = "Add New Hike";

        [ObservableProperty]
        string saveButtonText = "Save Hike";

        public bool IsEditMode => HikeId != 0;

        [ObservableProperty]
        string name;

        [ObservableProperty]
        string location;

        [ObservableProperty]
        DateTime hikeDate = DateTime.Today;

        [ObservableProperty]
        bool parkingAvailable;

        [ObservableProperty]
        string length;

        [ObservableProperty]
        string difficulty;

        [ObservableProperty]
        string description;

        [ObservableProperty]
        string transport;

        [ObservableProperty]
        string estTime;


        public List<string> DifficultyOptions { get; } = new List<string> { "Easy", "Medium", "Hard", "Very Hard" };


        public AddHikeViewModel(DatabaseService dbService)
        {
            _dbService = dbService;
        }


        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {

            if (query.ContainsKey("hikeId"))
            {

                HikeId = int.Parse(query["hikeId"].ToString());

                Task.Run(async () => await LoadHikeData(HikeId));
            }
        }

        private async Task LoadHikeData(int id)
        {
            if (id == 0) return;

            var hike = await _dbService.GetHikeAsync(id);
            if (hike != null)
            {

                MainThread.BeginInvokeOnMainThread(() =>
                {

                    Name = hike.Name;
                    Location = hike.Location;

                   
                    HikeDate = DateTime.ParseExact(hike.HikeDate, "dd/MM/yyyy", null);
                    ParkingAvailable = hike.ParkingAvailable;
                    Length = hike.Length;
                    Difficulty = hike.Difficulty; 
                    Description = hike.Description;
                    Transport = hike.Transport;
                    EstTime = hike.EstTime;


                    PageTitle = "Edit Hike";
                    SaveButtonText = "Update";
                });
            }
        }


        [RelayCommand]
        private async Task SaveHike()
        {

            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Location) || string.IsNullOrWhiteSpace(Length))
            {
                await Shell.Current.DisplayAlert("Error", "Name, Location, and Length are required", "OK");
                return;
            }


            var hike = new Hike
            {
                Id = this.HikeId,
                Name = this.Name,
                Location = this.Location,
                HikeDate = this.HikeDate.ToString("dd/MM/yyyy"),
                ParkingAvailable = this.ParkingAvailable,
                Length = this.Length,
                Difficulty = this.Difficulty,
                Description = this.Description,
                Transport = this.Transport,
                EstTime = this.EstTime
            };


            await _dbService.SaveHikeAsync(hike);

           
            string message = IsEditMode ? "Hike updated successfully!" : "Hike saved successfully!";
            await Shell.Current.DisplayAlert("Success", message, "OK");

         
            await Shell.Current.GoToAsync("..");
        }
    }
}