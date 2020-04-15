using System.Collections.Generic;

namespace Kingice_Parser {
    class Product {
        public int num = 0;
        public string imageSrc = "";
        public string productHref = "";
        public string productTitle = "";
        public string price = "";

        public List<string> Types = new List<string>();

        public Details Details = null;

        public bool IsValid() {
            return !string.IsNullOrEmpty(productTitle);
        }

        public string GetHeaderRow() {
            return string.Format("{0}@{1}@{2}@{3}@{4}@{5}@{6}@{7}@{8}",
    "Номер",//num, 
    "Изображение",//imageSrc.Trim(),
    "Отеосительная ссылка",//productHref.Trim(),
    "Наименование (Title)",//productTitle.Trim(),
    "Цена (Price)",//price.Trim(),
    "Материалы (circle)",//string.Join(", ", Types).Trim(),
    "Описание (Description)",//Details.Description.Trim(),
    "Подробные сведение (Details)",//string.Join(", ", Details.DetailItems),
    "Артикул (SKU)"//Details.SKU.Trim()
    );
        }

        public override string ToString() {
            if (Details == null)
            return string.Format("{0}@{1}@{2}@{3}@{4}@{5}",
                num, imageSrc.Trim(), productHref.Trim(), productTitle.Trim(), price.Trim(), string.Join(", ", Types).Trim());

            return string.Format("{0}@{1}@{2}@{3}@{4}@{5}@{6}@{7}@{8}",
                num, imageSrc.Trim(), productHref.Trim(), productTitle.Trim(), price.Trim(),
                string.Join(", ", Types).Trim(), Details.Description.Trim(),
                string.Join(", ", Details.DetailItems), Details.SKU.Trim());
        }
    }
}
