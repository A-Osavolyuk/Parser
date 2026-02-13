namespace Parser.Parsing;

public static class Locators
{
    public static class Rows
    {
        public const string Rank = "td.cmc-table__cell--sort-by__rank";
        public const string Name = "td.cmc-table__cell--sort-by__nam";
        public const string Symbol = "td.cmc-table__cell--sort-by__symbol";
        public const string Price = "td.cmc-table__cell--sort-by__price";
        public const string MarketCap = "td.cmc-table__cell--sort-by__market-cap";
        public const string Volume24H = "td.cmc-table__cell--sort-by__volume-24-h";
        public const string PercentChange24H = "td.cmc-table__cell--sort-by__percent-change-24-h";
    }

    public static class Elements
    {
        public const string Row = "tbody tr.cmc-table-row";
    }
}