using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LabSheet_6
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Entities db = new Entities();
		public MainWindow()
		{
			InitializeComponent();
		}
		//Ex1
		private void Ex1Btn_Click(object sender, RoutedEventArgs e)
		{
			var query = from c in db.Categories
						join p in db.Products on c.CategoryName equals p.Category.CategoryName
						orderby c.CategoryName
						select new { Category = c.CategoryName, Product = p.ProductName };

			var results = query.ToList();

			exlbxdisplay.ItemsSource = results;
			ex1TblkCount.Text = results.Count.ToString();
		}
		//Ex2
		private void Ex2Btn_Click(object sender, RoutedEventArgs e)
		{
			var query = from p in db.Products
						orderby p.Category.CategoryName, p.ProductName
						select new { Category = p.Category.CategoryName, Product = p.ProductName };

			var results = query.ToList();

			exlbxdisplay1.ItemsSource = results;

			ex2TblkCount.Text = results.Count.ToString();
		}
		//Ex3
		private void Ex3Btn_Click(object sender, RoutedEventArgs e)
		{//return the total number of orders of orders for product 7
			var query1 = (from detail in db.Order_Details
						  where detail.ProductID == 7
						  select detail);
			//return the total value of orders for product 7
			var query2 = (from detail in db.Order_Details
						  where detail.ProductID == 7
						  select detail.UnitPrice * detail.Quantity);

			int numberofOrders = query1.Count();
			decimal totalValue = query2.Sum();
			decimal averageValue = query2.Average();

			ex3lbsdisplay.Text = string.Format(
				"Total number of orders {0}\nValue of Orders {1:C}\nAverage Order Value {2:C}", numberofOrders, totalValue, averageValue);
			

		}
		//Ex4
		private void Ex4Btn4_Click(object sender, RoutedEventArgs e)
		{
			var query = from customer in db.Customers
						where customer.Orders.Count >= 20
						select new
						{
							Name = customer.CompanyName,
							OrderCount = customer.Orders.Count
						};
			exl4bxdisplay.ItemsSource = query.ToList();
		}
		//Ex5
		private void Ex5Btn_Click(object sender, RoutedEventArgs e)
		{
			var query = from Customer in db.Customers
						where Customer.Orders.Count < 3
						select new
						{
							Company = Customer.CompanyName,
							City = Customer.City,
							Region = Customer.Region,
							OrderCount = Customer.Orders.Count
						};
			ex5lbxdisplay.ItemsSource = query.ToList();
		}
		//Ex6
		private void Ex6Btn_Click(object sender, RoutedEventArgs e)
		{
			var query = from Customer in db.Customers
						orderby Customer.CompanyName
						select Customer.CompanyName;
			ex6lbxdisplay.ItemsSource = query.ToList();
		}
		//Ex6 part
		private void ex6lbxdisplay_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			string company = (string)ex6lbxdisplay.SelectedItem;
			if(company != null)
			{
				var query = (from detail in db.Order_Details
							 where detail.Order.Customer.CompanyName == company
							 select detail.UnitPrice * detail.Quantity).Sum();

				ex6TblkCount.Text = string.Format("Total for Supplier {0}\n\n{1:C}", company, query);
			}
		}
		//Ex7
		private void Ex7Btn_Click(object sender, RoutedEventArgs e)
		{
			var query = from p in db.Products
						group p by p.Category.CategoryName into g
						orderby g.Count() descending
						select new
						{
							Category = g.Key,
							Count = g.Count()
						};
			ex7lbxdisplay.ItemsSource = query.ToList();
		}
	}
}
