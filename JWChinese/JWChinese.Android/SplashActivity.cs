using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using System.Threading.Tasks;

namespace JWChinese.Droid
{
    [Activity(Label = "JW Chinese", Icon = "@drawable/ic_launcher", Theme = "@style/SplashScreen", WindowSoftInputMode = SoftInput.AdjustNothing, MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        // Splash screen timer
        private static int SPLASH_TIME_OUT = 1000;

        //protected override void OnCreate(Bundle savedInstanceState)
        //{
        //    base.OnCreate(savedInstanceState);

        //    SetContentView(Resource.Layout.SplashActivity);

        //    new Handler().PostDelayed(() =>
        //    {
        //        StartActivity(typeof(MainActivity));

        //    }, SPLASH_TIME_OUT);
        //}

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
        }

        protected override void OnResume()
        {
            base.OnResume();

            SetContentView(Resource.Layout.SplashActivity);

            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }

        // Prevent the back button from canceling the startup process
        public override void OnBackPressed() { }

        private async void SimulateStartup()
        {
            await Task.Delay(SPLASH_TIME_OUT); // Simulate a bit of startup work.
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}