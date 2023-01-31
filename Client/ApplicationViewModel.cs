using System;
using Flurl.Http;
using System.Windows;
using System.Net.Http;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

using DevPace.DB.Models;

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
                            // Open confirm window
                            if (MessageBox.Show($"Delete customer {customer.Name}?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                            {
                                RemoveCustomer();
                                Customers?.Remove(customer);
                            }
                        }
                    },
                    (obj) => Customers?.Count > 0 && SelectedCustomer != null));
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
                            UpdateCustomer();
                        }
                    },
                    (obj) => Customers?.Count > 0 && SelectedCustomer != null));
            }
        }

        public ApplicationViewModel()
        {
            loader = LoadCustomers;

            loader.Invoke();
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
            if (Customers == null)
            {
                try
                {
                    Customers = await CreateCustumerRequest().GetJsonAsync<ObservableCollection<Customer>>();

                    OnPropertyChanged("Customers");
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        private async void UpdateCustomer()
        {
            try
            {
                await CreateCustumerRequest().PutJsonAsync(selectedCustomer);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async void RemoveCustomer()
        {
            try
            {
                await CreateCustumerRequest().SendJsonAsync(new HttpMethod("DELETE"), selectedCustomer);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private IFlurlRequest CreateCustumerRequest() =>
            "https://localhost:7224/api/Customer".WithHeader("Authorization", "base YWRtaW46cGFzc3dvcmQ=");
    }
}
