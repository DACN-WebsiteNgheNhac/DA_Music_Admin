using CustomControls.Utils;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;


namespace CustomControls.Controls
{
    public partial class CustomItemMenu : UserControl
    {
        string iconMainMemu;
        string mainMenu;
        string[] subMenu;
        string[] iconSubMenu;
        Thickness paddingLeft = new Thickness(15, 0, 0, 0);
        bool isOpen = false;
        MyAnimation animation = new MyAnimation(60);

        CustomRadioButton _MainItemMenu;
        public CustomRadioButton MainItemMenu
        {
            get { return _MainItemMenu; }
            set { _MainItemMenu = value; }
        }

        List<CustomRadioButton> _SubItemMenu = new List<CustomRadioButton>();
        public List<CustomRadioButton> SubItemMenu
        {
            get { return _SubItemMenu; }
            set { _SubItemMenu = value; }
        }



        private event EventHandler _Click;
        public event EventHandler Click
        {
            add { _Click += value; }
            remove { _Click -= value; }
        }

        bool isFirstChecked = false;

        public CustomItemMenu()
        {
            InitializeComponent();
        }

        public CustomItemMenu(string iconMainMemu, string mainMenu, string[] iconSubMenu, string[] subMenu, bool isFirstChecked = false)
        {
            InitializeComponent();
            this.iconMainMemu = iconMainMemu;
            this.mainMenu = mainMenu;
            this.iconSubMenu = iconSubMenu;
            this.subMenu = subMenu;
            this.isFirstChecked = isFirstChecked;
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            MainItemMenu = new CustomRadioButton(iconMainMemu, mainMenu, isFirstChecked);
            spContainerMainMenu.Children.Add(MainItemMenu);
            MainItemMenu.Click += MainItem_Click;

            spContainerMainMenu.Children.RemoveAt(0);

            if (subMenu.Length == 0)
            {
                MainItemMenu.IsMainMenu = false;
                return;
            }
            for (int i = 0; i < subMenu.Length; i++)
            {
                CustomRadioButton item = new CustomRadioButton(iconSubMenu[i], subMenu[i], paddingLeft);
                spContainerSubMenu.Children.Add(item);
                SubItemMenu.Add(item);
                item.Click += MainItem_Click;
            }

            spContainerMainMenu.Children.Add(spContainerSubMenu);
        }


        public void MainItem_Click(object sender, RoutedEventArgs e)
        {
            if (_Click != null)
                _Click(sender, e);
            if (subMenu.Length == 0)
                return;
            if (!(sender as CustomRadioButton).IsMainMenu)
                return;
            if (!isOpen)
            {
                double toValue = MainItemMenu.ActualHeight * subMenu.Length;
                animation.StartAnimationWithEaseFunction(spContainerSubMenu, FrameworkElement.HeightProperty
                    , (int)OptionEasingFunction.CubicEase, toValue, 0, 125);
                isOpen = !isOpen;
            }
            else
            {
                double toValue = 0;
                animation.StartAnimationWithEaseFunction(spContainerSubMenu, FrameworkElement.HeightProperty
                    , (int)OptionEasingFunction.BackEase, toValue, 0, 125);
                isOpen = !isOpen;
            }
        }
    }
}
