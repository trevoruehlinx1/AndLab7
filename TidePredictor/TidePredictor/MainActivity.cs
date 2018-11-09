using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using System;
using System.Linq;
using System.Collections.Generic;
using Android.Views;

namespace TidePredictor
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : ListActivity
    {
        List<string> tideData;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            XmlTideFileParser reader = new XmlTideFileParser(Assets.Open("AnnualTidePredictions.xml"));
            tideData = new List<string>();
                   
            reader.TideList.ForEach(tide => {
                string highlow = tide["highlow"].ToString();
                if (highlow == "H")
                    highlow = "High";
                else
                    highlow = "Low";
                string tideText = tide["day"].ToString() + " " + tide["date"].ToString().Substring(5,2)
                                    + "/" +tide["date"].ToString().Substring(6,2) + tide["date"].ToString().Substring(2,2)+" "
                                   + tide["time"].ToString() + " " + highlow;
                tideData.Add(tideText);
            });

            //ListAdapter = new ArrayAdapter<string> (this, Resource.Layout.SimpleListItem1, tideData.ToArray());
            ListAdapter = new Adapter(tideData, this);

            ListView.TextFilterEnabled = true;

            ListView.FastScrollEnabled = true;
        }
        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            XmlTideFileParser reader = new XmlTideFileParser(Assets.Open("AnnualTidePredictions.xml"));
            var entry = reader.TideList[position];
            string tideHeight = entry["pred_in_ft"].ToString();
            double tideHeightInches = Convert.ToDouble(tideHeight)*12;
            string tideHeightString = "Tide Height is " + tideHeightInches.ToString() + " Inches";
            Toast.MakeText(Application,tideHeightString, ToastLength.Short).Show();
        }

    }
}

