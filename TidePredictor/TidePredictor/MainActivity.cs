using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using System;
using System.Linq;
using System.Collections.Generic;
using Android.Views;
using System.Data.SQLite;
using System.IO;
using TidePredictorDataAccess_Library;

namespace TidePredictor
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : ListActivity
    {
        List<string> tideData;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            /* ------ copy and open the dB file using the SQLite-Net ORM ------ */

            string dbPath = "";
            SQLiteConnection db = null;

            // Get the path to the database that was deployed in Assets
            dbPath = Path.Combine(
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "tides.db3");

            // It seems you can read a file in Assets, but not write to it
            // so we'll copy our file to a read/write location
            using (Stream inStream = Assets.Open("tides.db3"))
            using (Stream outStream = File.Create(dbPath))
                inStream.CopyTo(outStream);

            // Open the database
            db = new SQLiteConnection(dbPath);

            

            ListAdapter = new ArrayAdapter<string> (this, Resource.Layout.SimpleListItem1, tideData.ToArray());
            //ListAdapter = new Adapter(tideData, this);

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


        //OLD ONCREATE METHOD

        //protected override void OnCreate(Bundle savedInstanceState)
        //{
        //    base.OnCreate(savedInstanceState);

        //    XmlTideFileParser reader = new XmlTideFileParser(Assets.Open("AnnualTidePredictions.xml"));
        //    tideData = new List<string>();
        //    var stuff = reader.TideList;

        //    reader.TideList.ForEach(tide => {
        //        string highlow = tide["highlow"].ToString();
        //        if (highlow == "H")
        //            highlow = "High";
        //        else
        //            highlow = "Low";
        //        string tideText = tide["day"].ToString() + " " + tide["date"].ToString().Substring(5, 2)
        //                            + "/" + tide["date"].ToString().Substring(6, 2) + tide["date"].ToString().Substring(2, 2) + " "
        //                           + tide["time"].ToString() + " " + highlow;
        //        tideData.Add(tideText);
        //    });

        //    //ListAdapter = new ArrayAdapter<string> (this, Resource.Layout.SimpleListItem1, tideData.ToArray());
        //    //ListAdapter = new Adapter(tideData, this);

        //    ListView.TextFilterEnabled = true;
        //    ListView.FastScrollEnabled = true;
        //}
    }
}

