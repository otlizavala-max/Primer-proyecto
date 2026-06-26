using System;
using System.Collections.Generic;
using System.Text;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Primer_proyecto.Models;
using Primer_proyecto.Services;
using Primer_proyecto.Views;
using System.Collections.ObjectModel;

namespace Primer_proyecto.ViewModels;

public partial class PageListPersonasViewModel : BaseViewModel
{
    private readonly DatabaseServices _database;

    public ObservableCollection<Personas> Personas { get; } = new();

    public PageListPersonasViewModel(DatabaseServices database)
    {
        _database = database;
    }

    public async Task CargarPersonas()
    {
        Personas.Clear();

        var lista = await _database.ObtenerListaPersonas();

        foreach (var persona in lista)
            Personas.Add(persona);
    }

    [RelayCommand]
    private async Task Agregar()
    {
        await Application.Current.MainPage.Navigation.PushAsync(new PageAddPersonas());
    }

    [RelayCommand]
    private async Task Editar(Personas persona)
    {
        if (persona == null)
            return;

        await Application.Current.MainPage.Navigation.PushAsync(new PageAddPersonas(persona));
    }

    [RelayCommand]
    private async Task Eliminar(Personas persona)
    {
        if (persona == null)
            return;

        bool confirmar = await Application.Current.MainPage.DisplayAlert(
            "Confirmar",
            $"¿Eliminar a {persona.NombreCompleto}?",
            "Sí",
            "No");

        if (!confirmar)
            return;

        await _database.DeletePersona(persona);

        await CargarPersonas();
    }
}