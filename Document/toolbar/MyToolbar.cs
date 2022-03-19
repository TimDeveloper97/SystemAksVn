using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinApp.Views
{
    public class MenuInfo
    {
        public string IconName { get; set; }
        public string Text { get; set; }
        public string Command { get; set; }
        public Action Callback { get; set; }
        public IMyToolItem Button { get; set; }
    }
    public interface IMyToolItem
    {
        void SetColor(Color color);
        event EventHandler Clicked;
    }
    public class MyToolbarEventArgs : EventArgs
    {
        public string Command { get; set; }
    }

    public class MyToolbar : Grid
    {
        class ImgTool : ImageButton, IMyToolItem
        {
            public void SetColor(Color color)
            {
                ((TintColor)Effects[0]).Foreground = color;
            }
            public ImgTool(string source)
            {
                BackgroundColor = Color.Transparent;
                HorizontalOptions = LayoutOptions.Start;

                Source = source;
            }
        }
        class TextTool : Button, IMyToolItem
        {
            public void SetColor(Color color)
            {
                this.TextColor = color;
            }
            public TextTool(string text)
            {
                BackgroundColor = Color.Transparent;
                HorizontalOptions = LayoutOptions.Start;
                Text = text;
            }
        }

        ImgTool _back;
        Label _title;
        StackLayout _right;
        List<MenuInfo> _items;


        public static readonly BindableProperty TitleColorProperty =
            BindableProperty.Create(nameof(TitleColorProperty), typeof(Color), typeof(MyEntry), Color.White);
        public Color TitleColor
        {
            get { return (Color)GetValue(TitleColorProperty); }
            set { SetValue(TitleColorProperty, value); }
        }

        public static readonly BindableProperty ForegroundProperty =
            BindableProperty.Create(nameof(ForegroundProperty), typeof(Color), typeof(MyEntry), Color.White);
        public Color Foreground
        {
            get { return (Color)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        public bool CanBack { get; set; }
        public event EventHandler Back;
        public string Title
        {
            get => _title?.Text;
            set
            {
                if (_title == null)
                {
                    _title = new Label
                    {
                        VerticalOptions = LayoutOptions.Center,
                    };
                    _title.SetBinding(Label.TextColorProperty, "TitleColor");
                    _title.BindingContext = this;
                }
                _title.Text = value;
            }
        }
        public IList<MenuInfo> ToolbarItems
        {
            get
            {
                if (_items == null)
                {
                    _items = new List<MenuInfo>();
                }
                return _items;
            }
        }
        public event EventHandler<MyToolbarEventArgs> ExecuteCommand;
        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            double imgHeight = this.HeightRequest / 2;

            if (_title?.Parent == null)
            {
                Children.Add(_title);
            }

            if (CanBack)
            {
                if (_back == null)
                {
                    _back = new ImgTool("back.png");
                    _back.Clicked += (s, e) => Back?.Invoke(this, EventArgs.Empty);
                }
                if (_back.Parent == null)
                {
                    Children.Insert(0, _back);
                }
                _back.HeightRequest = _back.WidthRequest = imgHeight;
                _back.SetColor(Foreground);
            }
            else if (_back.Parent == this)
            {
                Children.Remove(_back);
            }

            if (_title != null)
            {
                _title.Margin = new Thickness(CanBack ? HeightRequest : 10, 0, 10, 0);
                _title.FontSize = imgHeight;
            }

            if (_items?.Count > 0)
            {
                if (_right == null)
                {
                    _right = new StackLayout
                    {
                        HorizontalOptions = LayoutOptions.End,
                        Orientation = StackOrientation.Horizontal,
                    };
                    this.Children.Add(_right);
                }
                foreach (var info in ToolbarItems)
                {
                    View btn = null;
                    if (info.IconName != null)
                    {
                        btn = new ImgTool(info.IconName);
                        btn.HeightRequest = imgHeight;
                    }
                    else
                    {
                        btn = new TextTool(info.Text);
                    }

                    var item = (IMyToolItem)btn;

                    item.SetColor(Foreground);
                    info.Button = item;
                    
                    if (info.Callback != null)
                    {
                        item.Clicked += (s, e) => info.Callback.Invoke();
                    }
                    if (info.Command != null)
                    {
                        item.Clicked += (s, e) => ExecuteCommand?.Invoke(this, new MyToolbarEventArgs { 
                            Command = info.Command
                        });
                    }

                    _right.Children.Add(btn);
                }
            }

            return base.OnMeasure(widthConstraint, heightConstraint);
        }

        public MyToolbar()
        {
            this.BackgroundColor = Color.FromRgb(0, 140, 253);
            this.VerticalOptions = LayoutOptions.Start;
            Padding = new Thickness(10, 0);
        }
    }
}
