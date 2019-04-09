using System.Runtime.Serialization;

namespace Presentation_Generator.Models
{
    [DataContract]
    public class Slide
    {
        [DataMember]
        public string PathToBackgroundPicture { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Text { get; set; }

    }
}