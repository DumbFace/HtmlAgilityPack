using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HtmlAgilityPack
{
    public static class myCommon
    {
        public static string patternVideoYoutube = @"<figure class='media ck-widget' contenteditable='false'>
                                                    <div class='ck-media__wrapper' data-oembed-url='{0}'>
                                                    <div style='position: relative; padding-bottom: 100%; height: 0; padding-bottom: 56.2493%;'>
                                                   <iframe src='{0}' style='position: absolute; width: 100%; height: 100%; top: 0; left: 0;' frameborder='0' allow='autoplay; encrypted-media' allowfullscreen=''></iframe>
                                                    </div>
                                                    </div>
                                                </figure>";

        //Chèn video source
        public static string patternVideoSource = @"<figure class='media text-center ck-widget ck-widget_selected' contenteditable='false'>
                    <div class='ck-media__wrapper' data-oembed-url='{0}'>
                            <video width='720' controls=''>
                            <source src='{0}' type='video/mp4'>
                        </video>
                         </div>
                    </figure>";
    }
}