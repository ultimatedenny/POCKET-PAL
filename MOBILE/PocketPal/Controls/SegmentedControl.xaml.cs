using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace POCKETPAL.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SegmentedControl : ContentView
	{
        public static readonly BindableProperty PrimaryColorProperty
            = BindableProperty.Create(
                nameof(PrimaryColor),
                typeof(Color),
                typeof(SegmentedControl),
                Color.CornflowerBlue);

        public Color PrimaryColor
        {
            get { return (Color)GetValue(PrimaryColorProperty); }
            set { SetValue(PrimaryColorProperty, value); }
        }

        public static readonly BindableProperty SecondaryColorProperty
            = BindableProperty.Create(
                nameof(SecondaryColor),
                typeof(Color),
                typeof(SegmentedControl),
                Color.White);

        public Color SecondaryColor
        {
            get { return (Color)GetValue(SecondaryColorProperty); }
            set { SetValue(SecondaryColorProperty, value); }
        }

        public static readonly BindableProperty Tab1TextProperty
            = BindableProperty.Create(
                nameof(Tab1Text),
                typeof(string),
                typeof(SegmentedControl),
                "Tab 1");

        public string Tab1Text
        {
            get { return (string)GetValue(Tab1TextProperty); }
            set { SetValue(Tab1TextProperty, value); }
        }

        public static readonly BindableProperty Tab2TextProperty
            = BindableProperty.Create(
                nameof(Tab2Text),
                typeof(string),
                typeof(SegmentedControl),
                "Tab 2");

        public string Tab2Text
        {
            get { return (string)GetValue(Tab2TextProperty); }
            set { SetValue(Tab2TextProperty, value); }
        }

        public static readonly BindableProperty SelectedTabIndexProperty
            = BindableProperty.Create(
                nameof(SelectedTabIndex),
                typeof(int),
                typeof(SegmentedControl),
                0);

        public int SelectedTabIndex
        {
            get { return (int)GetValue(SelectedTabIndexProperty); }
            private set { SetValue(SelectedTabIndexProperty, value); }
        }

        public event EventHandler<SelectedTabIndexEventArgs> SelectedTabIndexChanged;
        public SegmentedControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// load up the customizations and applying
        /// properties when the element has rendered
        /// </summary>
        protected override void OnParentSet()
        {
            base.OnParentSet();
            SelectTab1();
            SelectedTabIndex = 0;
            SendSelectedTabIndexChangedEvent();
        }

        private void Tab1Button_OnClicked(object sender, EventArgs e)
        {
            SelectTab1();
            SelectedTabIndex = 0;
            SendSelectedTabIndexChangedEvent();
        }

        private void Tab2Button_OnClicked(object sender, EventArgs e)
        {
            SelectTab2();
            SelectedTabIndex = 1;
            SendSelectedTabIndexChangedEvent();
        }

        private void SelectTab1()
        {
            Tab1ButtonView.BackgroundColor = SecondaryColor;
            Tab1ButtonView.TextColor = Color.White;
            Tab2ButtonView.TextColor = Color.Black;
            Tab2ButtonView.BackgroundColor = PrimaryColor;
        }

        private void SelectTab2()
        {
            Tab1ButtonView.BackgroundColor = PrimaryColor;
            Tab2ButtonView.TextColor = Color.White;
            Tab1ButtonView.TextColor = Color.Black;
            Tab2ButtonView.BackgroundColor = SecondaryColor;
        }

        /// <summary>
        /// Invoke the SelectedTabIndexChanged event
        /// for whoever has subscribed so they can
        /// use it for any reative action
        /// </summary>
        private void SendSelectedTabIndexChangedEvent()
        {
            var eventArgs = new SelectedTabIndexEventArgs();
            eventArgs.SelectedTabIndex = SelectedTabIndex;

            SelectedTabIndexChanged?.Invoke(this, eventArgs);
        }
        public class SelectedTabIndexEventArgs : EventArgs
        {
            public int SelectedTabIndex { get; set; }
        }
    }
}