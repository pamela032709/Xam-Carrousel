using System;
namespace CarrouselTest.Model
{
    public class Details
    {

        public string Name { get; set; }
        public string Location { get; set; }

        //URL for our monkey image!
        public string Image { get; set; }

        public string NameSort => Name[0].ToString();

    }
}
