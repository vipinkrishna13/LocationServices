using System;
using Android;
using Android.Content.PM;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using LocationSample.Droid;
using Xamarin.Forms;
[assembly: Dependency(typeof(LocationListener))]
namespace LocationSample.Droid
{
    public class LocationListener : AppCompatActivity, ILocationListener, ILocationHelper
    {
        const long ONE_MINUTE = 60 * 1000;
        const long FIVE_MINUTES = 5 * ONE_MINUTE;
        static readonly string KEY_REQUESTING_LOCATION_UPDATES = "requesting_location_updates";
        protected LocationManager locationManager = (LocationManager)Android.App.Application.Context.GetSystemService(LocationService);
        static readonly int RC_LAST_LOCATION_PERMISSION_CHECK = 1000;
        static readonly int RC_LOCATION_UPDATES_PERMISSION_CHECK = 1100;
         bool isRequestingLocationUpdates;
        IFileLogger _logger;
        public event EventHandler<LocationData> OnLocationReceived;

        public LocationListener()
        {
            _logger = DependencyService.Get<IFileLogger>();
            isRequestingLocationUpdates = false;

        }


        public void OnLocationChanged(Location location)
        {
            _logger.LogInformation($"{DateTime.Now} - Lat: {location.Latitude} , Long: {location.Longitude} , Speed: {location.Speed*3.6}");
            OnLocationReceived.Invoke(this, new LocationData()
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                Speed = location.Speed
            });
        }

        public void OnProviderDisabled(string provider)
        {
            isRequestingLocationUpdates = false;
        }

        public void OnProviderEnabled(string provider)
        {
            // Nothing to do in this example.
            Log.Debug("LocationExample", "The provider " + provider + " is enabled.");
        }
 
        private void FetchRequestingLocationUpdates()
        {
             locationManager.RequestLocationUpdates(LocationManager.GpsProvider, 0, 0, this);
        }

        void StopRequestingLocationUpdates()
        {
            locationManager.RemoveUpdates(this);
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            if (status == Availability.OutOfService)
            {
                StopRequestingLocationUpdates();
                isRequestingLocationUpdates = false;
            }
        }

        public void Start()
        {
            FetchRequestingLocationUpdates();
            isRequestingLocationUpdates = true;
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void PositionChanged()
        {
            throw new NotImplementedException();
        }


        //void RequestLocationUpdatesButtonOnClick(object sender, EventArgs eventArgs)
        //{
        //    if (isRequestingLocationUpdates)
        //    {
        //        isRequestingLocationUpdates = false;
        //        StopRequestingLocationUpdates();
        //    }
        //    else
        //    {
        //        if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) == Permission.Granted)
        //        {
        //            StartRequestingLocationUpdates();
        //            isRequestingLocationUpdates = true;
        //        }
        //        else
        //        {
        //            RequestLocationPermission(RC_LAST_LOCATION_PERMISSION_CHECK);
        //        }
        //    }
        //}

        //void GetLastLocationButtonOnClick(object sender, EventArgs eventArgs)
        //{
        //    if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) == Permission.Granted)
        //    {
        //        GetLastLocationFromDevice();
        //    }
        //    else
        //    {
        //        RequestLocationPermission(RC_LAST_LOCATION_PERMISSION_CHECK);
        //    }
        //}

        //void GetLastLocationFromDevice()
        //{
        //    getLastLocationButton.SetText(Resource.String.getting_last_location);

        //    var criteria = new Criteria { PowerRequirement = Power.Medium };

        //    var bestProvider = locationManager.GetBestProvider(criteria, true);
        //    var location = locationManager.GetLastKnownLocation(bestProvider);

        //    if (location != null)
        //    {
        //        latitude.Text = Resources.GetString(Resource.String.latitude_string, location.Latitude);
        //        longitude.Text = Resources.GetString(Resource.String.longitude_string, location.Longitude);
        //        provider.Text = Resources.GetString(Resource.String.provider_string, location.Provider);
        //        getLastLocationButton.SetText(Resource.String.get_last_location_button_text);
        //    }
        //    else
        //    {
        //        latitude.SetText(Resource.String.location_unavailable);
        //        longitude.SetText(Resource.String.location_unavailable);
        //        provider.Text = Resources.GetString(Resource.String.provider_string, bestProvider);
        //        getLastLocationButton.SetText(Resource.String.get_last_location_button_text);
        //    }
        //}


    }

}
