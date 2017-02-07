using System.Collections.Generic;
using Android.App;
using Android.Widget;
using Android.OS;
using System.Linq;
using Android.Content.PM;

namespace SamsGear
{

    [Activity(Label = "Sams Gear", Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Landscape, Theme = "@android:style/Theme.NoTitleBar")]
    public class StockPage : Activity
    {
        //---------------------

        //View components

        private GridView gridViewAdapter;

        //---------------------


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.StockPage);

            //---------------------

            PopulateStockList();
        }

        protected override void OnRestart()
        {
            PopulateStockList();

            base.OnRestart();
        }

        #region StockView

        private void PopulateStockList()
        {
            //reset
            gridViewAdapter = null;

            using (Database database = new Database())
            {
                List<string> designImages = database.GetDesignImages();

                //Populate mainview with design images
                if (designImages.Any())
                {
                    GridView adapter = FindViewById<GridView>(Resource.Id.gridView1);
                    adapter.Adapter = new DesignAdapter(this, designImages.ToArray());
                    adapter.SetNumColumns(Settings.StockPageColumns);
                    adapter.SetColumnWidth(Settings.StockPageColumnWidth);

                    gridViewAdapter = adapter;
                    gridViewAdapter.FastScrollEnabled = true;
                    gridViewAdapter.ItemClick += PopulateStockView_ItemClick;
                }
            }
        }

        private void PopulateStockView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //reset
            gridViewAdapter.ItemClick -= PopulateStockView_ItemClick;

            List<TShirtEntity> finalTShirt = new List<TShirtEntity>();

            using (Database database = new Database())
            {
                List<DesignEntity> designEntity = database.GetDesignEntity();
                List<TShirtEntity> tshirtEntity = database.GetTShirtEntity();
                List<ColourEntity> colourEntity = database.GetColourEntity();
                List<SizeEntity> sizeEntity = database.GetSizeEntity();

                #region get tshirt
                //get selected DesignTShirt
                List<DesignTShirtEntity> indexDesign = designEntity[e.Position].DesignTShirtEntity;

                //------------------------

                for (int i = 0; i < indexDesign.Count(); i++)
                {
                    //Find matching TShirt
                    int id = indexDesign[i].IDTShirt;

                    foreach (TShirtEntity t in tshirtEntity)
                    {
                        //using selected DesignTShirt get all matching items
                        if (id == t.ID)
                        {
                            //Add items
                            finalTShirt.Add(t);
                        }
                    }
                }
                #endregion

                #region get colours

                List<string> finalColour = new List<string>();
                List<int> tshirtColourList = new List<int>();

                for (int i = 0; i < indexDesign.Count(); i++)
                {
                    int id = indexDesign[i].IDTShirt;

                    foreach (TShirtEntity t in tshirtEntity)
                    {
                        if (t.TShirtColourEntity != null)
                        {
                            if (id == t.ID)
                            {
                                foreach (TShirtColourEntity selectedTShirt in t.TShirtColourEntity)//gets x
                                {
                                    tshirtColourList.Add(selectedTShirt.IDColour);//gets y
                                }
                            }
                        }
                    }
                }

                for (int i = 0; i < tshirtColourList.Count(); i++)
                {
                    if (colourEntity.Any())
                    {
                        int id = tshirtColourList[i];

                        foreach (ColourEntity c in colourEntity)
                        {
                            if (id == c.ID)
                            {
                                finalColour.Add(c.Color);
                            }
                        }
                    }
                }

                #endregion get colours

                #region get size

                List<string> finalSize = new List<string>();
                List<int> tshirtSizeList = new List<int>();

                for (int i = 0; i < indexDesign.Count(); i++)
                {
                    int id = indexDesign[i].IDTShirt;

                    foreach (TShirtEntity t in tshirtEntity)
                    {
                        if (t.TShirtSizeEntity != null)
                        {
                            if (id == t.ID)
                            {
                                foreach (TShirtSizeEntity selectedTShirt in t.TShirtSizeEntity)//gets x
                                {
                                    tshirtSizeList.Add(selectedTShirt.IDSize);//gets y
                                }
                            }
                        }
                    }
                }

                for (int i = 0; i < tshirtSizeList.Count(); i++)
                {
                    int id = tshirtSizeList[i];

                    foreach (SizeEntity s in sizeEntity)
                    {
                        if (id == s.ID)
                        {
                            finalSize.Add(s.Size);
                        }
                    }
                }

                #endregion get size

                //Populate mainview
                if (finalTShirt.Any())
                {
                    GridView adapter = FindViewById<GridView>(Resource.Id.gridView1);
                    adapter.Adapter = new StockAdapter(this, designEntity[e.Position].Image, finalTShirt.ToArray(), finalColour, finalSize);
                    adapter.SetNumColumns(Settings.StockPageColumns);
                    adapter.SetColumnWidth(Settings.StockPageColumnWidth);
                    gridViewAdapter = adapter;
                }
            }
        }

        #endregion

        protected override void OnResume()
        {
            base.OnResume();
        }
    }
}

