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

using System.IO;
using System.Drawing;
using Android.Graphics;

namespace SamsGear
{
    public class ProductAdapter : BaseAdapter
    {
        private Activity context;

        private String imageName;

        private List<DesignEntity> designEntity = new List<DesignEntity>();
        private List<ColourEntity> colourEntity = new List<ColourEntity>();
        private List<SizeEntity> sizeEntity = new List<SizeEntity>();

        private List<int> designIndex = new List<int>();
        private List<int> colourIndex = new List<int>();
        private List<int> sizeIndex = new List<int>();

        public ProductAdapter(Activity context,
            List<DesignEntity> designEntity,
            List<ColourEntity> colourEntity,
            List<SizeEntity> SizeEntity,
            List<int> designIndex,
            List<int> colourIndex,
            List<int> sizeIndex)
        {
            this.context = context;

            this.designEntity.AddRange(designEntity);
            this.colourEntity.AddRange(colourEntity);
            this.sizeEntity.AddRange(SizeEntity);

            this.designIndex.AddRange(designIndex);
            this.colourIndex.AddRange(colourIndex);
            this.sizeIndex.AddRange(sizeIndex);
        }
        public override int Count
        {
            get { return this.designIndex.Count(); }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null) // no view to re-use, create new
            {
                view = LayoutInflater.From(context).Inflate(Resource.Layout.SingleProductView, parent, false);

                ImageView imageView = view.FindViewById<ImageView>(Resource.Id.imageView1); //image
                TextView textView1 = view.FindViewById<TextView>(Resource.Id.textView1);    //size
                TextView textView2 = view.FindViewById<TextView>(Resource.Id.textView2);    //color
                TextView textView3 = view.FindViewById<TextView>(Resource.Id.textView3);    //price

                imageName = designEntity[designIndex[position]].Image;
                textView1.Text = colourEntity[colourIndex[position]].Color;
                textView2.Text = sizeEntity[sizeIndex[position]].Size;
                textView3.Text = designEntity[designIndex[position]].Price.ToString();

                String img = imageName;

                if (img.Equals("FiaBotz"))
                    imageView.SetImageResource(Resource.Drawable.FiaBotz);
                else if (img.Equals("TokoUso"))
                    imageView.SetImageResource(Resource.Drawable.TokoUso);
                else if (img.Equals("Beatz"))
                    imageView.SetImageResource(Resource.Drawable.Beatz);
                else if (img.Equals("Blues"))
                    imageView.SetImageResource(Resource.Drawable.Blues);
                else if (img.Equals("Bulu"))
                    imageView.SetImageResource(Resource.Drawable.Bulu);
                else if (img.Equals("Usos"))
                    imageView.SetImageResource(Resource.Drawable.Usos);
                else if (img.Equals("CookIsland"))
                    imageView.SetImageResource(Resource.Drawable.CookIslands);
                else if (img.Equals("Fobzilla"))
                    imageView.SetImageResource(Resource.Drawable.Fobzilla);
                else if (img.Equals("iFob"))
                    imageView.SetImageResource(Resource.Drawable.iFob);
                else if (img.Equals("iFobMini"))
                    imageView.SetImageResource(Resource.Drawable.iFobMini);
                else if (img.Equals("MaoriStrong"))
                    imageView.SetImageResource(Resource.Drawable.MaoriStrong);
                else if (img.Equals("Maroons"))
                    imageView.SetImageResource(Resource.Drawable.Maroons);
                else if (img.Equals("NiueStrong"))
                    imageView.SetImageResource(Resource.Drawable.NiueStrong);
                else if (img.Equals("ObeyDeez"))
                    imageView.SetImageResource(Resource.Drawable.ObeyDeez);
                else if (img.Equals("SonsSth"))
                    imageView.SetImageResource(Resource.Drawable.SonsSth);
                else if (img.Equals("ToaSamoa"))
                    imageView.SetImageResource(Resource.Drawable.ToaSamoa);
                else if (img.Equals("Warriors"))
                    imageView.SetImageResource(Resource.Drawable.Warriors);
                else
                {
                    imageView.SetImageResource(Resource.Drawable.White);
                }
            }
            else
            {
                view = convertView;
            }

            return view;
        }
    }
}