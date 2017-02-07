using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using System.Linq;
using Android.Content.PM;

namespace SamsGear
{

    [Activity(Label = "Sams Gear", Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Landscape, Theme = "@android:style/Theme.NoTitleBar")]
    public class SellPage : Activity
    {
        //---------------------

        //View components

        private GridView gridViewAdapter;
        private ListView listCart;

        private TextView cost;

        private ImageButton finalSell;

        //---------------------

        /// <summary>
        /// Image, Colour, Size, Price
        /// </summary>
        public static List<Tuple<string, string, string, decimal>> cartItems;

        /// <summary>
        /// DesignID, TShirtID, TShirtColour, TShirtSize
        /// </summary>
        public static List<Tuple<int, int, int, int>> finalCartItem;

        //Main item selection
        private int selectedDesign;
        private int selectedTShirt;
        private int selectedTShirtColour;
        private int selectedTShirtSize;

        public static decimal totalcost = 0;

        private List<string> noDupes = new List<string>();

        //---------------------

        //private List<string> designImages = new List<string>();

        private List<string> finalColour = new List<string>();
        private List<int> tshirtColourList = new List<int>();

        private List<string> finalSize = new List<string>();
        private List<int> tshirtSizeList = new List<int>();

        //---------------------

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.SellPage);

            //---------------------
            //Load resource views

            cost = FindViewById<TextView>(Resource.Id.myImageViewText);
            cost.Text = "Total";
            cost.Click += buttonPay_Click;

            gridViewAdapter = FindViewById<GridView>(Resource.Id.gridView1);

            listCart = FindViewById<ListView>(Resource.Id.listView1);
            listCart.ItemClick += listCart_ItemClick;

            //---------------------

            finalCartItem = new List<Tuple<int, int, int, int>>();
            cartItems = new List<Tuple<string, string, string, decimal>>();

            finalSell = FindViewById<ImageButton>(Resource.Id.imageButton1);
            finalSell.Click += buttonPay_Click;

            //---------------------

            PopulateProductList();
        }

        protected override void OnRestart()
        {
            PopulateProductList();

            base.OnRestart();
        }

        //enum OrderType
        //{
        //    Ascending,
        //    Descending,
        //}

        //OrderType currentSelected = OrderType.Ascending;
        //OrderType lastSelected;
        //private void buttonChangeView_Click(object sender, EventArgs e)
        //{
        //    using (Database db = new Database())
        //    {
        //        var design = db.GetDesignImages();

        //        if (currentSelected == OrderType.Ascending)
        //        {
        //            //buttonChangeView.SetImageResource(Resource.Drawable.UpArrow);
        //            design = Order(design, currentSelected);

        //            //switch
        //            lastSelected = OrderType.Descending;
        //            currentSelected = lastSelected;
        //        }
        //        else if (currentSelected == OrderType.Descending)
        //        {
        //            //buttonChangeView.SetImageResource(Resource.Drawable.DownArrow);
        //            design = Order(design, currentSelected);

        //            //switch
        //            lastSelected = OrderType.Ascending;
        //            currentSelected = lastSelected;
        //        }
        //    }
        //    PopulateProductList();

        //}

        private void buttonPay_Click(object sender, EventArgs e)
        {
            var SellPage = new Intent(this, typeof(FinalizeSellPage));

            StartActivity(SellPage);
        }

        #region SellView

        private void PopulateProductList()
        {
            //reset
            selectedDesign = -1;
            selectedTShirt = -1;
            selectedTShirtColour = -1;
            selectedTShirtSize = -1;

            gridViewAdapter = null;

            try
            {
                using (Database db = new SamsGear.Database())
                {
                    var images = db.GetDesignImages();

                    if (images.Any())
                    {
                        GridView adapter = FindViewById<GridView>(Resource.Id.gridView1);
                        adapter.Adapter = new DesignAdapter(this, images.ToArray());
                        adapter.NumColumns = Settings.DesignPageColumns;
                        adapter.SetColumnWidth(Settings.DesignPageColumnWidth);

                        gridViewAdapter = adapter;
                        gridViewAdapter.FastScrollEnabled = true;
                        gridViewAdapter.ItemClick += PopulateColourList_ItemClick;
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            if (cartItems.Count > 0)
            {
                UpdateCost();
            }
        }
        private void PopulateColourList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            gridViewAdapter.ItemClick -= PopulateColourList_ItemClick;
            selectedDesign = e.Position;

            //finalColour.Clear();
            //tshirtColourList.Clear();
            noDupes.Clear();

            using (Database db = new SamsGear.Database())
            {
                var design = db.GetDesignEntity();
                var tshirt = db.GetTShirtEntity();

                var colour = db.GetColourEntity();

                var indexDesign = design[e.Position].DesignTShirtEntity;

                //------------------------
                for (int tshirtIndex = 0; tshirtIndex < indexDesign.Count(); tshirtIndex++)
                {
                    if (tshirt.Any())
                    {
                        int tID = indexDesign[tshirtIndex].IDTShirt;

                        foreach (TShirtEntity t in tshirt)
                        {
                            if (t.TShirtColourEntity != null)
                            {
                                if (tID == t.ID)
                                {
                                    foreach (TShirtColourEntity selectTShirt in t.TShirtColourEntity)//gets x
                                    {
                                        tshirtColourList.Add(selectTShirt.IDColour);//gets y
                                    }
                                }
                            }
                        }
                    }
                }

                //------------------------
                for (int colourIndex = 0; colourIndex < tshirtColourList.Count(); colourIndex++)
                {
                    if (colour.Any())
                    {
                        int cID = tshirtColourList[colourIndex];

                        foreach (ColourEntity c in colour)
                        {
                            if (cID == c.ID)
                            {
                                finalColour.Add(c.Color);
                            }
                        }
                    }
                }

                noDupes = finalColour.Distinct().ToList();

                if (noDupes.Any())
                {
                    GridView adapter = FindViewById<GridView>(Resource.Id.gridView1);
                    adapter.Adapter = new ColourAdapter(this, noDupes.ToArray());
                    adapter.SetNumColumns(5);
                    adapter.SetColumnWidth(15);

                    gridViewAdapter = adapter;
                    gridViewAdapter.FastScrollEnabled = true;
                    gridViewAdapter.ItemClick += PopulateSizeList_ItemClick;
                }
            }
        }
        private void PopulateSizeList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            gridViewAdapter.ItemClick -= PopulateColourList_ItemClick;
            gridViewAdapter.ItemClick -= PopulateSizeList_ItemClick;

            using (Database db = new Database())
            {
                var colour = db.GetColours();

                var design = db.GetDesignEntity();
                var tshirt = db.GetTShirtEntity();
                var size = db.GetSizeEntity();

                foreach (var c in colour)
                {
                    if (e.Position < noDupes.Count())
                    {
                        if (noDupes[e.Position].Contains(c.Item2))
                        {
                            selectedTShirtColour = c.Item1 - 1;
                            break;
                        }
                    }
                    else
                    {
                        PopulateProductList();
                    }
                }

                finalSize.Clear();
                tshirtSizeList.Clear();

                var indexDesign = design[e.Position].DesignTShirtEntity;

                //------------------------
                for (int tshirtIndex = 0; tshirtIndex < indexDesign.Count(); tshirtIndex++)
                {
                    if (tshirt.Any())
                    {
                        int tID = indexDesign[tshirtIndex].IDTShirt;

                        foreach (TShirtEntity t in tshirt)
                        {
                            if (t.TShirtSizeEntity != null)
                            {
                                if (tID == t.ID)
                                {
                                    foreach (TShirtSizeEntity selectTShirt in t.TShirtSizeEntity)//gets x
                                    {
                                        tshirtSizeList.Add(selectTShirt.IDSize);//gets y
                                    }
                                }
                            }
                        }
                    }
                }

                //------------------------
                for (int colourIndex = 0; colourIndex < tshirtSizeList.Count(); colourIndex++)
                {
                    if (size.Any())
                    {
                        int cID = tshirtSizeList[colourIndex];

                        foreach (SizeEntity s in size)
                        {
                            if (cID == s.ID)
                            {
                                finalSize.Add(s.Size);
                            }
                        }
                    }
                }


                if (finalSize.Any())
                {
                    GridView adapter = FindViewById<GridView>(Resource.Id.gridView1);
                    adapter.Adapter = new SizeAdapter(this, finalSize.ToArray());
                    adapter.SetNumColumns(5);
                    adapter.SetColumnWidth(15);

                    gridViewAdapter = adapter;
                    gridViewAdapter.FastScrollEnabled = true;
                    gridViewAdapter.ItemClick += AddItemToCart;
                }
            }
        }
        private void AddItemToCart(object sender, AdapterView.ItemClickEventArgs e)
        {
            gridViewAdapter.ItemClick -= PopulateColourList_ItemClick;
            gridViewAdapter.ItemClick -= PopulateSizeList_ItemClick;
            gridViewAdapter.ItemClick -= AddItemToCart;
            selectedTShirtSize = e.Position;

            try
            {
                //Add item to cart
                if (selectedDesign >= 0 && selectedTShirtColour >= 0 && selectedTShirtSize >= 0)
                {
                    using (Database db = new Database())
                    {
                        var design = db.GetDesignEntity();
                        var colour = db.GetColourEntity();
                        var tshirt = db.GetTShirtEntity();
                        var size = db.GetSizeEntity();

                        cartItems.Add(new Tuple<string, string, string, decimal>(
                            design[selectedDesign].Image,
                            colour[selectedTShirtColour].Color,
                            size[selectedTShirtSize].Size,
                            design[selectedDesign].Price));

                        listCart.Adapter = new CartAdapter(this, cartItems);

                        //remove item from stock
                        List<DesignTShirtEntity> indexDesign = design[selectedDesign].DesignTShirtEntity;
                        selectedTShirt = indexDesign[selectedTShirtSize].IDTShirt;

                        TShirtEntity t = tshirt[selectedTShirt - 1];
                        t.StockNZ -= 1;

                        db.AddORUpdateTShirtEntity(t);

                        //finalize

                        finalCartItem.Add(new Tuple<int, int, int, int>(
                            selectedDesign,
                            selectedTShirt,
                            selectedTShirtColour,
                            selectedTShirtSize));

                        Toast.MakeText(this, "Added " + design[selectedDesign].Name, ToastLength.Long).Show();
                    }
                }
            }
            catch
            {
                throw;
            }

            PopulateProductList();
        }

        #endregion

        //Update total cost of items in cart
        private void UpdateCost()
        {
            //clear
            totalcost = 0;

            //populate cost
            foreach (var items in cartItems)
            {
                totalcost += items.Item4;
            }

            //update total cost
            cost.Text = "Total" + "\n" + totalcost.ToString();
        }

        //Remove item from cart
        private void listCart_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            try
            {
                if (cartItems.Count() > 0)
                {
                    cartItems.RemoveAt((int)e.Position);
                    listCart.Adapter = new CartAdapter(this, cartItems);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            UpdateCost();
        }

        protected override void OnResume()
        {
            base.OnResume();
        }
    }
}

