using System.Windows;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using System.IO;

namespace Kingice_Parser {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void ButtonParse_Click(object sender, RoutedEventArgs e) {

            string hrefRoot = "https://www.kingice.com";

            for (int fileNum = 1; fileNum < 22; fileNum++) {
                string filePath = string.Format("C:\\Users\\Artur\\Downloads\\Html\\{0}.html", fileNum);

                List<Product> result = new List<Product>();
                Product current = null;                          
                int num = 0;

                var all = File.ReadAllLines(filePath);
                foreach (var item in all) {
                    if (item.Contains("<div class=\"box product\">")) {
                        if (current != null) {
                            result.Add(current);
                            num++;
                        }
                        current = new Product();
                    }

                    if (current != null) {
                        if (item.Contains("<img class=\"product_card__image\" src=")) {
                            var tmp = item.Replace("<img class=\"product_card__image\" src=\"", "");
                            current.imageSrc = tmp.Substring(0, tmp.IndexOf('"'));
                        }
                        if (item.Contains("<a href=\"/collections/all/") && item.Contains("class=\"title\">")) {
                            var tmp = item.Replace("<a href=\"", "");
                            current.href = tmp.Substring(0, tmp.IndexOf("\""));
                            current.productTitle = item.Substring(item.IndexOf('>')).Replace("</a>", "");
                        }
                        if (item.Contains("<span class=\"money\"><span class=money>")) {
                            var tmp = item.Replace("<span class=\"money\"><span class=money>", "");
                            current.price = tmp.Substring(0, tmp.IndexOf("</"));
                        }
                        if (item.Contains("<span class=\"product-swatches__label\">")) {
                            var tmp = item.Replace("<span class=\"product-swatches__label\">", "");
                            current.Types.Add(tmp.Substring(0, tmp.IndexOf("</")).Trim());
                        }
                    }
                }
                result.Add(current);

                File.AppendAllLines("test.csv", result.Where(product=>product.IsValid()).Select(product=>product.ToString()));
            }
        }   
    }
}
