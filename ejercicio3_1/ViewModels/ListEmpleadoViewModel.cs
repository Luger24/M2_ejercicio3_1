using System;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using ejercicio3_1.Models;

using Xamarin.Forms;
using System.Collections.Generic;

namespace ejercicio3_1.ViewModels
{
    public class ListEmpleadoViewModel : BaseViewModel
    {

        private ObservableCollection<Empleado> _empleados;

        public ObservableCollection<Empleado> Empleados
        {
            get { return _empleados; }
            set { _empleados = value; OnPropertyChanged(); }
        }

        private Empleado _selectedEmpleado;

        public Empleado SelectedEmpleado
        {
            get { return _selectedEmpleado; }
            set { _selectedEmpleado = value; OnPropertyChanged(); }
        }

        public ICommand GoToDetailsCommand { private set; get; }
        public ICommand CrearEmpleadoCommand { private set; get; }

        public INavigation Navigation { get; set; }

        public ListEmpleadoViewModel(INavigation navigation)
        {
            Navigation = navigation;
            GoToDetailsCommand = new Command<Type>(async (pageType) => await GoToDetails(pageType));
            CrearEmpleadoCommand = new Command<Type>(async (pageType) => await CrearEmpleado(pageType));



            /*Empleados.Add(new Empleado() { Nombre = "Empleado 1", Apellidos = "Apellidos", Edad = 30, Direccion = "Dir. Emple 1", Puesto = "Ingeniero", Foto = null });
            Empleados.Add(new Empleado() { Nombre = "Empleado 2", Apellidos = "Apellidos", Edad = 30, Direccion = "Dir. Emple 2", Puesto = "Ingeniero", Foto = null });
            Empleados.Add(new Empleado() { Nombre = "Empleado 3", Apellidos = "Apellidos", Edad = 30, Direccion = "Dir. Emple 3", Puesto = "Ingeniero", Foto = null });*/


        }

        public async void setearEmpleadosSQLite()
        {
            Empleados = new ObservableCollection<Empleado>();

            List<Empleado> ListEmpleados = new List<Empleado>();
            ListEmpleados = await App.DBase.obtenerListaEmpleado();

            if(ListEmpleados != null)
            {
                ListEmpleados.Reverse();

                for (int i = 0; i < ListEmpleados.Count; i++)
                {
                    Empleados.Add(ListEmpleados[i]);
                }
            }
        }

        async Task GoToDetails(Type pageType)
        {
            if (SelectedEmpleado != null)
            {
                var page = (Page)Activator.CreateInstance(pageType);

                page.BindingContext = new EmpleadoViewModel()
                {
                    Id = SelectedEmpleado.Id,
                    Nombre = SelectedEmpleado.Nombre,
                    Apellidos = SelectedEmpleado.Apellidos,
                    Edad = SelectedEmpleado.Edad,
                    Direccion = SelectedEmpleado.Direccion,
                    Puesto = SelectedEmpleado.Puesto,
                    Foto = SelectedEmpleado.Foto,

                    btncrearIsVisible = false,
                    btnactualizarIsVisible = true,
                    btneliminarIsVisible = true,

                    Navigation = Navigation
                };

                await Navigation.PushAsync(page);
            }
        }

        async Task CrearEmpleado(Type pageType)
        {
            var page = (Page)Activator.CreateInstance(pageType);

            page.BindingContext = new EmpleadoViewModel()
            {
                btncrearIsVisible = true,
                btnactualizarIsVisible = false,
                btneliminarIsVisible = false,

                Navigation = Navigation
            };

            await Navigation.PushAsync(page);
        }

    }
}
