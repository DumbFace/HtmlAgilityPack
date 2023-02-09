

using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace MyProject;
class Program
{
    static async Task Main(string[] args)
    {
        Program program = new Program();
        var htmlString = program.GetHtmlAsync("https://tiin.vn/chuyen-muc/phim-chau-a/hoan-hon-2-tap-8-lee-jae-wook-mot-long-xin-cuoi-go-yoon-jung-ngay-khi-dang-gai-phat-hien-minh-la-ai.html").Result;
        string html = @"
        <div style='text-align: center;' class='content-img'><img src='https://static1.bestie.vn/Mlog/ImageContent/202202/yeu-nu-hang-hieu-nam-2022-goi-ten-hoang-thuy-linh-c9608a.jpg' alt='Yêu nữ hàng hiệu năm 2022 gọi tên Hoàng Thùy Linh'><div class='content-img-caption'>
        Bộ trang phục lấy cảm hứng từ &aacute;o b&agrave; ba&nbsp;v&agrave;&nbsp;chiếc khăn rằn đặc trưng của người d&acirc;n miền T&acirc;y. Stylist Ho&agrave;ng Ku kh&eacute;o l&eacute;o kết hợp c&ugrave;ng v&ograve;ng chocker, gi&agrave;y đế bệt v&agrave;&nbsp;tất để bộ trang phục hiện đại hơn. Ảnh: FB Ho&agrave;ng Ku</div></div>

        <div style='text-align: center;' class='content-img'><img src='https://static1.bestie.vn/Mlog/ImageContent/202202/yeu-nu-hang-hieu-nam-2022-goi-ten-hoang-thuy-linh-3039a0.jpg' alt='Yêu nữ hàng hiệu năm 2022 gọi tên Hoàng Thùy Linh'><div class='content-img-caption'>
        Ho&agrave;ng Th&ugrave;y Linh nổi bật với set đồ mang m&agrave;u sắc rực rỡ, phối l&ocirc;ng vũ, b&ecirc;n tr&ecirc;n&nbsp;c&oacute; đ&iacute;nh 9 con rồng tượng trưng cho 9 nh&aacute;nh s&ocirc;ng Cửu Long của miền T&acirc;y Việt Nam.&nbsp;Ảnh: FB Ho&agrave;ng Ku</div></div>
";
        HtmlDocument htmlDoc = new HtmlDocument();

        // string htmlString = await GetHtmlAsync(url);

        if (html != null && html != "")
        {
            htmlDoc.LoadHtml(html);
        }


        var nodeImage = htmlDoc.DocumentNode.SelectNodes("//div[@class='content-img']");

        if (nodeImage != null)
        {
            foreach (var item in nodeImage)
            {
                item.Name = "figure";
                Console.WriteLine(item.OuterHtml);
            }
        }

        Console.ReadKey();
    }

    public async Task<string> GetHtmlAsync(string url)
    {
        using (var request = new HttpClient())
        {
            request.DefaultRequestHeaders.Add("User-Agent", "C# program");
            return await request.GetStringAsync(url);
        }
    }
    public string GetBody(HtmlDocument htmlDoc, string whichWeb)
    {
        string bodyHtml = "";
        switch (whichWeb)
        {
            case "yan":
                bodyHtml = GetContentYan(htmlDoc);
                break;
            case "bestie":
                bodyHtml = GetContentBestie(htmlDoc);
                break;
            case "dan":
                bodyHtml = GetContentDAN(htmlDoc);
                break;
            case "tiin":
                bodyHtml = GetContentTiinVN(htmlDoc);
                break;
            case "kenh14":
                bodyHtml = GetContentKenh14(htmlDoc);
                break;
        }

        return bodyHtml;
    }

    // public tblNews GetMetaTag(HtmlDocument htmlDoc)
    // {
    //     tblNews tbl = new tblNews();
    //     HtmlNodeCollection metaNodes = SelectMetaNodes("meta", "property", "og:", htmlDoc);

    //     if (metaNodes != null)
    //     {
    //         foreach (HtmlNode node in metaNodes)
    //         {
    //             if (node.Attributes["property"].Value.ToLower() == "og:title")
    //             {
    //                 //Giải mã các kí tự " & < >... có trong title
    //                 tbl.Title = WebUtility.HtmlDecode(node.Attributes["content"].Value);
    //             }
    //             if (node.Attributes["property"].Value.ToLower() == "og:description")
    //             {
    //                 tbl.Teaser = WebUtility.HtmlDecode(node.Attributes["content"].Value);
    //             }
    //             if (node.Attributes["property"].Value.ToLower() == "og:image")
    //             {
    //                 tbl.Avatar2x1 = node.Attributes["content"].Value;
    //                 tbl.AvatarFB = node.Attributes["content"].Value;
    //             }
    //         }
    //     }
    //     return tbl;
    // }

    public string GetContentTiinVN(HtmlDocument htmlDoc)
    {
        Program program = new Program();
        string bodyContent = "";
        HtmlNode bodyNode = SelectSingleNode("div", "class", "detail-content", htmlDoc);
        //HtmlNode bodyVideoNode = SelectSingleNode("div", "class", "video-page", htmlDoc);
        // HtmlNode bodyVideoNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='container video-page']");
        //HtmlNode bodyVideoNode = htmlDoc.DocumentNode.SelectSingleNode(@"//div[contains('@class','video-page')]/div[contains('@class','col-md-7')]");

        //1
        HtmlNode bodyVideoNode = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class,'video-page')]//div[@class='col-md-7 top']");

        //2
        // HtmlNode bodyVideoNode = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class,'video-page')]//div[contains(@class,'col-md-7')]");

        if (bodyNode != null)
        {
            //Bỏ quảng cáo, tin tức liên quan...
            //...
            HtmlNodeCollection nodeScripts = bodyNode.SelectNodes("//script");
            if (nodeScripts != null)
            {
                foreach (HtmlNode node in nodeScripts)
                {
                    node.Remove();
                }
            }

            //Chú thích giữa
            HtmlNodeCollection nodeCenterText = bodyNode.SelectNodes("//p[@class='p-chuthich']");
            if (nodeCenterText != null)
            {
                foreach (HtmlNode node in nodeCenterText)
                {
                    node.Attributes.Add(htmlDoc.CreateAttribute("style", "text-align:center"));
                }

            }


            //Lấy hình ảnh trang tiin
            HtmlNodeCollection nodeImages = htmlDoc.DocumentNode.SelectNodes("//p//img");
            if (nodeImages != null)
            {
                foreach (HtmlNode node in nodeImages)
                {
                    node.Attributes.Add("src", node.Attributes["data-src"].Value);
                }
            }


            //Chèn video source
            //HtmlNodeCollection nodeVideos = htmlDoc.DocumentNode.SelectNodes("//div[@class='hot_news_img']//video//source");
            HtmlNodeCollection nodeVideos = htmlDoc.DocumentNode.SelectNodes("//video//source");
            if (nodeVideos != null)
            {
                foreach (HtmlNode node in nodeVideos)
                {
                    string srcVideo = node.GetAttributeValue("src", "");
                    string videoFrame = string.Format(myCommon.patternVideoSource, srcVideo);

                    HtmlNode newIframe = HtmlNode.CreateNode(videoFrame);
                    node.ChildNodes.Add(newIframe);
                }
            }

            //Chèn video youtube
            HtmlNodeCollection nodeIframes = htmlDoc.DocumentNode.SelectNodes("//p//iframe/..");
            if (nodeIframes != null)
            {
                foreach (HtmlNode node in nodeIframes)
                {
                    HtmlNode childNode = node.SelectSingleNode("//iframe");

                    string srcVideo = childNode.Attributes["src"].Value;
                    string videoFrame = string.Format(myCommon.patternVideoYoutube, srcVideo);

                    node.RemoveAll();

                    HtmlNode newIframe = HtmlNode.CreateNode(videoFrame);
                    node.ChildNodes.Add(newIframe);
                }
            }

            bodyContent = bodyNode.InnerHtml;
        }
        else if (bodyVideoNode != null)
        {
            //HtmlNodeCollection nodeVideos = htmlDoc.DocumentNode.SelectNodes("//div[@class='hot_news_img']//video//source");
            HtmlNodeCollection nodeVideos = htmlDoc.DocumentNode.SelectNodes("//video//source");
            if (nodeVideos != null)
            {
                foreach (HtmlNode node in nodeVideos)
                {
                    string srcVideo = node.GetAttributeValue("src", "");
                    string videoFrame = string.Format(myCommon.patternVideoSource, srcVideo);

                    HtmlNode newIframe = HtmlNode.CreateNode(videoFrame);
                    node.ChildNodes.Add(newIframe);
                }
            }
        }
        return bodyContent;
    }

    public string GetContentKenh14(HtmlDocument htmlDoc)
    {
        string bodyContent = "";
        HtmlNode bodyNode = SelectSingleNode("div", "class", "knc-content", htmlDoc);
        if (bodyNode != null)
        {
            //Bỏ quảng cáo, tin tức liên quan...
            //...

            bodyContent = bodyNode.InnerHtml;
        }
        return bodyContent;
    }

    public string GetContentYan(HtmlDocument htmlDoc)
    {
        string bodyContent = "";
        HtmlNode bodyNode = SelectSingleNode("div", "id", "contentBody", htmlDoc);
        if (bodyNode != null)
        {
            //Bỏ quảng cáo, tin tức liên quan...
            //...

            bodyContent = bodyNode.InnerHtml;
        }

        return bodyContent;
    }

    public string GetContentBestie(HtmlDocument htmlDoc)
    {
        string bodyContent = "";
        HtmlNode bodyNode = SelectSingleNode("div", "id", "postContent", htmlDoc);
        if (bodyNode != null)
        {
            //Bỏ quảng cáo, tin tức liên quan...
            //...

            bodyContent = bodyNode.InnerHtml;
        }
        return bodyContent;
    }
    public string GetContentDAN(HtmlDocument htmlDoc)
    {
        string bodyContent = "";
        HtmlNode bodyNode = SelectSingleNode("div", "class", "knc-content", htmlDoc);
        if (bodyNode != null)
        {
            //Bỏ quảng cáo, tin tức liên quan...
            //...

            bodyContent = bodyNode.InnerHtml;
        }
        return bodyContent;
    }

    public void RemoveChild(string element, string attribute, string name, HtmlNode HtmlDocument)
    {
        HtmlDocument.SelectSingleNode($"//{element}[@{attribute}='{name}']").Remove();
    }

    public HtmlNode SelectSingleNode(string element, string attribute, string name, HtmlDocument HtmlDocument)
    {
        return HtmlDocument.DocumentNode.SelectSingleNode($"//{element}[@{attribute}='{name}']");
    }
    public HtmlNodeCollection SelectMetaNodes(string element, string attribute, string name, HtmlDocument HtmlDocument)
    {
        return HtmlDocument.DocumentNode.SelectNodes($"//{element}[contains(@{attribute},'{name}')]");
    }
}

