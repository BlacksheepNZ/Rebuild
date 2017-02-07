using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using System.Data;
using SQLite;

namespace SamsGear
{
    [Activity(Label = "Sams Gear", Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Landscape, Theme = "@android:style/Theme.NoTitleBar")]
    public class FinalizeSellPage : Activity
    {
        private ImageButton buttonPay;

        //---------------------

        private EditText name;
        private EditText email;

        //---------------------

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            CreateView();
        }

        protected override void OnRestart()
        {
            CreateView();

            base.OnRestart();
        }

        /// <summary>
        /// Load view for sellpage
        /// </summary>
        private void CreateView()
        {
            SetContentView(Resource.Layout.FinalizeSellPage);

            buttonPay = FindViewById<ImageButton>(Resource.Id.imageButton3);
            buttonPay.Click += buttonPay_Click;

            name = FindViewById<EditText>(Resource.Id.editText1);
            email = FindViewById<EditText>(Resource.Id.editText2);

            ListView listCart = FindViewById<ListView>(Resource.Id.listView1);
            listCart.Adapter = new CartAdapter(this, SellPage.cartItems);
        }

        private void buttonPay_Click(object sender, EventArgs e)
        {
            try
            {
                //deselect pay button
                buttonPay.Click -= buttonPay_Click;

                //user data information
                OrderEntity order = new OrderEntity();
                order.Name = name.Text;
                order.Email = email.Text;
                order.DateTime = DateTimeSQLite(DateTime.Now);

                int orderCount = 0;
                int productCount = 0;

                using (Database database = new Database())
                {
                    //create/add item order number
                    orderCount = database.Order().Count() + 1;

                    database.AddORUpdateOrderEntity(order);

                    //add each item in the card to database
                    foreach (var items in SellPage.finalCartItem)
                    {
                        ProductEntity productEntity = new ProductEntity();
                        productEntity.DesignID = items.Item1;
                        productEntity.TShirtID = items.Item2;
                        productEntity.TShirtColorID = items.Item3;
                        productEntity.TShirtSizeID = items.Item4;

                        database.AddORUpdateProductEntity(productEntity);

                        productCount = database.Product().Count() + 1;
                        OrderProductEntity op = new OrderProductEntity();
                        op.OrderID = orderCount;
                        op.ProductID = productCount;
                        database.AddORUpdateOrderProductEntity(op);
                    }

                    Toast.MakeText(this, "Transaction Completed.", ToastLength.Short).Show();
                }

                //clear items
                SellPage.totalcost = 0;
                SellPage.cartItems.Clear();
                SellPage.finalCartItem.Clear();

                //return to main page
                var MainPage = new Intent(this, typeof(MainActivity));
                StartActivity(MainPage);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Returns current datetime in speacificed format
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        private string DateTimeSQLite(DateTime datetime)
        {
            string dateTimeFormat = "{0}-{1}-{2} {3}:{4}:{5}.{6}";
            return string.Format(dateTimeFormat, datetime.Year, datetime.Month, datetime.Day, datetime.Hour, datetime.Minute, datetime.Second, datetime.Millisecond);
        }
    }
}