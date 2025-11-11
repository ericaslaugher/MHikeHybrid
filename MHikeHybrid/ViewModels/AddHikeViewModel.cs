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
        string pageTitle = "Thêm chuyến đi mới";

        [ObservableProperty]
        string saveButtonText = "Lưu chuyến đi";

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


        public List<string> DifficultyOptions { get; } = new List<string> { "Dễ", "Trung bình", "Khó", "Rất khó" };


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

                  
                    PageTitle = "Sửa chuyến đi";
                    SaveButtonText = "Cập nhật";
                });
            }
        }


        [RelayCommand]
        private async Task SaveHike()
        {

            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Location) || string.IsNullOrWhiteSpace(Length))
            {
                await Shell.Current.DisplayAlert("Lỗi", "Tên, Địa điểm, và Chiều dài là bắt buộc", "OK");
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

            // Hiển thị thông báo thành công
            string message = IsEditMode ? "Đã cập nhật chuyến đi!" : "Đã lưu chuyến đi!";
            await Shell.Current.DisplayAlert("Thành công", message, "OK");

            // Tự động quay về trang danh sách
            await Shell.Current.GoToAsync("..");
        }
    }
}