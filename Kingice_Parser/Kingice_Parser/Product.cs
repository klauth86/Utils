using System.Collections.Generic;

namespace Kingice_Parser {
    class Product {
        public int num = 0;
        public string imageSrc = "";
        public string productHref = "";
        public string productTitle = "";
        public string price = "";
        public string href = "";

        public List<string> Types = new List<string>();

        public bool IsValid() {
            return !string.IsNullOrEmpty(productTitle);
        }

        public override string ToString() {
            return string.Format("{0}@{1}@{2}@{3}@{4}@{5}",
                num, imageSrc.Trim(), productHref.Trim(), productTitle.Trim(), price.Trim(), string.Join(", ", Types).Trim());
        }
    }
}
