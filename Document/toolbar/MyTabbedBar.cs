using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinApp.Views
{
    public class MyTabbedBarItem : Grid, IMyToolItem
    {
        public event EventHandler Clicked;

        bool _activated;
        public bool Activated
        {
            get => _activated;
            set
            {
                if (_activated != value)
                {
                    _activated = value;
                    OnActivated();
                }
            }
        }
        public virtual void OnActivated()
        {
        }
        public string Notify
        {
            get => _bagg.Text;
            set => _bagg.Text = value;
        }

        public void SetColor(Color color)
        {
            ((TintColor)Effects[0]).Foreground = color;
        }

        Label _bagg;
        Label _text;
        Image _img;
        

        public MyTabbedBarItem(MenuInfo info)
        {
            var btn = new Button
            {
                BackgroundColor = Color.Transparent,
            };
            btn.Clicked += (s, e) => {
                Clicked?.Invoke(this, EventArgs.Empty);
            };

            _bagg = new Label { 
            
            };
            _img = new Image
            {
                Source = info.IconName,
            };
            _text = new Label {
                Text = info.Text,
            };

            this.Children.Add(_img);
            this.Children.Add(_bagg);
            this.Children.Add(btn);

            this.Children.Add(_text);
        }
    }
    public class MyTabbedBar : Grid
    {
        class ImgTool : Grid, IMyToolItem
        {
            public event EventHandler Clicked;
            public void SetColor(Color color)
            {
                ((TintColor)Effects[0]).Foreground = color;
            }
            public ImgTool(string source)
            {
                var btn = new Button {
                    BackgroundColor = Color.Transparent,
                };
                var bagged = new Label {
                };
                var img = new Image { 
                    Source = source, 
                };

                this.Children.Add(img);
                this.Children.Add(btn);
            }
        }

        public static readonly BindableProperty ForegroundProperty =
            BindableProperty.Create(nameof(ForegroundProperty), typeof(Color), typeof(MyEntry), Color.Gray);
        public Color Foreground
        {
            get { return (Color)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }
        public Color ActiveColor { get; set; } = Color.Blue;

        public IList<MenuInfo> ToolbarItems { get; private set; } = new List<MenuInfo>();
        public event EventHandler<MyToolbarEventArgs> ExecuteCommand;
        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            if (Children.Count == 0)
            {
                double imgHeight = this.HeightRequest / 2;
                foreach (var info in ToolbarItems)
                {
                    var item = new MyTabbedBarItem(info);

                    item.SetColor(Foreground);
                    info.Button = item;

                    if (info.Callback != null)
                    {
                        item.Clicked += (s, e) => info.Callback.Invoke();
                    }
                    if (info.Command != null)
                    {
                        item.Clicked += (s, e) => ExecuteCommand?.Invoke(this, new MyToolbarEventArgs
                        {
                            Command = info.Command
                        });
                    }
                }
            }

            return base.OnMeasure(widthConstraint, heightConstraint);
        }

        public MyTabbedBar()
        {
            this.BackgroundColor = Color.FromRgb(0, 140, 253);
            this.VerticalOptions = LayoutOptions.End;
            Padding = new Thickness(10, 0);
        }
    }
}
