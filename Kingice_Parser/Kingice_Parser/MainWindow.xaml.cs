using System.Windows;
using System.Collections.Generic;
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
            for (int fileNum = 1; fileNum < 22; fileNum++) {
                string filePath = string.Format("C:\\Users\\Artur\\Downloads\\Html\\{0}.html", fileNum);
                var all = File.ReadAllLines(filePath);

                List<string> result = new List<string>();

                string imageSrc = "";
                string productHref = "";
                string productTitle = "";
                string price = "";
                List<string> Types = new List<string>();
                int num = 0;
                bool isFirst = true;

                foreach (var item in all) {
                    if (item.Contains("<div class=\"box product\">")) {
                        if (isFirst) {
                            isFirst = false;
                        }
                        else {
                            result.Add(string.Format("{0}@{1}@{2}@{3}@{4}@{5}", num, imageSrc.Trim(), productHref.Trim(), productTitle.Trim(), price.Trim(), string.Join(", ", Types).Trim()));
                            num++;
                            Types.Clear();
                        }
                    }

                    if (item.Contains("<img class=\"product_card__image\" src=")) {
                        var tmp = item.Replace("<img class=\"product_card__image\" src=\"", "");
                        imageSrc = tmp.Substring(0, tmp.IndexOf('"'));
                    }
                    if (item.Contains("<a href=\"/collections/all/") && item.Contains("class=\"title\">")) {
                        productTitle = item.Substring(item.IndexOf('>')).Replace("</a>", "");
                    }
                    if (item.Contains("<span class=\"money\"><span class=money>")) {
                        var tmp = item.Replace("<span class=\"money\"><span class=money>", "");
                        price = tmp.Substring(0, tmp.IndexOf("</"));
                    }
                    if (item.Contains("<span class=\"product-swatches__label\">")) {
                        var tmp = item.Replace("<span class=\"product-swatches__label\">", "");
                        Types.Add(tmp.Substring(0, tmp.IndexOf("</")).Trim());
                    }
                }
                result.Add(string.Format("{0}@{1}@{2}@{3}@{4}@{5}", num, imageSrc.Trim(), productHref.Trim(), productTitle.Trim(), price.Trim(), string.Join(", ", Types).Trim()));
                Types.Clear();

                File.AppendAllLines("test.csv", result);
            }
        }   
    }
}
