using System.Windows;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using System.IO;
using System.Net;
using System;

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

            for (int fileNum = 1; fileNum < 2; fileNum++) {
                string filePath = string.Format("C:\\Users\\Artur\\source\\repos\\Kingice_Parser\\Exports\\{0}.html", fileNum);

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
                            current.productHref = tmp.Substring(0, tmp.IndexOf("\"")).Trim();
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


                HtmlWeb web = new HtmlWeb();
                Certificates.Instance.GetCertificatesAutomatically();
                foreach (var product in result.Where(product=>product.IsValid())) {
                    var fullRef = hrefRoot + product.productHref;
                    var htmlDoc = web.LoadFromBrowser(fullRef);
                    var content = htmlDoc.Text;

                    string prev = "";
                    foreach (var item in content.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)) {

                        bool isDescription = prev == "<H3>Description</H3>";
                        if (isDescription) {
                            product.Details = new Details();
                            product.Details.Description = item.Substring(0, item.IndexOf("")).Replace("<DIV><SPAN>", "");
                        }
                        if (product.Details != null) {

                            if (item.Contains("<DIV><STRONG>")) {
                                var pair = item.Replace(":</STRONG> ", "^").Split('^').ToList();
                                if (pair.Count > 2)
                                    throw new Exception("Parse symbol is found is attribute values!");
                                var key = pair[0].Replace("<DIV><STRONG>", "").Trim();
                                var value = pair[1].Replace("</DIV>", "").Trim();
                                product.Details.DetailItems.Add(key, value);
                            }

                            if (item.Contains("<P class=variant-sku>")) {
                                product.Details.SKU = item.Replace("<P class=variant-sku>", "").Replace("</P>", "");
                            }
                        }
                        prev = item;
                    }
                }
                File.AppendAllLines("test.csv", new[] { result[0].GetHeaderRow() });
                File.AppendAllLines("test.csv", result.Where(product=>product.IsValid()).Select(product=>product.ToString()));
            }
        }   
    }
}
