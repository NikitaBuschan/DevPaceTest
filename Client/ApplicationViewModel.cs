using DevPace.DB.Models;
using Flurl.Http;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Client
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        public delegate void LoadDataDelegate();
        public LoadDataDelegate loader;

        private Customer? selectedCustomer = null;
        public Customer? SelectedCustomer
        {
            get { return selectedCustomer; }
            set
            {
                selectedCustomer = value;
                OnPropertyChanged("SelectedCustomer");
            }
        }

        public ObservableCollection<Customer>? Customers { get; set; } = null;

        private RelayCommand? addCommand = null;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                    (addCommand = new RelayCommand(obj =>
                    {
                        Customer customer = new Customer();
                        Customers?.Insert(0, customer);
                        SelectedCustomer = customer;
                    }));
            }
        }

        private RelayCommand? removeCommand = null;
        public RelayCommand RemoveCommand
        {
            get
            {
                return removeCommand ??
                    (removeCommand = new RelayCommand(obj =>
                    {
                        Customer customer = obj as Customer;
                        if (customer != null)
                        {
                            Customers?.Remove(customer);
                        }
                    },
                    (obj) => Customers?.Count > 0));
            }
        }

        private RelayCommand? editCustomer = null;
        public RelayCommand EditCustomerCommand
        {
            get
            {
                return editCustomer ??
                    (editCustomer = new RelayCommand(obj =>
                    {
                        Customer customer = obj as Customer;

                        CustomerEdit editWindow = new CustomerEdit(SelectedCustomer);

                        if (editWindow.ShowDialog() == true)
                        {

                        }
                    },
                    (obj) => Customers?.Count > 0 && SelectedCustomer != null));
            }
        }

        public ApplicationViewModel()
        {
            Task.Run(() => LoadCustomers()).ConfigureAwait(false).GetAwaiter();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        private async void LoadCustomers()
        {
            try
            {
                if (Customers == null)
                {
                    Customers = await "https://localhost:7224/api/Customer"
                        .WithHeader("Authorization", "base YWRtaW46cGFzc3dvcmQ=")
                        .GetJsonAsync<ObservableCollection<Customer>>();

                    OnPropertyChanged("Customers");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
