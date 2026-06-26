using Primer_proyecto.Models;
using Primer_proyecto.Services;
using Primer_proyecto.ViewModels;

namespace Primer_proyecto.Views;

public partial class PageAddPersonas : ContentPage
{
    public PageAddPersonas(Personas persona = null)
    {
        InitializeComponent();

        var vm = new PageAddPersonasViewModel(new DatabaseServices());

        BindingContext = vm;

        if (persona != null)
            vm.CargarPersona(persona);
    }
}