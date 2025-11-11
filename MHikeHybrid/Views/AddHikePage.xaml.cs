
using MHikeHybrid.ViewModels;

namespace MHikeHybrid.Views;

public partial class AddHikePage : ContentPage
{
   
    public AddHikePage(AddHikeViewModel viewModel)
    {
        InitializeComponent();

       
        BindingContext = viewModel;
    }
}