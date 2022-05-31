using POCKETPAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace POCKETPAL.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SSITSummaryScan : ContentPage
	{
		string SSITUserGroup, SSITApprCat, SSITUserID;
		public SSITSummaryScan (string menuSSIT)
		{
			InitializeComponent ();
			this.BindingContext = new SSITViewModel(menuSSIT,"SUMMARY");
			SSITUserGroup = GetValue("SSITUserGroup");
			SSITApprCat = GetValue("SSITApprCat");
			SSITUserID = GetValue("SSITUserID");

			if (menuSSIT.ToUpper() == "SCRAP" || (menuSSIT.ToUpper() == "PURCHASE RETURN" && Convert.ToInt32(SSITApprCat) > 1))
			{
				PickerPlant.IsVisible = false;
				PickerVendor.IsVisible = false;
				PickerAction.IsVisible = false;
				EntryVendor.IsVisible = false;

				PickerPlant_view.IsVisible = false;
				PickerVendor_view.IsVisible = false;
				PickerAction_view.IsVisible = false;
				EntryVendor_view.IsVisible = false;
			}
		}
		public void AddValue(string key, string value)
		{
			Preferences.Set(key, value);
		}
		public string GetValue(string key)
		{
			return Preferences.Get(key, "");
		}

		private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
		{
			var vm = BindingContext as SSITViewModel;
			vm.SearchCommand.Execute(null);
		}

		private void searchResults_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			var vendor = e.Item as string;
			var vm = BindingContext as SSITViewModel;
			vm.AddComand.Execute(vendor);
		}
		private void Chacked_All(object sender, CheckedChangedEventArgs e)
		{
			SSITViewModel SVM = BindingContext as SSITViewModel;
			if (CheckAll.IsChecked == true)
			{
				SVM.CheckAll();
			}
			else
			{
				SVM.UnCheckAll();
			}
		}
	}
}