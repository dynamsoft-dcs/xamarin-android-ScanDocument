using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Com.Dynamsoft.Camerasdk.View;
using Org.Litepal;
using Com.Dynamsoft.Camerasdk.Exception;
using Android.Content.PM;
using System;
using Android.Runtime;
using Com.Dynamsoft.Camerasdk.Model;
using Android.Views;

namespace ScanDocument
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]

    public class MainActivity : AppCompatActivity,IDcsVideoViewListener,IDcsViewListener, Android.Views.View.IOnClickListener
    {
        private DcsView dcsView;
        private TextView tvShow;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            LitePal.Initialize(ApplicationContext);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            try
            {
                DcsView.SetLicense(ApplicationContext, "your license number");
            }
            catch (DcsValueNotValidException e)
            {
                e.PrintStackTrace();
            }
            
            dcsView = FindViewById<DcsView>(Resource.Id.dcsview_id);
            dcsView.CurrentView = DcsView.DveImagegalleryview;
            dcsView.SetListener(this);
            DcsView.SetLogLevel(DcsView.DlleDebug);
            try
            {
                dcsView.VideoView.Mode = DcsView.DmeDocument;
            }
            catch(DcsValueOutOfRangeException e)
            {
                e.PrintStackTrace();
            }
            dcsView.VideoView.NextViewAfterCancel = DcsView.DveImagegalleryview;
            dcsView.VideoView.NextViewAfterCapture = DcsView.DveEditorview;
            dcsView.VideoView.SetListener(this);

            tvShow = FindViewById<TextView>(Resource.Id.tv_show_id);
            tvShow.SetOnClickListener(this);

        }

        protected override void OnStart()
        {
            base.OnStart();
            RequestDynamsoftCameraPermissions();
            if (dcsView.CurrentView == DcsView.DveVideoview)
            {
                try
                {
                    dcsView.VideoView.Preview();
                }
                catch (DcsCameraNotAuthorizedException e)
                {
                    e.PrintStackTrace();
                }
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (dcsView.CurrentView == DcsView.DveVideoview)
            {
                try
                {
                    dcsView.VideoView.Preview();
                }
                catch (DcsCameraNotAuthorizedException e)
                {
                    e.PrintStackTrace();
                }
            }
        }
        protected override void OnStop()
        {
            base.OnStop();
            dcsView.VideoView.StopPreview();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            dcsView.VideoView.DestroyCamera();
        }
        private const int CAMERA_OK = 10;
        private const int REQUEST_EXTERNAL_STORAGE = 1;
        private void RequestDynamsoftCameraPermissions()
        {
            if (Build.VERSION.SdkInt > BuildVersionCodes.LollipopMr1) // 22
            {
                try
                {
                    if (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(this, "android.permission.WRITE_EXTERNAL_STORAGE") != Permission.Granted)
                    {
                        Android.Support.V4.App.ActivityCompat.RequestPermissions(this, new String[] { Android.Manifest.Permission.ReadExternalStorage, Android.Manifest.Permission.WriteExternalStorage }, REQUEST_EXTERNAL_STORAGE);
                    }
                    if (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.Camera) != Permission.Granted)
                    {
                        Android.Support.V4.App.ActivityCompat.RequestPermissions(this, new String[] { Android.Manifest.Permission.Camera }, CAMERA_OK);
                    }
                }
                catch { }
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            try
            {
                DcsView.SetLicense(ApplicationContext, "your license number");
            }
            catch (Com.Dynamsoft.Camerasdk.Exception.DcsValueNotValidException e)
            {
                e.PrintStackTrace();
            }
            catch (Exception ex)
            {
            }
        }

        public void OnCancelTapped(DcsVideoView p0)
        {           
        }

        public void OnCaptureFailure(DcsVideoView p0, DcsException p1)
        {            
        }

        public void OnCaptureTapped(DcsVideoView p0)
        {            
        }

        public void OnDocumentDetected(DcsVideoView p0, DcsDocument p1)
        {
          
        }

        public void OnPostCapture(DcsVideoView p0, DcsImage p1)
        {          
        }

        public bool OnPreCapture(DcsVideoView p0)
        {
            return true;
        }

        //DcsViewListener
        public void OnCurrentViewChanged(DcsView p0, int lastView, int currentView)
        {
            if (currentView == DcsView.DveImagegalleryview)
            {
                tvShow.Visibility = Android.Views.ViewStates.Visible;                
            }
            else
            {
                tvShow.Visibility = Android.Views.ViewStates.Gone;

            }
        }

        public void OnClick(View v)
        {
            if(v.Id == Resource.Id.tv_show_id)
            {
                if (dcsView.CurrentView != DcsView.DveVideoview)
                {
                    dcsView.CurrentView = DcsView.DveVideoview;
                }
            }
        }
    }
}

