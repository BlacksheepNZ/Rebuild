﻿using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using System.IO;

namespace SamsGear
{
    [Activity(Label = "SamsGear", MainLauncher = true, Theme = "@android:style/Theme.NoTitleBar", Icon = "@drawable/icon")] //MainLauncher = true
    public class MainActivity : TabActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            INIDatabase();

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            CreateTab(typeof(DayPage), Resource.Drawable.DayButton);
            CreateTab(typeof(SellPage), Resource.Drawable.SellButton);
            CreateTab(typeof(StockPage), Resource.Drawable.StockButton);

            SetBackground(TabHost);
            TabHost.CurrentTab = 1;
        }

        private void CreateTab(Type activityType, int drawableId)
        {
            var intent = new Intent(this, activityType);
            intent.AddFlags(ActivityFlags.NewTask);

            var spec = TabHost.NewTabSpec("");
            var drawableIcon = Resources.GetDrawable(drawableId);
            spec.SetIndicator("", drawableIcon);
            spec.SetContent(intent);

            TabHost.AddTab(spec);
        }

        public void SetBackground(TabHost tabhost)
        {
            for (int i = 0; i < tabhost.TabWidget.ChildCount; i++)
            {
                tabhost.TabWidget.GetChildAt(i).SetBackgroundResource(Resource.Color.white); //unselected
            }
            tabhost.TabWidget.GetChildAt(tabhost.CurrentTab).SetBackgroundResource(Resource.Color.white); // selected
            tabhost.TabWidget.StripEnabled = true;
            tabhost.TabWidget.SetLeftStripDrawable(Resource.Color.Gray);
            tabhost.TabWidget.SetRightStripDrawable(Resource.Color.Gray);
        }

        public void INIDatabase()
        {
            //Reads from local database file
            //Transfers file to device

            try
            {
                var readStream = Resources.OpenRawResource(Resource.Raw.easyDB);
                FileStream writeStream = new FileStream(Database.databasePath, FileMode.OpenOrCreate, FileAccess.Write);
                Database.ReadWriteStream(readStream, writeStream);
                writeStream.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

