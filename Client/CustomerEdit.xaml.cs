using System.Windows;

using DevPace.DB.Models;

namespace Client
{
    public partial class CustomerEdit : Window
    {
        public Customer Customer { get; set; }

        public CustomerEdit(Customer customer)
        {
            InitializeComponent();
            Customer = customer;
            DataContext = Customer;
        }

        void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
