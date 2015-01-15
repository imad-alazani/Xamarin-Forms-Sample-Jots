using Stackoverflow.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Labs;
using Xamarin.Forms.Labs.Services;
using Xamarin.Forms.Labs.Services.Media;

namespace Stackoverflow
{
    public class App
    {
        public static Page GetMainPage()
        {
            return new mediapicker();
        }
    }

}
