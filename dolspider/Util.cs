using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Net;
using EncodeMy;
using Dol.Base.DataModel;

namespace dolspider
{
    public abstract class Util
    {
        public static string GetContent(Uri uri, Encoding encoding)
        {
            WebClient wc = new WebClient();
            wc.Headers.Add("Accept: */*");
            wc.Headers.Add("User-Agent: Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; Trident/4.0; .NET4.0E; .NET4.0C; InfoPath.2; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729; SE 2.X MetaSr 1.0)");
            var bytes = wc.DownloadData(uri);
            return encoding.GetString(bytes);
        }

        public static HtmlDocument GetDoc(Uri uri, Encoding encoding)
        {
            Console.Out.Write("请求" + uri.AbsoluteUri + "...");
            var content = GetContent(uri, encoding);
            content=Consistency.Parse(content);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(content);
            Console.Out.WriteLine("请求完成。");
            return doc;
        }
    }

    public class POIs
    {
        public static string[] Citys = cityNames();

        public static string[] cityNames()
        {
            CityDM dm = new CityDM();
            return dm.Load().Select(city =>
            {
                return city.Name;
            }).ToArray();
        }

        public static string[] Land = new string[]{
            "不列颠岛北岸","不列颠岛东岸",
            "不列颠岛南岸","比斯开湾南岸","法国西北岸","波斯尼亚湾西岸",
            "波罗的海北岸","波罗的海东南岸","波罗的海西南岸","斯堪地那维亚西岸",
            "非洲北岸","摩洛哥西岸","南地中海非洲北岸","土耳其西岸",
            "土耳其北岸","黑海东岸","黑海西北岸","埃及北岸",
            "开罗对岸","尼罗河中游","尼罗河上游","非洲西北岸",
            "非洲西岸","大西洋中部西岸","非洲几内亚湾北部","南非西岸",
            "南非西南岸","非洲南岸","孟加拉湾东北岸","印度洋非洲东岸",
            "南非东南岸","南非东岸","波斯湾北岸","阿拉伯海东北岸",
            "阿拉伯海西北岸","红海西岸","红海东岸","印度洋西岸",
            "墨西哥湾西南岸","亚马逊河上游","南美东北岸","南美东南岸",
            "苏门答腊岛西南岸","爪哇岛北岸","苏拉威西岛东岸","南美西南岸",
            "北美大陆东岸","北美大陆东南岸"
        };
    }

    public abstract class Consistency
    {
        private static EncodeRobert edControl = new EncodeRobert();
        public static string Parse(string input)
        {
            input = edControl.SCTCConvert(ConvertType.Traditional, ConvertType.Simplified, input);

            input = input.Replace("维拉克鲁斯", "韦拉克鲁斯");
            input = input.Replace("罗安达", "卢安达");
            input = input.Replace("伊斯坦堡", "伊斯坦布尔");
            input = input.Replace("圣多明哥", "圣多明各");
            input = input.Replace("圣地牙哥", "圣地亚哥");
            input = input.Replace("伊斯坦堡", "伊斯坦布尔");
            input = input.Replace("塞维尔", "塞维利亚");
            input = input.Replace("拿坡里", "那不勒斯");
            input = input.Replace("卡法", "刻赤");
            input = input.Replace("甘地亚", "坎迪亚");
            input = input.Replace("麻六甲", "马六甲");
            input = input.Replace("萨沙里", "萨萨里");
            input = input.Replace("第里雅斯特", "的里雅斯特");
            input = input.Replace("默苏利珀德姆", "马苏利帕特南");
            input = input.Replace("荷姆兹", "霍尔木兹");
            input = input.Replace("麻林地", "马林迪");
            input = input.Replace("瓜地马拉", "危地马拉");
            input = input.Replace("岚屿", "格兰");
            input = input.Replace("洛布里", "华富里");
            input = input.Replace("荷巴特", "霍巴特");
            input = input.Replace("奥德萨", "敖德萨");
            input = input.Replace("塞拉里昂", "塞拉利昂");
            input = input.Replace("拿波里", "那不勒斯");
            input = input.Replace("开普", "开普敦");

            
            return input;
        }
    }

    public class Escape
    {
        private String[] hex = {
        "00","01","02","03","04","05","06","07","08","09","0A","0B","0C","0D","0E","0F",
        "10","11","12","13","14","15","16","17","18","19","1A","1B","1C","1D","1E","1F",
        "20","21","22","23","24","25","26","27","28","29","2A","2B","2C","2D","2E","2F",
        "30","31","32","33","34","35","36","37","38","39","3A","3B","3C","3D","3E","3F",
        "40","41","42","43","44","45","46","47","48","49","4A","4B","4C","4D","4E","4F",
        "50","51","52","53","54","55","56","57","58","59","5A","5B","5C","5D","5E","5F",
        "60","61","62","63","64","65","66","67","68","69","6A","6B","6C","6D","6E","6F",
        "70","71","72","73","74","75","76","77","78","79","7A","7B","7C","7D","7E","7F",
        "80","81","82","83","84","85","86","87","88","89","8A","8B","8C","8D","8E","8F",
        "90","91","92","93","94","95","96","97","98","99","9A","9B","9C","9D","9E","9F",
        "A0","A1","A2","A3","A4","A5","A6","A7","A8","A9","AA","AB","AC","AD","AE","AF",
        "B0","B1","B2","B3","B4","B5","B6","B7","B8","B9","BA","BB","BC","BD","BE","BF",
        "C0","C1","C2","C3","C4","C5","C6","C7","C8","C9","CA","CB","CC","CD","CE","CF",
        "D0","D1","D2","D3","D4","D5","D6","D7","D8","D9","DA","DB","DC","DD","DE","DF",
        "E0","E1","E2","E3","E4","E5","E6","E7","E8","E9","EA","EB","EC","ED","EE","EF",
        "F0","F1","F2","F3","F4","F5","F6","F7","F8","F9","FA","FB","FC","FD","FE","FF"
    };
        private byte[] val = {
        0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,
        0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,
        0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,
        0x00,0x01,0x02,0x03,0x04,0x05,0x06,0x07,0x08,0x09,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,
        0x3F,0x0A,0x0B,0x0C,0x0D,0x0E,0x0F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,
        0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,
        0x3F,0x0A,0x0B,0x0C,0x0D,0x0E,0x0F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,
        0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,
        0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,
        0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,
        0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,
        0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,
        0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,
        0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,
        0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,
        0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F,0x3F
    };
        public String escape(String s)
        {
            StringBuilder sbuf = new StringBuilder();
            int len = s.Length;
            for (int i = 0; i < len; i++)
            {
                int ch = s[i];
                if ('A' <= ch && ch <= 'Z')
                {    // 'A'..'Z' : as it was
                    sbuf.Append((char)ch);
                }
                else if ('a' <= ch && ch <= 'z')
                {    // 'a'..'z' : as it was
                    sbuf.Append((char)ch);
                }
                else if ('0' <= ch && ch <= '9')
                {    // '0'..'9' : as it was
                    sbuf.Append((char)ch);
                }
                else if (ch == '-' || ch == '_'       // unreserved : as it was
                  || ch == '.' || ch == '!'
                  || ch == '~' || ch == '*'
                  || ch == '\'' || ch == '('
                  || ch == ')')
                {
                    sbuf.Append((char)ch);
                }
                else if (ch <= 0x007F)
                {              // other ASCII : map to %XX
                    sbuf.Append('%');
                    sbuf.Append(hex[ch]);
                }
                else
                {                                // unicode : map to %uXXXX
                    sbuf.Append('%');
                    sbuf.Append('u');
                    int cht = ch;
                    sbuf.Append(hex[(ch >>= 8)]);
                    sbuf.Append(hex[(0x00FF & cht)]);
                }
            }
            return sbuf.ToString();
        }
        public String unescape(String s)
        {
            StringBuilder sbuf = new StringBuilder();
            int i = 0;
            int len = s.Length;
            while (i < len)
            {
                int ch = s[i];
                if ('A' <= ch && ch <= 'Z')
                {    // 'A'..'Z' : as it was
                    sbuf.Append((char)ch);
                }
                else if ('a' <= ch && ch <= 'z')
                {    // 'a'..'z' : as it was
                    sbuf.Append((char)ch);
                }
                else if ('0' <= ch && ch <= '9')
                {    // '0'..'9' : as it was
                    sbuf.Append((char)ch);
                }
                else if (ch == '-' || ch == '_'       // unreserved : as it was
                  || ch == '.' || ch == '!'
                  || ch == '~' || ch == '*'
                  || ch == '\'' || ch == '('
                  || ch == ')')
                {
                    sbuf.Append((char)ch);
                }
                else if (ch == '%')
                {
                    int cint = 0;
                    if ('u' != s[i + 1])
                    {         // %XX : map to ascii(XX)
                        cint = (cint << 4) | val[s[i + 1]];
                        cint = (cint << 4) | val[s[i + 2]];
                        i += 2;
                    }
                    else
                    {                            // %uXXXX : map to unicode(XXXX)
                        cint = (cint << 4) | val[s[i + 2]];
                        cint = (cint << 4) | val[s[i + 3]];
                        cint = (cint << 4) | val[s[i + 4]];
                        cint = (cint << 4) | val[s[i + 5]];
                        i += 5;
                    }
                    sbuf.Append((char)cint);
                }
                i++;
            }
            return sbuf.ToString();
        }
    }
}
