namespace ICS.Web.User.Models
{
    public class MapModel
    {
        public string Groups { get; set; }
        public string Parkings { get; set; }
        public int MarkerId { get; set; }
        public MapModel(string parkings, string serviceGroups, int markerId)
        {
            Parkings = parkings;
            Groups = serviceGroups;
            MarkerId = markerId;
        }
    }
}
