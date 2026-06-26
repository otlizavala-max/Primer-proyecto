using Primer_proyecto.ViewModels;

namespace Primer_proyecto.Views;

public partial class PageListPersonas : ContentPage
{
    private readonly PageListPersonasViewModel _viewModel;

    public PageListPersonas()
    {
        InitializeComponent();

        _viewModel = new PageListPersonasViewModel(new Services.DatabaseServices());

        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.CargarPersonas();
    }
}