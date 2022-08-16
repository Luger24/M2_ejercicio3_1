using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ejercicio3_1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListEmpleadoView : ContentPage
    {
        ViewModels.ListEmpleadoViewModel vmListEmpleado;

        public ListEmpleadoView()
        {
            InitializeComponent();

            vmListEmpleado = new ViewModels.ListEmpleadoViewModel(Navigation);
            BindingContext = vmListEmpleado;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            vmListEmpleado.setearEmpleadosSQLite();
        }
    }
}