using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace POCKETPAL.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InitialsViewControl : ContentView
    {
        public static readonly BindableProperty DefaultBackgroundColorProperty = BindableProperty.Create(nameof(DefaultBackgroundColor), typeof(Color), typeof(InitialsViewControl), Color.LightGray);

        public Color DefaultBackgroundColor
        {
            get => (Color)GetValue(DefaultBackgroundColorProperty);
            set => SetValue(DefaultBackgroundColorProperty, value);
        }

        public static readonly BindableProperty TextColorLightProperty = BindableProperty.Create(nameof(TextColorLight), typeof(Color), typeof(InitialsViewControl), Color.White);

        public Color TextColorLight
        {
            get => (Color)GetValue(TextColorLightProperty);
            set => SetValue(TextColorLightProperty, value);
        }

        public static readonly BindableProperty TextColorDarkProperty
            = BindableProperty.Create(nameof(TextColorDark), typeof(Color), typeof(InitialsViewControl), Color.Black);
        public Color TextColorDark
        {
            get => (Color)GetValue(TextColorDarkProperty);
            set => SetValue(TextColorDarkProperty, value);
        }

        public static readonly BindableProperty NameProperty = BindableProperty.Create(nameof(Name), typeof(string), typeof(InitialsViewControl), string.Empty, propertyChanged: OnNamePropertyChanged);

        public string Name
        {
            get => (string)GetValue(NameProperty);
            set => SetValue(NameProperty, value);
        }

        private static void OnNamePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is InitialsViewControl initialsView))
                return;

            initialsView.SetName((string)newValue);
        }

        private void SetName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                ContentLabel.Text = string.Empty;
                ContentBoxView.BackgroundColor = DefaultBackgroundColor;
                return;
            }
            var words = name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length == 1)
            {
                ContentLabel.Text = words[0][0].ToString();
            }
            else if (words.Length > 1)
            {
                var initialsString = words[0][0].ToString() + words[words.Length - 1][0].ToString();
                ContentLabel.Text = initialsString;
            }
            else
            {
                ContentLabel.Text = string.Empty;
            }
            SetColors(name);
        }

        public InitialsViewControl()
        {
            InitializeComponent();
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();
            if (WidthRequest == -1 || HeightRequest == -1)
            {
                InitControl(50);
            }
            else
            {
                InitControl(Math.Min(WidthRequest, HeightRequest));
            }
        }

        private void SetColors(string name)
        {
            var hexColor = "#FF" + Convert.ToString(name.GetHashCode(), 16).Substring(0, 6);
            if (hexColor.Length == 8)
                hexColor += "5";
            var color = Color.FromHex(hexColor);
            ContentBoxView.BackgroundColor = color;
            var brightness = color.R * .3 + color.G * .59 + color.B * .11;
            ContentLabel.TextColor = brightness < 0.5 ? TextColorLight : TextColorDark;
        }

        private void InitControl(double size)
        {
            double size2 = size / 1.2;
            ContentBoxView.HeightRequest = size2;
            ContentBoxView.WidthRequest = size2;
            ContentBoxView.CornerRadius = size2 / 2;
            FrameView.CornerRadius = (Convert.ToInt32(size2) / 2) + 10;
            ContentBoxView.BackgroundColor = DefaultBackgroundColor;
            ContentLabel.FontSize = (size2 / 2) - 5;
            if (!string.IsNullOrEmpty(Name))
                SetName(Name);
        }
    }
}