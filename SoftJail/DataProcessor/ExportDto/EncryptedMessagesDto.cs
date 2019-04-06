using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ExportDto
{
    [XmlType("Message")]
    public class EncryptedMessagesDto
    {
        [XmlIgnore]
        public string InverseDescription { get; set; }

        [XmlElement("Description")]
        public string Description { get { return this.InverseDescription; } set { this.InverseDescription = Reverse(value); } }

        //<Message>
        //      <Description>!?sdnasuoht evif-ytnewt rof deksa uoy ro orez artxe na ereht sI</Description>
        //    </Message>
        public string Reverse(string text)
        {
            char[] cArray = text.ToCharArray();
            string reverse = string.Empty;
            for (int i = cArray.Length - 1; i > -1; i--)
            {
                reverse += cArray[i];
            }
            return reverse;
        }
    }

}