namespace Dedy.Models
{
    public class Filter
    {
        public string Name { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public bool Color_Black { get; set; }
        public bool Color_White { get; set; }
        public bool Color_Red { get; set; }
        public string Material { get; set; }
        public string Orientation { get; set; }

    }
}
