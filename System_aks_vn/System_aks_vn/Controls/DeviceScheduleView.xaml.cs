using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System_aks_vn.Models;
using System_aks_vn.ViewModels.Devices.Settings;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace System_aks_vn.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeviceScheduleView : ContentView
    {
        private static readonly string colorBoxEnable = "#1fb141";
        //public static List<string> _tmpSource = null;

        public static readonly BindableProperty ItemSourceProperty = BindableProperty.Create(
            "ItemSource",        // the name of the bindable property
            typeof(IList),     // the bindable property type
            typeof(DeviceScheduleView),   // the parent object type
            null,
            BindingMode.TwoWay,
            propertyChanged: OnSourceChanged);      // the default value for the property

        public IList ItemSource
        {
            get => (IList)GetValue(DeviceScheduleView.ItemSourceProperty);
            set => SetValue(DeviceScheduleView.ItemSourceProperty, value);
        }

        private static void OnSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((DeviceScheduleView)bindable).ItemSource = (IList)newValue;
        }

        public static List<MyBoxView> MyBoxViews { get; set; }

        public DeviceScheduleView()
        {
            InitializeComponent();
            Init();
            DrawFrameView();
        }

        void Init()
        {
            //BindingContext = new DeviceSettingScheduleViewModel()
            MyBoxViews = new List<MyBoxView>();
            if (ItemSource == null)
            {
                ItemSource = new ObservableCollection<string>();

                for (int i = 0; i < 48; i++)
                {
                    ItemSource.Add("-1");
                }
            }
        }

        public static void UpdateViewWhenChangeDay(List<string> sources)
        {
            int row = 0;
            foreach (var code in sources)
            {
                if (code == "-1")
                {
                    var lboxview = MyBoxViews.FindAll(x => x.X == row);
                    foreach (var bv in lboxview)
                    {
                        bv.BackgroundColor = GetColorBoxView();
                    }
                }
                else
                {
                    var lboxview = MyBoxViews.FindAll(x => x.X == row);
                    foreach (var box in lboxview)
                    {
                        if (box.Y == int.Parse(code) + 1)
                            box.BackgroundColor = ConvertHexToColor(colorBoxEnable);
                        else
                            box.BackgroundColor = GetColorBoxView();
                    }
                }
                row++;
            }
        }

        void DrawFrameView()
        {
            var scrollview = new ScrollView { Content = DrawListBoxView() };
            var stackContent = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children = { DrawTitleView(), scrollview }
            };

            gContent.Children.Add(stackContent);
        }

        Grid DrawTitleView()
        {
            var gTitleView = new Grid
            {
                RowDefinitions = new RowDefinitionCollection(),
                ColumnDefinitions = new ColumnDefinitionCollection()
            };

            gTitleView.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });

            for (int i = 0; i < 4; i++)
            {
                gTitleView.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });
            }

            //title
            var ltitle = new List<string> { "Timeline", "DISARM", "ARM0", "ARM1" };
            for (int j = 0; j < 4; j++)
            {
                gTitleView.Children.Add(new Label
                {
                    BackgroundColor = ConvertHexToColor("#FFA700"),
                    TextColor = Color.White,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    Text = ltitle[j],

                }, j, 0);
            }

            return gTitleView;
        }

        Grid DrawListBoxView()
        {
            var gBoxView = new Grid
            {
                RowDefinitions = new RowDefinitionCollection(),
                ColumnDefinitions = new ColumnDefinitionCollection()
            };
            for (int i = 0; i < 48; i++)
            {
                gBoxView.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });
            }

            for (int i = 0; i < 4; i++)
            {
                gBoxView.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });
            }

            //time 
            bool half = false;
            int hour = 0;
            for (int i = 0; i < 48; i++)
            {
                var m = half == false ? "00" : "30";
                var h = hour < 10 ? "0" + hour : hour.ToString();

                gBoxView.Children.Add(new Label
                {
                    TextColor = GetColorText(),
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Text = $"{h}:{m}",
                    FontSize = GetSizeText(),
                }, 0, i);

                if (half)
                    hour++;
                half = !half;
            }

            MyBoxViews?.Clear();
            //list myboxview
            for (int i = 0; i < 48; i++)
            {
                for (int j = 1; j < 4; j++)
                {
                    var tap = new TapGestureRecognizer();
                    var box = new MyBoxView
                    {
                        BackgroundColor = GetColorBoxView(),
                        Margin = 0,
                        X = i,
                        Y = j,
                    };
                    tap.Tapped += (sender, e) =>
                    {
                        for (int z = box.X; z < 48; z++)
                        {
                            ItemSource[z] = (box.Y - 1).ToString();
                            MyBoxViews.Find(x => x.X == z && x.Y == box.Y)
                                .BackgroundColor = ConvertHexToColor(colorBoxEnable);

                            for (int k = 1; k < 4; k++)
                            {
                                if (k != box.Y)
                                    MyBoxViews.Find(x => x.X == z && x.Y == k)
                                        .BackgroundColor = GetColorBoxView();
                            }
                        }

                    };
                    box.GestureRecognizers.Add(tap);

                    if (ItemSource[i].ToString() == (j - 1).ToString())
                    {
                        box.BackgroundColor = ConvertHexToColor(colorBoxEnable);
                    }

                    MyBoxViews.Add(box);
                    gBoxView.Children.Add(box, j, i);
                }
            }
            return gBoxView;
        }

        Color GetColorText()
        {
            OSAppTheme currentTheme = Application.Current.RequestedTheme;
            return currentTheme == OSAppTheme.Dark ? Color.White : Color.Black;
        }

        static Color GetColorBoxView()
        {
            OSAppTheme currentTheme = Application.Current.RequestedTheme;
            return currentTheme == OSAppTheme.Dark ? ConvertHexToColor("#4a4b4d") : ConvertHexToColor("#97989b");
        }

        static Color ConvertHexToColor(string hexColor)
        {
            return Xamarin.Forms.Color.FromHex(hexColor);
        }

        int GetSizeText()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    return 14;
                case Device.Android:
                    return 16;
                default: return 16;
            }
        }
    }
}