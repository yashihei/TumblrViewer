using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.IO;

using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace tumblrAppWPF
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Elysium.Controls.Window
    {
        string domain = "gazo0141.tumblr.com";
        string api_key = "GesVlVXWDOv1DKzVowf2jqsY8ndlgag0Uxy8HlLzN8AXAYd8FE";
        string url = "http://api.tumblr.com/v2/blog/";

        List<BitmapImage> images = new List<BitmapImage>();
        int index = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //var json = (new WebClient() { Encoding = Encoding.UTF8 }).DownloadString(url + domain + "/posts/photo?api_key=" + api_key);
            //var json = (new WebClient() { Encoding = Encoding.UTF8 }).DownloadString("http://b.hatena.ne.jp/entry/jsonlite/?url=http://google.com");

            //var req = WebRequest.Create(blog);
            //var req = (HttpWebRequest)WebRequest.Create(blog);
            //req.Headers.Add("Accept-Language:ja,en-US;q=0.8,en;q=0.6");
            var req = WebRequest.Create(url + domain + "/posts/photo?api_key=" + api_key);
            var stream = req.GetResponse().GetResponseStream();

            //var serializer = new DataContractJsonSerializer(typeof(TumblrPost));
            //TumblrPost result = (TumblrPost)serializer.ReadObject(stream);
            
            //JSON出力
            using (var sr = new StreamReader(stream))
            {
                var sw = new StreamWriter("out.txt");
                var json = sr.ReadToEnd();

                //using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json))) {
                //    var data = (TumblrPost)serializer.ReadObject(ms);
                //}
                
                //sw.WriteLine(sr.ReadToEnd());
                //sw.Write(json);

                //Console.Write(json);

                var result = JsonConvert.DeserializeObject<TumblrPost>(json);
                //Console.WriteLine(result.response.posts);
                
                foreach (var post in result.response.posts) {
                    String imgUrl = post.photos[0].original_size.url;
                    Console.WriteLine(imgUrl);
                    images.Add(new BitmapImage(new Uri(imgUrl)));
                    //var imgReq = WebRequest.Create(post.photos[0].original_size.url);
                    //var imgStream = imgReq.GetResponse().GetResponseStream();
                }

                //Console.WriteLine(result);
                //Console.WriteLine(sr.ReadToEnd());
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            index--;
            image1.Source = images[index];
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            index++;
            image1.Source = images[index];
        }
    }
    
    public class TumblrPost {
        public class Meta
        {
            public int status { get; set; }
            public string msg { get; set; }
        }

        public class Blog
        {
            public string title { get; set; }
            public string name { get; set; }
            public int posts { get; set; }
            public string url { get; set; }
            public int updated { get; set; }
            public string description { get; set; }
            public bool ask { get; set; }
            public string ask_page_title { get; set; }
            public bool ask_anon { get; set; }
            public bool is_nsfw { get; set; }
            public bool share_likes { get; set; }
            public int likes { get; set; }
        }

        public class AltSize
        {
            public int width { get; set; }
            public int height { get; set; }
            public string url { get; set; }
        }

        public class OriginalSize
        {
            public int width { get; set; }
            public int height { get; set; }
            public string url { get; set; }
        }

        public class Exif
        {
            public string Camera { get; set; }
            public int ISO { get; set; }
            public string Aperture { get; set; }
            public string Exposure { get; set; }
            public string FocalLength { get; set; }
        }

        public class Photo
        {
            public string caption { get; set; }
            public List<AltSize> alt_sizes { get; set; }
            public OriginalSize original_size { get; set; }
            public Exif exif { get; set; }
        }

        public class Post
        {
            public string blog_name { get; set; }
            public object id { get; set; }
            public string post_url { get; set; }
            public string slug { get; set; }
            public string type { get; set; }
            public string date { get; set; }
            public int timestamp { get; set; }
            public string state { get; set; }
            public string format { get; set; }
            public string reblog_key { get; set; }
            public List<object> tags { get; set; }
            public string short_url { get; set; }
            public List<object> highlighted { get; set; }
            public int note_count { get; set; }
            public string source_url { get; set; }
            public string source_title { get; set; }
            public string caption { get; set; }
            public string link_url { get; set; }
            public string image_permalink { get; set; }
            public List<Photo> photos { get; set; }
            public string photoset_layout { get; set; }
        }

        public class Response
        {
            public Blog blog { get; set; }
            public List<Post> posts { get; set; }
            public int total_posts { get; set; }
        }

        public Meta meta { get; set; }
        public Response response { get; set; }
    }
}
