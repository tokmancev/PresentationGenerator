using System.Runtime.Serialization;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Collections.Generic;

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