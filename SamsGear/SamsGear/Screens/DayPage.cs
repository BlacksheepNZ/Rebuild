using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Linq;
using System.Threading;
using Android.Content.PM;
namespace SamsGear
{

    [Activity(Label = "Sams Gear", Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Landscape, Theme = "@android:style/Theme.NoTitleBar")]
    public class DayPage : Activity
    {
        //---------------------

        //View components

        private GridView gridViewAdapter;

        //---------------------

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.DayPage);

            //---------------------
            //Load resource views

            gridViewAdapter = FindViewById<GridView>(Resource.Id.gridView1);

            PopulateOrder();
        }

        protected override void OnRestart()
        {
            PopulateOrder();

            base.OnRestart();
        }

        #region DayView

        private void PopulateOrder()
        {
            Toast.MakeText(this, "Please wait...", ToastLength.Short).Show();

            gridViewAdapter = null;

            try
            {
                using (Database database = new Database())
                {
                    List<OrderEntity> order = database.GetOrderEntity();

                    GridView adapter = FindViewById<GridView>(Resource.Id.gridView1);
                    adapter.Adapter = new OrderAdapter(this, order);
                    adapter.SetNumColumns(Settings.OrderPageColumns);
                    adapter.SetColumnWidth(Settings.OrderPageColumnWidth);

                    adapter.SetGravity(GravityFlags.CenterHorizontal);

                    gridViewAdapter = adapter;
                    gridViewAdapter.FastScrollEnabled = true;
                    gridViewAdapter.ItemClick += OrderAdapter_ItemClick;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		/// <summary>
		/// Quite a big loading process, need to fix.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OrderAdapter_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
            try
            {
                gridViewAdapter.ItemClick -= OrderAdapter_ItemClick;
                gridViewAdapter = null;

                List<Tuple<int, int, int, int>> finalProduct = new List<Tuple<int, int, int, int>>();
                List<int> orderProductList = new List<int>();
                List<Tuple<int, int, int, int>> noProductDupes = new List<Tuple<int, int, int, int>>();

                using (Database database = new Database())
                {
                    List<OrderEntity> orderEntity = database.GetOrderEntity();
                    List<DesignEntity> designEntity = database.GetDesignEntity();
                    List<ProductEntity> productEntity = database.GetProductEntity();
                    List<TShirtEntity> tshirtEntity = database.GetTShirtEntity();
                    List<ColourEntity> colourEntity = database.GetColourEntity();
                    List<SizeEntity> sizeEntity = database.GetSizeEntity();

                    List<OrderProductEntity> indexOrder = orderEntity[e.Position].OrderProductEntity;

                    //------------------------
                    for (int i = 0; i < indexOrder.Count(); i++)
                    {
                        int id = indexOrder[i].ProductID;

                        foreach (ProductEntity product in productEntity)
                        {
                            if (product.OrderProductEntity != null)
                            {
                                if (id == product.ID)
                                {
                                    foreach (OrderProductEntity selectedIndex in product.OrderProductEntity)//gets x
                                    {
                                        orderProductList.Add(selectedIndex.ProductID);//gets y
                                    }
                                }
                            }
                        }
                    }

                    //------------------------
                    for (int i = 0; i < orderProductList.Count(); i++)
                    {
                        int id = orderProductList[i];

                        foreach (ProductEntity product in productEntity)
                        {
                            if (id == product.ID)
                            {
                                finalProduct.Add(new Tuple<int, int, int, int>(product.DesignID, product.TShirtID, product.TShirtColorID, product.TShirtSizeID));
                            }
                        }
                    }

                    noProductDupes = finalProduct.Distinct().ToList();

                    List<int> designIndex = new List<int>();
                    List<int> colourIndex = new List<int>();
                    List<int> sizeIndex = new List<int>();

                    if (noProductDupes.Any())
                    {
                        for (int i = 0; i < noProductDupes.Count; i++)
                        {
                            designIndex.Add(noProductDupes[i].Item1);
                            colourIndex.Add(noProductDupes[i].Item3);
                            sizeIndex.Add(noProductDupes[i].Item4);
                        }

                        GridView adapter = FindViewById<GridView>(Resource.Id.gridView1);
                        adapter.Adapter = new ProductAdapter(this, designEntity, colourEntity, sizeEntity, designIndex, colourIndex, sizeIndex);
                        adapter.SetNumColumns(Settings.ProductPageColumns);
                        adapter.SetColumnWidth(Settings.ProductPageColumnWidth);
                        adapter.SetGravity(GravityFlags.CenterHorizontal);

                        gridViewAdapter = adapter;
                        gridViewAdapter.FastScrollEnabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
		}

        #endregion

        protected override void OnResume()
        {
            base.OnResume();
        }
    }
}

